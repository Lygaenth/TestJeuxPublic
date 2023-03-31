using Prism.Mvvm;
using System.Windows.Media.Imaging;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class DecorationViewModel : BindableBase
    {
        private CachedBitmap _sprite;
        public CachedBitmap Sprite
        {
            get => _sprite;
            set => SetProperty(ref _sprite, value);
        }

        public Decorations Decoration { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
