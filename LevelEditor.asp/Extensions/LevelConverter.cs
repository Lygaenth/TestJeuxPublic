using LevelEditor.asp.Models;
using System.Drawing;
using TestJeux.API.Models;

namespace LevelEditor.asp.Extensions
{
	/// <summary>
	/// Converters for model to DTOs
	/// </summary>
	public static class DtoConverter
	{
		/// <summary>
		/// Convert level to DTO
		/// </summary>
		/// <param name="levelModel"></param>
		/// <returns></returns>
		public static LevelDto Convert(this LevelModel levelModel)
		{
			var levelDto = new LevelDto();
			levelDto.ID = levelModel.ID;
			levelDto.Shader = levelModel.Shader;
			levelDto.LevelMusic = levelModel.LevelMusic;

			// Converting tile zones
			levelDto.TilesZones = new List<TileZoneDto>();
			foreach (var tileRow in levelModel.Tiles)
			{
				foreach (var tile in tileRow)
					levelDto.TilesZones.Add(tile.Convert());
			}

			// Converting decorations
			levelDto.Decorations = new List<DecorationDto>();
			foreach(var decoration in levelModel.Decorations)
				levelDto.Decorations.Add(decoration.Convert());

			// Converting Items
			levelDto.Items = new List<ItemDto>();
			foreach (var item in levelModel.Items)
				levelDto.Items.Add(item.Convert());

			// Converting Zones
			levelDto.Zones = new List<ZoneDto>();
			foreach (var zone in levelModel.Zones)
				levelDto.Zones.Add(zone.Convert());

			return levelDto;
		}

		/// <summary>
		/// Convert Tile to Dto
		/// </summary>
		/// <param name="tileModel"></param>
		/// <returns></returns>
		public static TileZoneDto Convert(this TileModel tileModel)
		{
			var tileZoneDto = new TileZoneDto();
			tileZoneDto.Tile = tileModel.GroundType;
			tileZoneDto.Angle = tileModel.Angle / 90;
			tileZoneDto.TopLeft = new Point(tileModel.X, tileModel.Y);
			tileZoneDto.BottomRight = new Point(tileModel.X + 50, tileModel.Y + 50);
			return tileZoneDto;
		}

		/// <summary>
		/// Convert Decoration to Dto
		/// </summary>
		/// <param name="decorationModel"></param>
		/// <returns></returns>
		public static DecorationDto Convert(this DecorationModel decorationModel)
		{
			var decorationDto = new DecorationDto();
			decorationDto.Decoration = decorationModel.Decoration;
			decorationDto.Angle = 0;
			decorationDto.TopLeft = new Point(decorationModel.X, decorationModel.Y);
			return decorationDto;
		}
	
		/// <summary>
		/// Convert Item to Dto
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ItemDto Convert(this Item item)
		{
			var itemDto = new ItemDto();
			itemDto.ID = item.ItemId;
			itemDto.Code = item.Code;
			itemDto.StartPosition = item.StartPosition;
			itemDto.Orientation = item.Orientation;
			itemDto.DefaultState = item.DefaultState;
			return itemDto;
		}

		/// <summary>
		/// Convert Zone to Dto
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		public static ZoneDto Convert(this Zone zone)
		{
			var zoneDto = new ZoneDto();
			zoneDto.ID = zone.ZoneId;
			zoneDto.GroundType = zone.GroundType;
			zoneDto.TopLeft = zone.TopLeft;
			zoneDto.BottomRight = zone.BottomRight;
			return zoneDto;
		}
	}
}
