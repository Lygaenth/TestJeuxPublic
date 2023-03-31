using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Services.API;
using TestJeux.Business.Supervisor;
using TestJeux.Display.Helper;
using TestJeux.Display.ViewModels.Base;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class LevelViewModel : BaseViewModel
    {
        private readonly ILevelService _levelService;
        private readonly ITileService _tileManager;
        private readonly ICharacterManager _characterManager;
        private readonly ICharacterBuilder _characterBuilder;

        public int ID { get; private set; }

        public ObservableCollection<ZoneViewModel> Zones { get; set; }
        public ObservableCollection<TileViewModel> Tiles { get; set; }
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public ObservableCollection<DecorationViewModel> Decorations { get; set; }

        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

        public ShaderType Shader { get; set; }
        public Musics Music { get; set; }


        public LevelViewModel(ILevelService levelService, ITileService tileManager, ICharacterManager characterManager, ICharacterBuilder characterBuilder)
        {
            _levelService = levelService;
            _tileManager = tileManager;
            _characterManager = characterManager;
            _characterBuilder = characterBuilder;

            Tiles = new ObservableCollection<TileViewModel>();
            Zones = new ObservableCollection<ZoneViewModel>();
            Items = new ObservableCollection<ItemViewModel>();
            Decorations = new ObservableCollection<DecorationViewModel>();
            _tileManager = tileManager;
        }

        public void LoadLevel(int id)
        {
            ID = id;
            var level = _levelService.GetLevel(id);
            Shader = level.Shader;
            Music = level.LevelMusic;

            foreach (var tile in _tileManager.GetTiles(level.ID))
            {
                tile.SpriteCodes = _tileManager.GetTileSprites(tile.Tile);
                Tiles.Add(new TileViewModel(tile.SpriteCodes.Select(s => GetImageBitmap(s)).ToList(), tile.Tile, tile.TopLeft.X, tile.TopLeft.Y, tile.Angle));
            }

            Zones.Clear();
            foreach (var zone in level.Zones)
                Zones.Add(new ZoneViewModel(zone.ID, zone.TopLeft, zone.BottomRight.X - zone.TopLeft.X, zone.BottomRight.Y - zone.TopLeft.Y, zone.GroundType));

            Decorations.Clear();
            foreach (var decoration in level.Decorations)
                Decorations.Add(new DecorationViewModel() { Decoration = decoration.Decoration, Sprite = GetImageBitmap(decoration.Decoration.ToString()), X = decoration.TopLeft.X, Y = decoration.TopLeft.Y });

            SelectedItem = null;
            Items.Clear();

            foreach (var itemId in level.ItemsIDs)
            {
                var itemVm = _characterBuilder.CreateItem(_characterManager.GetCharacter(itemId));
                itemVm.RefreshSprite();
                Items.Add(itemVm);
                if (itemVm.IsSelected)
                    SelectedItem = itemVm;
            }
        }

        public void Reset()
        {
            Tiles.Clear();
            Zones.Clear();
            Decorations.Clear();
            Items.Clear();
        }

        public void RemoveItem(int id)
        {
            foreach (var item in Items.Where(i => i.ID == id))
                Items.Remove(item);
        }
        private CachedBitmap GetImageBitmap(string code)
        {
            return ImageHelper.GetImage(code);
        }

    }
}
