using System;
using System.Collections.Generic;
using System.Drawing;
using TestJeux.API.Services;
using TestJeux.Business.Entities.Action;
using TestJeux.Business.Managers.API;
using TestJeux.Business.ObjectValues;
using TestJeux.Business.Services.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities;
using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	public class MoveService : IMoveService
    {
        GameAggregate _gameRoot;
        private readonly List<ItemModel> _items;
        private readonly ILevelService _levelManager;
        private readonly IActionManager _actionManager;
        private ActionFabric _actionFabric;

        public event EventHandler<MovementDto> MoveStarted;

        public MoveService(GameAggregate gameRoot, ILevelService levelManager, IActionManager actionManager)
        {
            _gameRoot = gameRoot;
            _levelManager = levelManager;
            _actionManager = actionManager;
            _items = new List<ItemModel>();
        }

        public void Initialize(ActionFabric actionFabric)
        {
            _actionFabric = actionFabric;
        }

        public void Register(int itemID)
        {
            _items.Add(_gameRoot.GetItemFromCurrentLevel(itemID));
        }

        public void Unregister(int itemID)
        {
            _items.Remove(_gameRoot.GetItemFromCurrentLevel(itemID));
        }

        /// <summary>
        /// Execute move actions then obtain reactions on item move from all registered items  
        /// </summary>
        /// <param name="itemModel"></param>
        public void NotifyMove(ItemModel itemModel)
        {
            var actions = GetLevelChangeActionBasedOnGround(_levelManager.GetGroundType(_gameRoot.GetCurrentLevel().ID, itemModel.Position));
            if (actions.Count > 0)
            {
                foreach (var action in actions)
                    action.Execute();
                return;
            }

            foreach (var item in _items)
            {
                if (item == itemModel)
                    continue;
                var reactions = item.ReactMove(itemModel);
                foreach(var reaction in reactions)
                {
                    if (reaction.ReactionType == Reactions.Specific)
                        actions.AddRange(item.GetSpecificReaction(reaction.ScriptId, itemModel));
                    if (reaction.ReactionType == Reactions.Flee)
                        actions.AddRange(GetFleeMoves(item.ID));
                }
				_actionManager.AddRangeActions(actions);
            }            
        }

        /// <summary>
        /// Obtain flee actions for item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
		private List<ActionBase> GetFleeMoves(int itemId)
		{
			var actions = new List<ActionBase>();
			if (_gameRoot.GetCurrentLevel() == null || !_gameRoot.HasItemInCurrentLevel(itemId))
				return actions;

			var item = _gameRoot.GetItemFromCurrentLevel(itemId);

			var directions = GetAvailableDirections(new Point(item.X, item.Y), item.GetMoveType());
			DirectionEnum fleeDirection = DirectionEnum.Top;
			if (directions.Count == 1)
				fleeDirection = directions[0];
			else if (directions.Count == 2)
				fleeDirection = directions[new Random(DateTime.Now.Millisecond).Next(0, directions.Count - 1)];
			else if (!directions.Contains(DirectionEnum.Bottom))
				fleeDirection = DirectionEnum.Top;
			else if (!directions.Contains(DirectionEnum.Top))
				fleeDirection = DirectionEnum.Bottom;
			else if (!directions.Contains(DirectionEnum.Left))
				fleeDirection = DirectionEnum.Right;
			else if (!directions.Contains(DirectionEnum.Right))
				fleeDirection = DirectionEnum.Left;
			item.Orientation = fleeDirection;
			var groundType = _levelManager.GetGroundType(_gameRoot.GetCurrentLevel().ID, GetTargetPosition(item.Position, fleeDirection));
			actions.Add(_actionFabric.CreateMoveAction(item, groundType));
			return actions;
		}

		/// <summary>
		/// Get level change action based on ground type
		/// </summary>
		/// <param name="ground"></param>
		/// <returns></returns>
		private List<ActionBase> GetLevelChangeActionBasedOnGround(GroundType ground)
        {
            var actions = new List<ActionBase>();
            if (ground == GroundType.GoUp)
                actions.Add(_actionFabric.CreateLevelChangeAction(_levelManager.GetCurrentLevel() - 1));
            else if (ground == GroundType.GoDown)
                actions.Add(_actionFabric.CreateLevelChangeAction(_levelManager.GetCurrentLevel() + 1));
            return actions;
        }

        /// <summary>
        /// Check that move type is compatible with target position
        /// </summary>
        /// <param name="moveType"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool CanMove(MoveType moveType, Point position)
        {
            if (IsPositionOccupied(position) >= 0)
                return false;

            var groundType = GetUpdatedGroundType(_levelManager.GetGroundType(_gameRoot.GetCurrentLevel().ID, position), HasGroundModifier(position));            

            return !GetNotAuthorizedGroundTypes(moveType).Contains(groundType);
        }

        private GroundType GetUpdatedGroundType(GroundType groundType, GroundModifier groundModifier)
        {
            if (groundModifier == GroundModifier.Bridge && groundType == GroundType.Water)
                return GroundType.Neutral;
            return groundType;
        }

        public List<DirectionEnum> GetAvailableDirections(Point pos, MoveType moveType)
        {
            var directions = new List<DirectionEnum>();

            // check Top
            if (CanMove(moveType, new Point(pos.X, pos.Y - 50)))
                directions.Add(DirectionEnum.Top);

            // check Bottom
            if (CanMove(moveType, new Point(pos.X, pos.Y + 50)))
                directions.Add(DirectionEnum.Bottom);

            // check Left
            if (CanMove(moveType, new Point(pos.X -50, pos.Y)))
                directions.Add(DirectionEnum.Left);

            // check Right
            if (CanMove(moveType, new Point(pos.X + 50, pos.Y)))
                directions.Add(DirectionEnum.Right);

            return directions;
        }

        private List<GroundType> GetNotAuthorizedGroundTypes(MoveType moveType)
        {
            var groundTypes= new List<GroundType>();
            switch(moveType)
            {
                case MoveType.Walk:
                    groundTypes.Add(GroundType.Water);
                    break;
                case MoveType.Swim:
                    groundTypes.Add(GroundType.Neutral);
                    groundTypes.Add(GroundType.GoDown);
                    groundTypes.Add(GroundType.GoUp);
                    groundTypes.Add(GroundType.Bridge);
                    break;
            }

            groundTypes.Add(GroundType.Block);
            groundTypes.Add(GroundType.OutOfBound);
            return groundTypes;
        }

        /// <summary>
        /// Move item in direction
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="itemId"></param>
        public void MoveCharacter(DirectionEnum direction, int itemId)
        {
            var item = _items.Find(i => i.ID == itemId);
            item.Refresh();
            item.Orientation = direction;
			item.Move(new Movement(1, direction));
        }

        /// <summary>
        /// Check if any item occupy the target position
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int IsPositionOccupied(Point point)
        {
            int groundItemId = -1;
            foreach (var item in _items)
                if (item.X == point.X && item.Y == point.Y)
                {
                    if (!item.IsGround)
                        return item.ID;
                }
            return groundItemId;
        }

        /// <summary>
        /// Check if any item affects the target ground type 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public GroundModifier HasGroundModifier(Point point)
        {
            foreach (var item in _items)
                if (item.X == point.X && item.Y == point.Y)
                {
                    if (item.IsGround)
                        return GroundModifier.Bridge;
                }
            return GroundModifier.None;
        }

        /// <summary>
        /// Get target position based on direction
        /// </summary>
        /// <param name="point"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Point GetTargetPosition(Point point, DirectionEnum direction)
        {
            var targetPoint = new Point(point.X, point.Y);
            switch (direction)
            {
                case DirectionEnum.Right:
                    targetPoint.X += ConstantesDisplay.UnitSize;
                    break;
                case DirectionEnum.Left:
                    targetPoint.X -= ConstantesDisplay.UnitSize;
                    break;
                case DirectionEnum.Top:
                    targetPoint.Y -= ConstantesDisplay.UnitSize;
                    break;
                case DirectionEnum.Bottom:
                    targetPoint.Y += ConstantesDisplay.UnitSize;
                    break;
            }
            return targetPoint;
        }

        /// <summary>
        /// Reset manager
        /// </summary>
        public void Reset()
        {
            _items.Clear();
        }

		public void NotifyEndOfMove(int itemId)
		{
            var item = _gameRoot.GetItemFromCurrentLevel(itemId);
			item.IsMoving = false;
			item.Refresh();
			NotifyMove(item);
		}

		public void Subscribe()
		{
            System.Diagnostics.Debug.WriteLine("Subscribe once");
			foreach (var item in _gameRoot.GetItems())
				item.Moved += OnItemMoved;
		}

		public void Unsubscribe()
		{
            if (_gameRoot.GetCurrentLevel() == null)
                return;

			foreach (var item in _gameRoot.GetItems())
				item.Moved -= OnItemMoved;
		}

		private void OnItemMoved(object sender, Movement e)
		{
			System.Diagnostics.Debug.WriteLine("Item moved raised");
			if (sender == null || !Int32.TryParse(sender.ToString(), out var itemId))
                return;

			if (MoveStarted != null)
				MoveStarted(itemId, new MovementDto() { UnitNumber = ConstantesDisplay.UnitSize, Direction = e.Direction });
		}
	}
}
