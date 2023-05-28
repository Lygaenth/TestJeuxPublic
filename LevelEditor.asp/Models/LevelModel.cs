using TestJeux.API.Models;
using TestJeux.Business.Managers.API;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
	public class LevelModel
	{
		private const int DefaultTileSize = 16;

		public int ID { get; }

		private TileModel _selectedSprite;
		public TileModel SelectedSprite
		{
			get => _selectedSprite;
			set
			{
				_selectedSprite = value;
				UpdateSelectedSpriteImage();
			}
		}

		public TileModel[][] Tiles { get; set; }

		public List<DecorationModel> Decorations { get; set; }

		public List<Item> Items { get; set; }

		public List<Zone> Zones { get; set; }

		public ShaderType Shader { get; set; }

		public Musics LevelMusic { get; set; }

		public bool IsEdited { get; set; }

		IImageManager _imageService;

		public LevelModel(int id, IImageManager imageService)
		{
			ID = id;
			_imageService = imageService;

			Items = new List<Item>();
			Zones = new List<Zone>();

			Tiles = new TileModel[DefaultTileSize][];
			for (int i = 0; i < DefaultTileSize; i++)
				Tiles[i] = new TileModel[DefaultTileSize];
			Decorations = new List<DecorationModel>();
		}

		private void UpdateSelectedSpriteImage()
		{
			if (SelectedSprite != null)
				SelectedSprite.Image = _imageService.GetImage(SelectedSprite.GroundType.ToString());
		}

		public void UpdateTiles(GroundSprite defaultTile, List<TileZoneDto> tilesDto)
		{
			var orderedTiles = GenerateTiles(defaultTile, tilesDto);
			foreach (var tile in orderedTiles)
				Tiles[tile.TopLeft.X / 50][tile.TopLeft.Y / 50] = new TileModel(_imageService) { X = tile.TopLeft.X, Y = tile.TopLeft.Y, Image = _imageService.GetImage(tile.Tile.ToString()), Angle = tile.Angle, GroundType = tile.Tile };

		}

		private List<TileZoneDto> GenerateTiles(GroundSprite defaultTile, List<TileZoneDto> tilesDto)
		{
			var tiles = new List<TileZoneDto>();

			for (int j = 0; j < 16; j++)
			{
				for (int i = 0; i < 16; i++)
				{
					var x = i * 50;
					var y = j * 50;
					var tileDto = GetTileForPosition(defaultTile, x, y, tilesDto);
					var newTile = new TileZoneDto
					{
						Tile = tileDto.Tile,
						TopLeft = new System.Drawing.Point(x, y),
						BottomRight = new System.Drawing.Point(x + 50, y + 50),
						Angle = tileDto.Angle
					};
					tiles.Add(newTile);
				}
			}

			return tiles;
		}

		private TileZoneDto GetTileForPosition(GroundSprite defaultTile, int X, int Y, List<TileZoneDto> zones)
		{
			foreach (var zone in zones)
			{
				if (X < zone.TopLeft.X || X >= zone.BottomRight.X)
					continue;
				if (Y < zone.TopLeft.Y || Y >= zone.BottomRight.Y)
					continue;
				return zone;
			}
			return new TileZoneDto() { Tile = defaultTile };
		}
	}
}
