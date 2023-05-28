using System.Drawing;
using TestJeux.API.Models;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Converters
{
	public static class LevelConverters
    {
        public static LevelDto Convert(this Level level)
        {
            var levelDto = new LevelDto();
            levelDto.ID = level.ID;
            levelDto.LevelMusic = level.Music;
            levelDto.Shader = level.Shader;
            levelDto.DefaultTile = 1;

            foreach (var zone in level.Zones)
            {
                var zoneDto = new ZoneDto();
                zoneDto.BottomRight = zone.BottomRight;
                zoneDto.TopLeft = new Point(zone.TopLeft.X, zone.TopLeft.Y);
                zoneDto.ID = zone.ID;
                zoneDto.GroundType = zone.GroundType;
                levelDto.Zones.Add(zoneDto);
            }

            foreach (var tile in level.Tiles)
            {
                var tileDto = new TileZoneDto();
                tileDto.Angle = tile.Angle;
                tileDto.BottomRight = new Point(tile.TopLeft.X + 50, tile.TopLeft.Y + 50);
                tileDto.TopLeft = new Point(tile.TopLeft.X, tile.TopLeft.Y);
                tileDto.Tile = tile.Enumvalue;
                levelDto.TilesZones.Add(tileDto);
            }

            foreach (var decoration in level.Decorations)
            {
                var decorationDto = new DecorationDto();
                decorationDto.Decoration = decoration.Enumvalue;
                decorationDto.TopLeft = new Point(decoration.TopLeft.X, decoration.TopLeft.Y);
                decorationDto.Angle = decoration.Angle;
                levelDto.Decorations.Add(decorationDto);
            }

            foreach (var item in level.Items)
            {
                var itemDto = new ItemDto();
                itemDto.StartPosition = item.Position;
                itemDto.Code = item.Code;
                // Make other view model for editor since some values can be modified that shouldn't in game
                itemDto.DefaultState = 0;
            }

            return levelDto;
        }
    
        public static Level Convert(this LevelDto levelDto)
        {
            var level = new Level(levelDto.ID);
            level.Shader = levelDto.Shader;
            level.Music = levelDto.LevelMusic;
            level.DefaultTile = (GroundSprite)levelDto.DefaultTile;

            foreach (var zone in levelDto.Zones)
                level.Zones.Add(new ZoneModel(zone.ID, zone.TopLeft, zone.BottomRight, zone.GroundType));

            foreach (var decoration in levelDto.Decorations)
                level.Decorations.Add(new Decoration(decoration.ID, decoration.Decoration, decoration.TopLeft, decoration.Angle));

            foreach (var tileZone in levelDto.TilesZones)
                level.Tiles.Add(new TileZone(tileZone.ID, tileZone.Tile, tileZone.TopLeft, tileZone.BottomRight, tileZone.Angle));

            return level;
        }
    }
}
