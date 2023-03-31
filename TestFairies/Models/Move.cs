using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFairies.Models.Effects;

namespace TestFairies.Models
{
    public class Move
    {
        public string Name { get; set; }
        public List<Effect> Effects { get; set; }

        public Move(string name, List<Effect> effects)
        {
            Name = name;
            Effects = effects;
        }
    }
}
