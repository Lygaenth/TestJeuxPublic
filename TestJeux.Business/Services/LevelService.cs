using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Test.Jeux.Data.Api;
using TestJeux.API.Models;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Business.Services.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Services
{
	public class LevelService : ILevelService
    {
        #region Attributes
        GameAggregate _game;
        IDALLevels _dal;
        public event LevelChange RaiseLevelChange;
		#endregion


		public LevelService(GameAggregate gameAggregate, IDALLevels dalLevels)
        {
            _game = gameAggregate;
            _dal = dalLevels;
        }

        #region public methods
        public void Reset()
        {
            _game.ClearState();
        }

        public List<int> GetAllLevelIds()
        {
            var levels = _dal.LoadAllLevels().Select(l => l.ID).ToList();
            levels.Sort();
            return levels;
        }

        public LevelDto GetLevel(int levelId)
        {
            if (!_game.HasLevel(levelId))
                _game.AddLevel(CreateLevel(levelId));
            _game.SetLevelAsCurrent(levelId);

            return CreateLevelDto(_game.GetCurrentLevel());
        }

        public int GetCurrentLevel()
        {
            if (_game.GetCurrentLevel() == null)
                return -1;
            return _game.GetCurrentLevel().ID;
        }

        /// <summary>
        /// Get groundType from current level
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
		public GroundType GetGroundType(Point position)
        {
            return GetGroundType(GetCurrentLevel(), position);
        }

        /// <summary>
        /// Get ground type
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public GroundType GetGroundType(int levelId, Point position)
        {
            if (!IsPointInRectangle(new Point(0, 0), new Point(ConstantesDisplay.MaxWidth, ConstantesDisplay.MaxHeigth), position))
            {
                return GroundType.OutOfBound;
            }

            foreach (var zone in _game.GetLevel(levelId).Zones)
            {
                if (IsPointInRectangle(zone.TopLeft, zone.BottomRight, position))
                    return zone.GroundType;
            }
            return GroundType.Neutral;
        }

        public void ChangeLevel(int id)
        {
            if (RaiseLevelChange != null)
                RaiseLevelChange(this, new LevelChangeArgs(id));
        }
        #endregion

        #region private methods
        private Level CreateLevel(int levelId)
        {
            var levelDto = _dal.GetDataById(levelId);
            var level = CreateLevel(levelDto);
            return level;
        }

        private Level CreateLevel(LevelDto levelDto)
        {
            var level = new Level(levelDto.ID);
            level.Shader = levelDto.Shader;
            level.Music = levelDto.LevelMusic;
            level.DefaultTile = (GroundSprite)levelDto.DefaultTile;

            foreach (var zone in levelDto.Zones)
                level.Zones.Add(new ZoneModel(zone.ID, zone.TopLeft, zone.BottomRight, zone.GroundType));

            int decorationId = 0;
            foreach (var decoration in levelDto.Decorations)
            {
                level.Decorations.Add(new Decoration(decorationId, decoration.Decoration, decoration.TopLeft));
                decorationId++;
            }

            int id = 0;
            foreach (var tileZone in levelDto.TilesZones)
            {
                id++;
                level.Tiles.Add(new TileZone(id, tileZone.Tile, tileZone.TopLeft, tileZone.BottomRight, tileZone.Angle));
            }

            _game.AddLevel(level);
            _game.SetLevelAsCurrent(level.ID);

            foreach(var itemDto in levelDto.Items)
            {
                _game.AddItemToCurrentLevel(itemDto.ID, itemDto.Code);
                var itemCreated = _game.GetItemFromCurrentLevel(itemDto.ID);
                itemCreated.SetDefaultState(itemDto.DefaultState);
                itemCreated.X = itemDto.StartPosition.X;
                itemCreated.Y = itemDto.StartPosition.Y;
                itemCreated.Orientation = itemDto.Orientation;
            }
            return level;
        }

        private LevelDto CreateLevelDto(Level level)
        {
            var dto = new LevelDto();
            dto.ID = level.ID;
            dto.LevelMusic = level.Music;
            dto.Shader = level.Shader;

            foreach (var zone in level.Zones)
                dto.Zones.Add(new ZoneDto(zone.ID, zone.BottomRight, zone.TopLeft, zone.GroundType));

            foreach (var decoration in level.Decorations)
                dto.Decorations.Add(new DecorationDto() { Decoration = decoration.Enumvalue, TopLeft = decoration.TopLeft });

            dto.TilesZones = Convert(level.Tiles);
            dto.ItemsIDs = level.Items.Select(i => i.ID).ToList();
            return dto;
        }

        private bool IsPointInRectangle(Point topLeft, Point bottomRight, Point point)
        {
            return !(point.X < topLeft.X || point.Y < topLeft.Y || point.X >= bottomRight.X || point.Y >= bottomRight.Y);
        }

        public void SaveLevel(LevelDto level)
        {
            _dal.SaveLevel(level);
        }
        #endregion

        private List<TileZoneDto> Convert(List<TileZone> tileZones)
        {
            var listDtos = new List<TileZoneDto>();
            foreach(var tileZone in tileZones)
                listDtos.Add(new TileZoneDto() { Tile = tileZone.Enumvalue, TopLeft = tileZone.TopLeft, BottomRight = tileZone.BottomRight, Angle = tileZone.Angle });

            return listDtos;
        }

		public void Subscribe()
		{
            // Nothing to do here
		}

		public void Unsubscribe()
		{
			// Nothing to do here
		}
	}
}
