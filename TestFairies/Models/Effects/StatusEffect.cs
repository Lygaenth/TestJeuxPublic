using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFairies.Enums;

namespace TestFairies.Models.Effects
{
    public class StatusEffect
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public StatusEffectType EffectType { get; set; }
        public int Duration { get; set; }

        public StatusEffect(string name, StatusEffectType effectType, Action action, int duration)
        {
            Name = name;
            EffectType = effectType;
            Action = action;
            Duration = duration;
        }

        public override string ToString()
        {
            return Name + " "+ EffectType.ToString() + " " + Duration.ToString();
        }
    }
}
