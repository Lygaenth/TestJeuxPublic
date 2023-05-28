using System.Collections.Generic;
using System.Drawing;
using TestJeux.API.Models;
using TestJeux.Data.EFCore.DbItems;

namespace TestJeux.Data.EFCore.Helper
{
	public static class Converters
	{
		public static LevelDto Convert(this Level level)
		{
			var levelDto = new LevelDto();
			levelDto.ID = level.LevelId;
			
			levelDto.Shader = level.Shader;
			levelDto.LevelMusic = level.Music;

			levelDto.TilesZones = new List<TileZoneDto>();
			foreach (var tile in level.Tiles)
					levelDto.TilesZones.Add(tile.Convert());

			levelDto.Decorations = new List<DecorationDto>();
			foreach (var decoration in level.Decorations)
				levelDto.Decorations.Add(decoration.Convert());

			levelDto.Zones = new List<ZoneDto>();
			foreach (var zone in level.Zones)
				levelDto.Zones.Add(zone.Convert());

			return levelDto;
		}

		/// <summary>
		/// Convert from Zone to Zone DTO
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		public static ZoneDto Convert(this Zone zone)
		{
			var zoneDto = new ZoneDto();
			zoneDto.ID = zone.ZoneId;
			zoneDto.GroundType = zone.GroundType;
			zoneDto.TopLeft = new Point(zone.X1, zone.Y1);
			zoneDto.BottomRight = new Point(zone.X2, zone.Y2);
			return zoneDto;
		}

		/// <summary>
		/// Convert from Zone DTO to zone
		/// </summary>
		/// <param name="zoneDto"></param>
		/// <returns></returns>
		public static Zone Convert(this ZoneDto zoneDto)
		{
			var zone = new Zone();
			zone.ZoneId = zoneDto.ID;
			zone.GroundType = zoneDto.GroundType;
			zone.X1 = zoneDto.TopLeft.X;
			zone.Y1 = zoneDto.TopLeft.Y;
			zone.X2 = zoneDto.BottomRight.X;
			zone.Y2 = zoneDto.BottomRight.Y;
			return zone;
		}

		/// <summary>
		/// Convert Tile to DTO
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		public static TileZoneDto Convert(this Tile tile)
		{
			var tileZoneDto = new TileZoneDto();
			tileZoneDto.Tile = tile.Type;
			tileZoneDto.Angle = tile.Angle / 90;
			tileZoneDto.TopLeft = new Point(tile.X1, tile.Y1);
			tileZoneDto.BottomRight = new Point(tile.X2, tile.Y2);
			return tileZoneDto;
		}

		/// <summary>
		/// Convert DTO to tile
		/// </summary>
		/// <param name="tileDto"></param>
		/// <returns></returns>
		public static Tile Convert(this TileZoneDto tileDto)
		{
			var tile = new Tile();
			tile.Type = tileDto.Tile;
			tile.Angle = tileDto.Angle * 90;
			tile.X1 = tileDto.TopLeft.X;
			tile.Y1 = tileDto.TopLeft.Y;
			tile.X2 = tileDto.BottomRight.X;
			tile.Y2 = tileDto.BottomRight.Y;
			return tile;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="decoration"></param>
		/// <returns></returns>
		public static DecorationDto Convert(this Decoration decoration)
		{
			var decorationDto = new DecorationDto();
			decorationDto.Decoration = decoration.Type;
			decorationDto.Angle = 0;
			decorationDto.TopLeft = new Point(decoration.X, decoration.Y);
			return decorationDto;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="decorationDto"></param>
		/// <returns></returns>
		public static Decoration Convert(this DecorationDto decorationDto)
		{
			var decoration = new Decoration();
			decoration.Type = decorationDto.Decoration;
			decoration.X = decorationDto.TopLeft.X;
			decoration.Y = decorationDto.TopLeft.Y;
			return decoration;
		}
	}
}
