using Prism.Mvvm;
using System.Drawing;
using System.Windows.Media;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
    public class ZoneViewModel : BindableBase
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private int _id;
        private GroundType _groundType;
        private string _label;

        private Brush _brush;
        public Brush Color { get => _brush; set => SetProperty(ref _brush, value); }

        public int ID { get => _id; }

        public string Label { get => _label; set => SetProperty(ref _label, value); }

        public int X
        {
            get => _x;
            set
            {
                SetProperty(ref _x, value);
                RaisePropertyChanged(nameof(Dimensions));
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                SetProperty(ref _y, value);
                RaisePropertyChanged(nameof(Dimensions));
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                SetProperty(ref _width, value);
                RaisePropertyChanged(nameof(Dimensions));
            }
        }
        public int Heigth
        {
            get => _height;
            set
            {
                SetProperty(ref _height, value);
                RaisePropertyChanged(nameof(Dimensions));
            }
        }

        public GroundType GroundType
        {
            get => _groundType;
            set
            {
                SetProperty(ref _groundType, value);
                Update();
            }
        }

        public string Dimensions { get => "(" + X + "," + Y + ") " + Width + "x" + Heigth; }

        public ZoneViewModel(int id, Point point, int width, int heigth, GroundType groundType)
        {
            _id = id;
            X = point.X;
            Y = point.Y;
            Width = width;
            Heigth = heigth;
            GroundType = groundType;
        }

        private void Update()
        {
            Label = _id + " : " + _groundType.ToString();
            switch (_groundType)
            {
                case GroundType.Block:
                    Color = Brushes.Red;
                    break;
                case GroundType.Water:
                    Color = Brushes.Azure;
                    break;
                case GroundType.GoUp:
                    Color = Brushes.Violet;
                    break;
                case GroundType.GoDown:
                    Color = Brushes.Orange;
                    break;
                case GroundType.OutOfBound:
                    Color = Brushes.Black;
                    break;
            }
        }
    }
}
