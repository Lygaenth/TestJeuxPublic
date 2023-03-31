using System;
using System.Collections.Generic;
using TestFairies.Enums;
using TestFairies.Models.Effects;

namespace TestFairies.Models
{
    public class Fairy
    {
        private ElementType _type;
        private string _name;
        public int _creativity;
        public int _maxCreativity;
        public int _morale;
        public int _maxMorale;

        public List<StatusEffect> StatusEffects { get; set; }

        public Fairy(string name, ElementType elementType, int maxCreativity, int maxMorale)
        {
            _name = name;
            _type = elementType;
            _creativity = maxCreativity;
            _maxCreativity = maxCreativity;
            _morale = maxMorale;
            _maxMorale = maxMorale;

            Moves = new List<Move>();
            StatusEffects = new List<StatusEffect>();
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Creativity { get => _creativity; set => _creativity = value; }

        public ElementType Type { get => _type; }

        public int Morale { get => _morale; set => _morale = value; }

        public override string ToString()
        {
            return _name + " (" + _type.ToString() + ") Creativity: " + _creativity + " / " + _maxCreativity + " Morale: " + _morale + "/" + _maxMorale;
        }

        public List<Move> Moves { get; set; }

        public string GetMovesOptions()
        {
            string moveDisplays = "";
            for (int i = 0; i < Moves.Count; i++)
                moveDisplays += i + 1 + ": " + Moves[i].Name + " ";

            return moveDisplays;
        }

        public void DamageMorale(int amount)
        {
            if (amount > 0)
                _morale = Math.Min(_morale + amount, _maxMorale);

            if (amount < 0)
                _morale = Math.Max(_morale + amount,0);
        }
    }
}
