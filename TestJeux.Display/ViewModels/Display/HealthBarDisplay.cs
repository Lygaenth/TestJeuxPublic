using Prism.Mvvm;

namespace TestJeux.Display.ViewModel.Display
{
	public class HealthBarDisplay : BindableBase
    {
        bool _visibility;
        int _X1;
        int _Y1;
        int _X2;
        int _Y2;
        int _currentX2;

        public HealthBarDisplay(int width)
        {
            Width = width;
            X1 = 0;
            X2 = 40 * width;
            Y1 = 0;
            Y2 = 0;
        }

        public bool Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        public int Width { get; set; }

        public int X1
        {
            get { return _X1; }
            set { SetProperty(ref _X1, value); }
        }

        public int Y1
        {
            get { return _Y1; }
            set { SetProperty(ref _Y1, value); }
        }

        public int X2
        {
            get { return _X2; }
            set { SetProperty(ref _X2, value); }
        }

        public int Y2
        {
            get { return _Y2; }
            set { SetProperty(ref _Y2, value); }
        }

        public int CurrentX2
        {
            get { return _currentX2; }
            set { SetProperty(ref _currentX2, value); }
        }

        public void UpdateHpRatio(int ratio)
        {
            CurrentX2 = X2 * ratio / 100;
        }
    }
}
