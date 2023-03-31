using TestFairies.Enums;

namespace TestFairies.Models.Effects
{
    public class Effect
    {
        public EffectType EffectType { get; private set; }
        public TargetType TargetType { get; private set; }
        public int Potency { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; private set; }

        public Effect(EffectType effect, TargetType targetType, int potency, int duration)
        {
            EffectType = effect;
            TargetType = targetType;
            Potency = potency;
            Duration = duration;
        }
    }
}
