namespace TestJeux.Display.ViewModels.Display.Stats
{
    public class StatWithMaxViewModel : StatViewModel
    {
        private int _maxvalue;
        public int MaxValue { get => _maxvalue; set => SetProperty(ref _maxvalue, value); }

        public StatWithMaxViewModel(string name, int value, int max)
            : base(name, value)
        {
            MaxValue = max;
        }
    }
}
