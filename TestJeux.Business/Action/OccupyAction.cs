using TestJeux.Business.Entities.Items;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class OccupyAction : ActionBase
    {
        private readonly bool _block;
        private readonly ItemModel _target;

        public override ActionType ActionType { get => ActionType.Lock; }

        public OccupyAction(ItemModel target, bool block)
        {
            _target = target;
            _block = block;
        }

        public override bool Acq()
        {
            return false;
        }

        public override bool Execute()
        {
            _target.IsOccupied = _block;
            IsCompleted = true;
            return false;
        }
    }
}
