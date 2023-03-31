using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TestJeux.Business.Entities.Stats;

namespace TestJeux.Display.ViewModels.Display.Stats
{
	public class StatsViewModel : BindableBase
    {
        public ObservableCollection<StatViewModel> Stats { get; set; }

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
                SetProperty(ref _currentHp, value);
                Stats.First(s => s.Name == StatHp).Value = value;
            }
        }

        public int CurrentMP
        {
            get { return _currentMp; }
            set
            {
                SetProperty(ref _currentMp, value);
                Stats.First(s => s.Name == StatMp).Value = value;
            }
        }


        public int HPMax
        {
            get { return _hpMax; }
            set
            {
                SetProperty(ref _hpMax, value);
                (Stats.First(s => s.Name == StatHp) as StatWithMaxViewModel).MaxValue = value;
            }
        }

        public int MPMax
        {
            get { return _mpMax; }
            set
            {
                SetProperty(ref _mpMax, value);
                (Stats.First(s => s.Name == StatMp) as StatWithMaxViewModel).MaxValue = value;
            }
        }

        public int Attack
        {
            get { return _attack; }
            set
            {
                SetProperty(ref _attack, value);
                Stats.First(s => s.Name == StatAttack).Value = value;
            }
        }

        public int Defense
        {
            get { return _defense; }
            set
            {
                SetProperty(ref _defense, value);
                Stats.First(s => s.Name == StatDefense).Value = value;
            }
        }

        public int Strength
        {
            get { return _strength; }
            set
            {
                SetProperty(ref _strength, value);
                Stats.First(s => s.Name == StatStrength).Value = value;
            }
        }

        public int Magic
        {
            get { return _magic; }
            set
            {
                SetProperty(ref _magic, value);
                Stats.First(s => s.Name == StatMagic).Value = value;
            }
        }

        public int Move
        {
            get { return _move; }
            set
            {
                SetProperty(ref _move, value);
            }
        }

        public int Sanity
        {
            get { return _sanity; }
            set
            {
                SetProperty(ref _sanity, value);
                Stats.First(s => s.Name == StatSanity).Value = value;
            }
        }

        public int MaxSanity
        {
            get { return _maxSanity; }
            set
            {
                SetProperty(ref _maxSanity, value);
                Stats.First(s => s.Name == StatSanity).Value = value;
            }
        }


        public StatsViewModel(StatsModel stats)
        {
            Stats = new ObservableCollection<StatViewModel>();
            Stats.Add(new StatWithMaxViewModel(StatHp, stats.CurrentHP, stats.HPMax));
            Stats.Add(new StatWithMaxViewModel(StatMp, stats.CurrentHP, stats.MPMax));
            Stats.Add(new StatViewModel(StatStrength, stats.Strength));
            Stats.Add(new StatViewModel(StatAttack, stats.Attack));
            Stats.Add(new StatViewModel(StatDefense, stats.Defense));
            Stats.Add(new StatViewModel(StatMagic, stats.Magic));
            //Stats.Add(new StatWithMaxViewModel(StatSanity, stats.Sanity, stats.SanityMax));

            HPMax = stats.HPMax;
            MPMax = stats.MPMax;
            Attack = stats.Attack;
            Defense = stats.Defense;
            Strength = stats.Strength;
            Magic = stats.Magic;
            Move = stats.Move;
            CurrentHP = stats.HPMax;
            CurrentMP = stats.MPMax;

            //Sanity = stats.Sanity;
            //MaxSanity = stats.SanityMax;
        }
    }
}
