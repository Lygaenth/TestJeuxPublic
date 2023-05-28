using Prism.Mvvm;
using System.Windows.Media.Imaging;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class DecorationViewModel : BindableBase
    {
        private int _id;
        public int ID { get => _id; }

        public DecorationViewModel(int id)
        {
            _id = id;
        }

        private CachedBitmap _sprite;
        public CachedBitmap Sprite
        {
            get => _sprite;
            set => SetProperty(ref _sprite, value);
        }

        public Decorations Decoration { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Angle { get;set; }

    }
}
