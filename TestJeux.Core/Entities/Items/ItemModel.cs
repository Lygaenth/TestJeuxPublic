using System.Drawing;
using TestJeux.Business.Entities.Stats;
using TestJeux.Business.ObjectValues;
using TestJeux.Business.Entities.Sprites;
using TestJeux.SharedKernel.Enums;
using TestJeux.Core.Entities;
using TestJeux.Core.ObjectValues;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace TestJeux.Core.Entities.Items
{
	public abstract class ItemModel : Entity
    {
        public ItemType ItemType { get; set; }
        public ItemCode Code { get; set; }

		public virtual string Name { get => Code.ToString() + ID; }

		public int Priority { get; set; }
		public int State { get; set; }
		public bool CanFloat { get; set; }
        public bool CanWalk { get; set; }
        public bool IsGround { get; set; }
        public bool CanBePushed { get; set; }
        public bool IsMoving { get; set; }

		public StatsModel Stats { get; set; }
		protected SpriteModel SpriteModel { get; set; }

		public bool HasScript { get; set; }

		protected bool _isSpecialSprite;

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Heigth { get; set; }

		public Point Position
		{
			get => new Point(X, Y);
		}

		protected DirectionEnum _orientation;
		public DirectionEnum Orientation
		{
			get { return _orientation; }
			set
			{
				if (_orientation == value)
					return;
				_orientation = value;
				Refresh();
			}
		}

		public bool IsOccupied { get; set; }

		public List<string> CurrentSprites;
		public string SpeakingSprite { get; set; }


		public event EventHandler<Movement> Moved;
        public event EventHandler<ItemExchange> ItemProvided;
		public event EventHandler<ItemExchange> ItemConsumed;
		public event EventHandler<Speaker> SpeakingRequested;
        public event EventHandler<MakeSound> MadeSound;
		public event EventHandler RequireDestruction;

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="id"></param>
        public ItemModel(int id)
            :base(id)
        {
            Priority = 1;
            SpriteModel = new SpriteModel();
            Stats = new StatsModel();
            Stats.HPMax = -1;
            HasScript = false;
            CurrentSprites = new List<string>();
            SpeakingSprite = "Error";
        }

        public virtual void Initialize()
        {
            LoadSprites();
            if (SpriteModel.Front.Count > 0)
                SpeakingSprite = SpriteModel.Front[0];
            foreach (var sprite in SpriteModel.Front)
                CurrentSprites.Add(sprite);
        }

        /// <summary>
        /// Load sprites
        /// </summary>
        public virtual void LoadSprites()
        {

        }

		#region Interactions
		public abstract List<ActionBase> Interact(ItemModel source, ItemModel target, ActionTarget actionTarget);

        public virtual List<ActionBase> GetSpecificReaction(int id, ItemModel target)
        {
            return new List<ActionBase>();
        }

        protected virtual Reaction GetReactionOnMove(ItemCode itemType, int distance, RelativePosition relativePosition)
        {
            return new Reaction(Reactions.None, 0);
        }
		#endregion

		#region State
		public virtual bool ChangeState(GroundType ground)
        {
            // No generic behaviour
            return false;
        }

        public void Remove()
        {
            if (RequireDestruction != null)
                RequireDestruction(this, new EventArgs());
        }

        public virtual void SetDefaultState(int stateId)
        {
            State = stateId;
        }

        public virtual Task GetTaskScript(ItemModel itemViewModel, CancellationToken cancellationToken)
        {
            return null;
        }

        public MoveType GetMoveType()
        {
            if (CanWalk && CanFloat)
                return MoveType.WalkOrSwim;
            else if (CanWalk)
                return MoveType.Walk;
            else if (CanFloat)
                return MoveType.Swim;
            return MoveType.None;
        }

        public virtual void Refresh()
        {
            var sprites = _orientation switch
            {
                DirectionEnum.Left => GetSprites(SpriteModel.Left),
                DirectionEnum.Right => GetSprites(SpriteModel.Right),
                DirectionEnum.Top => GetSprites(SpriteModel.Back),
                DirectionEnum.Bottom => GetSprites(SpriteModel.Front),
                _ => throw new NotImplementedException()
            };
            CurrentSprites.Clear();
            foreach (var sprite in sprites)
                CurrentSprites.Add(sprite);
        }
		#endregion

		#region Sprites
		public virtual List<string> GetSprites(CreatureState state)
        {
            var sprites = new List<string>();
            switch (state)
            {
                case CreatureState.Idle:
                case CreatureState.Walking:
                    switch (Orientation)
                    {
                        case DirectionEnum.Top:
                            sprites = SpriteModel.Back;
                            break;
                        case DirectionEnum.Left:
                            sprites = SpriteModel.Left;
                            break;
                        case DirectionEnum.Right:
                            sprites = SpriteModel.Right;
                            break;
                        case DirectionEnum.Bottom:
                        default:
                            sprites = SpriteModel.Front;
                            break;
                    }
                    break;
                case CreatureState.Dead:
                default:
                    sprites = SpriteModel.Front;
                    break;
            }
            return GetSprites(sprites);
        }

        protected List<string> GetSprites(List<string> sprites)
        {
            var observableSprites = new List<string>();
            if (sprites != null)
            {
                foreach (var sprite in sprites)
                    observableSprites.Add(sprite);
            }
            return observableSprites;
        }
		#endregion

		/// <summary>
		/// Lock character
		/// </summary>
		/// <param name="model"></param>
		protected void Lock(ItemModel model)
        {
            model.IsOccupied = true;
        }

        /// <summary>
        /// Unlock character
        /// </summary>
        /// <param name="model"></param>
        protected void UnLock(ItemModel model)
        {
            model.IsOccupied = false;
        }

        /// <summary>
        /// Move character
        /// </summary>
        /// <param name="movement"></param>
        public void Move(Movement movement)
        {
			int step = ConstantesDisplay.UnitSize / 10;
			int sens = (movement.Direction == DirectionEnum.Top || movement.Direction == DirectionEnum.Left) ? -1 : +1;
			step *= sens;
			if (movement.Direction == DirectionEnum.Bottom || movement.Direction == DirectionEnum.Top)
				this.Y += sens * ConstantesDisplay.UnitSize;
			else
				this.X += sens * ConstantesDisplay.UnitSize;

            NotifyMove(movement);
        }

        /// <summary>
        /// React to target move
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<Reaction> ReactMove(ItemModel target)
        {
            var reactions = new List<Reaction>();
            int distance = Math.Abs(target.X - X) / 50 + Math.Abs(target.Y - Y) / 50;
            RelativePosition relPos = CheckRelativePosition(target);
            reactions.Add(GetReactionOnMove(target.Code, distance, relPos));
            return reactions;
        }

        /// <summary>
        /// Check relative position
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private RelativePosition CheckRelativePosition(ItemModel target)
        {
            RelativePosition relPos;
            if (CheckIsOnside(X, Y, target.X, target.Y))
                relPos = RelativePosition.Side;
            else
            {
                relPos = Orientation switch
                {
                    DirectionEnum.Left => target.X < X ? RelativePosition.InFront : RelativePosition.Behind,
                    DirectionEnum.Right => target.X > X ? RelativePosition.InFront : RelativePosition.Behind,
                    DirectionEnum.Bottom => target.Y > Y ? RelativePosition.InFront : RelativePosition.Behind,
                    DirectionEnum.Top => target.Y < Y ? RelativePosition.InFront : RelativePosition.Behind,
                    _ => throw new NotImplementedException()
                };
            }
            return relPos;
        }

        private bool CheckIsOnside(int Xs, int Ys, int Xt, int Yt)
        {
            if (Xt != Xs && Yt != Ys)
                return true;
            if (Xt == Xs && (Orientation == DirectionEnum.Left || Orientation == DirectionEnum.Right))
                return true;
            if (Yt == Ys && (Orientation == DirectionEnum.Bottom || Orientation == DirectionEnum.Top))
                return true;
            return false;
        }

		#region Notifications
		/// <summary>
		/// Notify text event
		/// </summary>
		/// <param name="speaker"></param>
		protected void NotifySpeakRequest(Speaker speaker)
		{
			if (SpeakingRequested != null)
				SpeakingRequested(this, speaker);
		}

        /// <summary>
        /// Notify item moved
        /// </summary>
        /// <param name="movement"></param>
		protected void NotifyMove(Movement movement)
        {
            if (Moved != null)
                Moved(ID, movement);
        }

        /// <summary>
        /// Notify item provided
        /// </summary>
        /// <param name="itemExchange"></param>
        protected void NotifyItemProvided(ItemExchange itemExchange)
        {
            if (ItemProvided!=null)
                ItemProvided(this, itemExchange);
        }

        /// <summary>
        /// Notify item consumed 
        /// </summary>
        /// <param name="itemExchange"></param>
		protected void NotifyItemConsumed(ItemExchange itemExchange)
		{
			if (ItemConsumed != null)
				ItemConsumed(this, itemExchange);
		}

        /// <summary>
        /// Notify sound made
        /// </summary>
        /// <param name="sound"></param>
        protected void NotifySoundMade(MakeSound sound)
        {
            if (MadeSound != null)
                MadeSound(this, sound);
        }
		#endregion
	}
}
