using System;
using System.Collections.Generic;
using System.Text;
using TestJeux.Display.ViewModels.Base;

namespace TestJeux.Display.ViewModels.Display.Stats
{
    public class StatViewModel : BaseViewModel
    {
        private string _name;
        private int _value;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int Value { get => _value; set => SetProperty(ref _value, value); }

        public StatViewModel(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
