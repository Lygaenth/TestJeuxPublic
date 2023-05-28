using System.Collections.Generic;
using System.Drawing;
using TestJeux.API.Models;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	/// <summary>
	/// Tile service
	/// </summary>
	public class TileService : ITileService
	{
		private readonly GameAggregateBase _game;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="game"></param>
		public TileService(GameAggregateBase game)
		{
			_game = game;
		}

		/// <summary>
		/// Return tiles
		/// </summary>
		/// <param name="levelID"></param>
		/// <returns></returns>
		public List<TileZoneDto> GetTiles(int levelID)
		{
			if (_game.GetCurrentLevel() == null)
				return new List<TileZoneDto>();

			var level = _game.GetLevel(levelID);
			var tiles = GenerateTiles(level.DefaultTile, level.Tiles);
			return tiles;
		}

		private List<TileZoneDto> GenerateTiles(GroundSprite defaultTile, List<TileZone> tiles)
		{
			var tilesDtos = new List<TileZoneDto>();

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					var x = i * 50;
					var y = j * 50;
					var foundTile = GetTileForPosition(defaultTile, x, y, tiles);
					TileZoneDto tile = null; 
					if (foundTile.BottomRight.X - foundTile.TopLeft.X == 50 && (foundTile.BottomRight.Y - foundTile.TopLeft.Y == 50))
						tile = new TileZoneDto() { ID = foundTile.ID, Tile = foundTile.Enumvalue, TopLeft = new Point(x, y), BottomRight = new Point(x + 50, y + 50), Angle = foundTile.Angle, SpriteCodes = GetTileSprites(foundTile.Enumvalue) };
					else
						tile = new TileZoneDto() { ID = 0, Tile = foundTile.Enumvalue, TopLeft = new Point(x, y), BottomRight = new Point(x + 50, y + 50), Angle = foundTile.Angle, SpriteCodes = GetTileSprites(foundTile.Enumvalue) };
					tilesDtos.Add(tile);
				}
			}

			return tilesDtos;
		}

		private TileZone GetTileForPosition(GroundSprite defaultTile, int X, int Y, List<TileZone> zones)
		{
			foreach (var zone in zones)
			{
				if (X < zone.TopLeft.X || X >= zone.BottomRight.X)
					continue;
				if (Y < zone.TopLeft.Y || Y >= zone.BottomRight.Y)
					continue;
				return zone;
			}
			var tile = TileZone.None;
			tile.Enumvalue = defaultTile;
			return tile;
		}

		public List<string> GetTileSprites(GroundSprite groundSprite)
		{
			return groundSprite switch
			{
				GroundSprite.Grass => new List<string> { "Grass1" },
				GroundSprite.Water => new List<string> { "Water1" },
				GroundSprite.WaterSide => new List<string> { "WaterSide", "WaterSide2" },
				GroundSprite.CaveFloor => new List<string> { "CaveFloor" },
				GroundSprite.CaveWall => new List<string> { "CaveWall" },
				GroundSprite.CaveWallOneSide => new List<string> { "CaveWallOneSide" },
				GroundSprite.CaveWallOpposedSide => new List<string> { "CaveWallOpposedSide" },
				GroundSprite.CaveWallOneCorner => new List<string> { "CaveWallOneCorner" },
				_ => new List<string> { "Error" }
			};
		}
	
		public List<TileZoneDto> Convert(List<TileZone> tiles)
		{
			var dtos = new List<TileZoneDto>();
			foreach (var tile in tiles)
				dtos.Add(new TileZoneDto() { Tile = tile.Enumvalue, TopLeft = tile.TopLeft, BottomRight = tile.BottomRight, Angle = tile.Angle });
			return dtos;
		}
	}
}
