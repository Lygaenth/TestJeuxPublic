using LevelEditor.wpf.ViewModel;
using System.Drawing;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.wpf.Converters
{
	public static class LevelConverter
    {
        /// <summary>
        /// Convert LevelVM to DTO
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static LevelDto Convert(this LevelViewModel level)
        {
            var levelDto = new LevelDto();
            levelDto.ID = level.ID;
            levelDto.LevelMusic = level.Music;
            levelDto.Shader = level.Shader;
            levelDto.DefaultTile = 1;

            foreach (var zone in level.Zones)
            {
                var zoneDto = new ZoneDto();
				zoneDto.ID = zone.ID;
				zoneDto.BottomRight = new Point(zone.X + zone.Width, zone.Y + zone.Heigth);
                zoneDto.TopLeft = new Point(zone.X, zone.Y);
                zoneDto.GroundType = zone.GroundType;
                levelDto.Zones.Add(zoneDto);
            }

            foreach (var tile in level.Tiles)
            {
                var tileDto = new TileZoneDto();
                tileDto.ID = tile.ID;
                tileDto.Angle = tile.Angle;
                tileDto.BottomRight = new Point(tile.X + 50, tile.Y + 50);
                tileDto.TopLeft = new Point(tile.X, tile.Y);
                tileDto.Tile = tile.GroundSprite;
                levelDto.TilesZones.Add(tileDto);
            }

            foreach (var decoration in level.Decorations)
            {
                var decorationDto = new DecorationDto();
                decorationDto.ID = decoration.ID;
                decorationDto.Decoration = decoration.Decoration;
                decorationDto.TopLeft = new Point(decoration.X, decoration.Y);
                decorationDto.Angle = decoration.Angle;
                levelDto.Decorations.Add(decorationDto);
            }

            foreach (var item in level.Items)
            {
                var itemDto = new ItemDto();
                itemDto.ID = item.ID;
                itemDto.StartPosition = item.StartPosition;
                itemDto.Code = item.Code;
                itemDto.DefaultState = 0;
                levelDto.Items.Add(itemDto);
            }

            return levelDto;

        }

        /// <summary>
        /// Convert zone VM to zone DTO
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static ZoneDto Convert(this ZoneViewModel zone)
        {
            var zoneDto = new ZoneDto();
            zoneDto.ID = zone.ID;
            zoneDto.GroundType = zone.GroundType;
            zoneDto.TopLeft = new Point(zone.X, zone.Y);
            zoneDto.BottomRight = new Point(zone.X + zone.Width, zone.Y + zone.Heigth);
            return zoneDto;
		}
    }
}
