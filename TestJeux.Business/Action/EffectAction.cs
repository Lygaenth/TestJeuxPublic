using System.Collections.Generic;
using TestJeux.Business.Entities.Items;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class EffectAction : ActionBase
    {
        public override ActionType ActionType { get => ActionType.Effect; }

        public ItemModel Target { get; set; }
        public override bool IsBlocking { get => false; }

        private string _statName = "";
        public int Modifier { get; private set; }

        List<string> Animation { get; set; }

        public EffectAction(ItemModel source, ItemModel target, string propertyName, int modifier)
        {
            _statName = propertyName;
            Source = source;
            Target = target;

            Modifier = modifier;
            IsCompleted = false;
        }

        public override bool Execute()
        {
            var currentValue = (int)Target.Stats.GetType().GetProperty(_statName).GetValue(Target.Stats);
            currentValue += Modifier;
            Target.Stats.GetType().GetProperty(_statName).SetValue(Target.Stats, currentValue);
            IsCompleted = true;
            return true;
        }

        public override bool Acq()
        {
            return false;
        }
    }
}
