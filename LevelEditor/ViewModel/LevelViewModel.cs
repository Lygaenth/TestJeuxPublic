using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Supervisor;
using TestJeux.Display.Helper;
using TestJeux.Display.ViewModels.Display.Levelelements;
using TestJeux.SharedKernel.Enums;
using TestJeux.Business.Services.API;

namespace LevelEditor.wpf.ViewModel
{
	public class LevelViewModel
	{
		private readonly ILevelService _levelService;
		private readonly ITileService _tileManager;

		public int ID { get; private set; }

		public ObservableCollection<ZoneViewModel> Zones { get; set; }
		public ObservableCollection<TileViewModel> Tiles { get; set; }
		public ObservableCollection<ItemViewModel> Items { get; set; }
		public ObservableCollection<DecorationViewModel> Decorations { get; set; }

		public ShaderType Shader { get; set; }
		public Musics Music { get; set; }


		public LevelViewModel(ILevelService levelService, ITileService tileManager)
		{
			_levelService = levelService;
			_tileManager = tileManager;

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
				Tiles.Add(new TileViewModel(tile.ID, tile.SpriteCodes.Select(s => GetImageBitmap(s)).ToList(), tile.Tile, tile.TopLeft.X, tile.TopLeft.Y, tile.Angle));
			}

			Zones.Clear();
			foreach (var zone in level.Zones)
				Zones.Add(new ZoneViewModel(zone.ID, zone.TopLeft, zone.BottomRight.X - zone.TopLeft.X, zone.BottomRight.Y - zone.TopLeft.Y, zone.GroundType));

			Decorations.Clear();
			foreach (var decoration in level.Decorations)
				Decorations.Add(new DecorationViewModel(decoration.ID) { Decoration = decoration.Decoration, Sprite = GetImageBitmap(decoration.Decoration.ToString()), X = decoration.TopLeft.X, Y = decoration.TopLeft.Y, Angle = decoration.Angle });

			Items.Clear();
			foreach(var item in level.Items)
				Items.Add(new ItemViewModel(item.ID) { Code = item.Code, DefaultState = item.DefaultState, Orientation = item.Orientation, StartPosition = item.StartPosition });

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
			Items.Remove(Items.FirstOrDefault(i => i.ID == id));
		}

		private CachedBitmap GetImageBitmap(string code)
		{
			return ImageHelper.GetImage(code);
		}
	}
}
