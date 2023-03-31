using System;
using System.Linq;
using TestFairies.Enums;

namespace TestFairies.Models.Effects
{
    public class BuffEffect : Effect
    {
        private Characteristic _characteristic;
        private string _buffName;
        private StatusEffectType _effectType;

        public BuffEffect(string name, TargetType targetType, Characteristic characteristic, StatusEffectType effectType, int potency, int duration, string description)
        : base(EffectType.Buff, targetType, potency, duration)
        {
            _buffName = name;
            _characteristic = characteristic;
            _effectType = effectType;
            Description = description;
        }

        public void Execute(Fairy fairy)
        {
            // Attack buff
            if (_characteristic == Characteristic.Creativity)
            {
                fairy.Creativity = fairy.Creativity * Potency / 100;
                if (fairy.StatusEffects.Any(s => s.Name == _buffName))
                    fairy.StatusEffects.Find(s => s.Name == _buffName).Duration = Duration;
                else
                    fairy.StatusEffects.Add(new StatusEffect(_buffName, StatusEffectType.AppliedForSetDuration, () => { fairy.Creativity = fairy.Creativity * 100 / Potency; }, Duration));                
            }
            else if (_characteristic == Characteristic.Morale)
            {
                fairy.StatusEffects.Add(new StatusEffect(_buffName, StatusEffectType.EffectOnTime, () => { fairy.Morale -= Math.Max(1,fairy.Morale * Potency /100); }, Duration));
            }
        }
    }
}
