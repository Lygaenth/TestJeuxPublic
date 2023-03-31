using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestJeux.Business.Entities.Stats
{
    public class StatsModel
    {
        public ObservableCollection<StatModel> Stats { get; set; }

        private const string StatHp = "HP";
        private const string StatMp = "MP";
        private const string StatSanity = "Sanity";
        private const string StatStrength = "Strength";
        private const string StatAttack = "Attack";
        private const string StatDefense = "Defense";
        private const string StatMagic = "Magic";
        private const string StatMove = "Move";

        private int _currentHp;
        private int _currentMp;
        private int _hpMax;
        private int _mpMax;
        private int _strength;
        private int _attack;
        private int _defense;
        private int _magic;
        private int _move;
        private int _sanity;
        private int _maxSanity;

        public event EventHandler StatChanged;

        public int CurrentHP
        {
            get { return _currentHp; }
            set
            {
                if (value < 0)
                    value = 0;
                _currentHp = value;
                Stats.First(s => s.Name == StatHp).Value = value;
            }
        }

        public int CurrentMP
        {
            get { return _currentMp; }
            set
            {
                _currentMp = value;
                Stats.First(s => s.Name == StatMp).Value = value;
            }
        }


        public int HPMax
        {
            get { return _hpMax; }
            set
            {
                _hpMax = value;
                (Stats.First(s => s.Name == StatHp) as StatwithMaxModel).MaxValue = value;
            }
        }

        public int MPMax
        {
            get { return _mpMax; }
            set
            {
                _mpMax = value;
                (Stats.First(s => s.Name == StatMp) as StatwithMaxModel).MaxValue = value;
            }
        }

        public int Attack
        {
            get { return _attack; }
            set
            {
                _attack = value;
                Stats.First(s => s.Name == StatAttack).Value = value;
            }
        }

        public int Defense
        {
            get { return _defense; }
            set
            {
                _defense = value;
                Stats.First(s => s.Name == StatDefense).Value = value;
            }
        }

        public int Strength
        {
            get { return _strength; }
            set
            {
                _strength = value;
                Stats.First(s => s.Name == StatStrength).Value = value;
            }
        }

        public int Magic
        {
            get { return _magic; }
            set
            {
                _magic = value;
                Stats.First(s => s.Name == StatMagic).Value = value;
            }
        }

        public int Move
        {
            get => _move;
            set => _move = value;
        }

        public int Sanity
        {
            get { return _sanity; }
            set
            {
                _sanity = value;
                Stats.First(s => s.Name == StatSanity).Value = value;
            }
        }

        public int MaxSanity
        {
            get { return _maxSanity; }
            set
            {
                _maxSanity = value;
                Stats.First(s => s.Name == StatSanity).Value = value;
            }
        }

        public int StatCount
        {
            get => Stats.Count;
        }

        public StatsModel()
        {
            Stats = new ObservableCollection<StatModel>
            {
                new StatwithMaxModel(StatHp, 0, 0),
                new StatwithMaxModel(StatMp, 0, 0),
                new StatModel(StatStrength, 0),
                new StatModel(StatAttack, 0),
                new StatModel(StatDefense, 0),
                new StatModel(StatMagic, 0),
                new StatwithMaxModel(StatSanity, 0, 0)
            };
        }
    }
}
