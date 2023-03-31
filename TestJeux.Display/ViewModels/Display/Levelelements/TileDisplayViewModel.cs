using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using TestJeux.Business.Managers.API;
using TestJeux.Display.Helper;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class TileDisplayViewModel : BindableBase
    {
        private readonly ITileService _tileManager;
        private readonly IImageManager _imageManager;
        public ObservableCollection<TileViewModel> Tiles { get; set; }

        public TileDisplayViewModel(ITileService tileManager, IImageManager imageManager)
        {
            Tiles = new ObservableCollection<TileViewModel>();
            _tileManager = tileManager;
            _imageManager = imageManager;
        }

        public void ReloadTile(int levelId)
        {
            Tiles.Clear();
            foreach (var tile in _tileManager.GetTiles(levelId))
                Tiles.Add(new TileViewModel(tile.SpriteCodes.Select(s => GetImageBitmap(s)).ToList(), tile.Tile, tile.TopLeft.X, tile.TopLeft.Y, tile.Angle));
        }

        private CachedBitmap GetImageBitmap(string code)
        {
            return ImageHelper.GetImage(_imageManager.GetImage(code));
        }
    }
}
