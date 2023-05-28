using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class TileViewModel : BindableBase
    {
        public int ID { get; }
        private int _spriteIndex = 0;
        private DateTime _lastRefreshTime = DateTime.Now;

        public string Position { get => "Position: (" + X + "," + Y + ")"; }

        public TileViewModel(int id, List<CachedBitmap> sprites, GroundSprite groundSprite, int x, int y, int angle)
        {
            ID = id;
            _lastRefreshTime = DateTime.Now;
            _sprites = sprites;
            Sprite = _sprites[0];
            GroundSprite = groundSprite;
            X = x;
            Y = y;
            Angle = angle;
        }

        private List<CachedBitmap> _sprites { get; set; }

        private CachedBitmap _sprite;
        public CachedBitmap Sprite
        {
            get => _sprite;
            set => SetProperty(ref _sprite, value);
        }

        private GroundSprite _groundSprite;
        public GroundSprite GroundSprite
        {
            get => _groundSprite;
            set => SetProperty(ref _groundSprite, value);
        }

        public int X { get; set; }
        public int Y { get; set; }

        private int _angle;
        public int Angle
        {
            get => _angle;
            set => SetProperty(ref _angle, value);
        }

        public bool CanRefresh()
        {
            return _sprites.Count > 1;
        }

        public void UpdateSprites(List<CachedBitmap> bitmaps)
        {
            _sprites = bitmaps;
            Sprite = _sprites[0];
        }

        public void Refresh()
        {
            if (_sprites.Count == 1)
                return;

            var refreshTime = DateTime.Now;
            if (1000 / _sprites.Count < (refreshTime - _lastRefreshTime).TotalMilliseconds)
            {
                _spriteIndex = (_spriteIndex + 1) % _sprites.Count;
                Sprite = _sprites[_spriteIndex];
                _lastRefreshTime = refreshTime;
            }
        }
    }
}
