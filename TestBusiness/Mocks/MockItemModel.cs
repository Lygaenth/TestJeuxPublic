using System.Collections.Generic;
using TestJeux.Business.Action;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.ObjectValues;
using TestJeux.Core.Entities;
using TestJeux.Core.ObjectValues;
using TestJeux.SharedKernel.Enums;

namespace TestBusiness.Mocks
{
	public class MockItemModel : ItemModel
    {
        public bool WasRequestedReaction { get; set; }

        public MockItemModel(int id)
            : base(id)
        {
        }

        public override List<ActionBase> Interact(ItemModel source, ItemModel target, ActionTarget actionTarget)
        {
            return new List<ActionBase>();
        }

        /// <summary>
        /// Mock react, has specific reaction when distance is 1
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="distance"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        protected override Reaction GetReactionOnMove(ItemCode itemType, int distance, RelativePosition relativePosition)
        {
            WasRequestedReaction = true;
            if (distance == 1)
                return new Reaction(Reactions.Specific, 2);
            return new Reaction(Reactions.None, 0);
        }

        public override List<ActionBase> GetSpecificReaction(int id, ItemModel target)
        {
            if (target is MockItemModel && id == 2)
                return new List<ActionBase>() { new EffectAction(this, target, "MOCK", 2) };

            return new List<ActionBase>();
        }
    }
}
