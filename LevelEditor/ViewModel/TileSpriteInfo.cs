using Prism.Mvvm;
using System.Windows.Media.Imaging;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.wpf.ViewModel
{
	public class TileSpriteInfo : BindableBase
    {
        public TileSpriteInfo(GroundSprite code, CachedBitmap bitmap)
        {
            SpriteCode = code;
            Sprite = bitmap;
        }

        private CachedBitmap _sprite;
        public CachedBitmap Sprite
        {
            get => _sprite;
            set => SetProperty(ref _sprite, value);
        }

        private GroundSprite _code;
        public GroundSprite SpriteCode
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string _name;
        public string Name
        {
            get => _code.ToString();
        }
    }
}
