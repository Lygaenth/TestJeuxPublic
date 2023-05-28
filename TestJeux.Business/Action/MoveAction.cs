using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class MoveAction : ActionBase
    {
        private readonly ActionType _actionType;

        public override ActionType ActionType { get => _actionType; }

        public GroundType GroundType { get; set; }

        public override bool IsBlocking => true;

        public DirectionEnum Direction { get; set; }

        private readonly IMoveService _moveManager;

        bool _withCheck;

        public MoveAction(ActionType actionType, IMoveService moveManager, ItemModel source, GroundType groundType, bool withCheck)
        {
            _actionType = actionType;
            Source = source;
            Direction = source.Orientation;
            GroundType = groundType;
            _moveManager = moveManager;
            _withCheck = withCheck;
        }

        public override bool Execute()
        {
            Source.IsMoving = true;
            if (_withCheck && _moveManager.IsPositionOccupied(_moveManager.GetTargetPosition(Source.Position, Direction)) > 0)
            {
                IsCompleted = true;
                return true;
            }

            Source.ChangeState(GroundType);
            _moveManager.MoveCharacter(Direction, Source.ID);
            IsCompleted = true;
            return false;
        }

        public override bool Acq()
        {
            return true;
        }
    }
}
