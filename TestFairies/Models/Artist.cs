using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFairies.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public int Creativity { get; set; }
        public int Concentration { get; set; }
        public List<Fairy> Fairies { get; set; }

        public Artist(string name, int creativity, int concentration)
        {
            Name = name;
            Creativity = creativity;
            Concentration = concentration;

            Fairies = new List<Fairy>();
        }

        public bool HasLost()
        {
            return Fairies.Where(f => f.Morale > 0).Count() == 0;
        }

        public override string ToString()
        {
            return Name + " Creativity: " + Creativity + " Concentration: " + Concentration;
        }
    }
}
