using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TestJeux.Data.Api;
using TestJeux.API.Models;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Business.Services.API;
using TestJeux.Core.Aggregates;
using TestJeux.SharedKernel.Enums;
using TestJeux.API.Converters;

namespace TestJeux.Business.Services
{
	public class LevelService : ILevelService
    {
        #region Attributes
        protected GameAggregate _game;
        private readonly IDALLevels _dal;
        private readonly IDALLevels _dalEF;

        public event LevelChange RaiseLevelChange;
		#endregion


		public LevelService(GameAggregate gameAggregate, IDALLevels dalLevels, IDALLevels dalEf)
        {
            _game = gameAggregate;
            _dal = dalLevels;
            _dalEF = dalEf;
        }

        #region public methods
        /// <summary>
        /// Reset level states to database
        /// </summary>
        public void Reset()
        {
            _game.ClearState();
        }

        /// <summary>
        /// Get all existing level IDs
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllLevelIds()
        {
            var levels = _dal.LoadAllLevels().Select(l => l.ID).ToList();
            levels.Sort();
            return levels;
        }

        /// <summary>
        /// Get level
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public LevelDto GetLevel(int levelId)
        {
            if (!_game.HasLevel(levelId))
                _game.AddLevel(CreateLevel(levelId));
            _game.SetLevelAsCurrent(levelId);

            return _game.GetCurrentLevel().Convert();
        }

        /// <summary>
        /// Get current level
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Change level
        /// </summary>
        /// <param name="id"></param>
        public void ChangeLevel(int id)
        {
            if (RaiseLevelChange != null)
                RaiseLevelChange(this, new LevelChangeArgs(id));
        }
        #endregion

        #region private methods
        /// <summary>
        /// Load level by ID
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        private Level CreateLevel(int levelId)
        {
            var levelDto = _dal.GetDataById(levelId);
            var level = InitializeLevel(levelDto);
            return level;
        }

        /// <summary>
        /// Initialize level
        /// </summary>
        /// <param name="levelDto"></param>
        /// <returns></returns>
        private Level InitializeLevel(LevelDto levelDto)
        {
            var level = levelDto.Convert();

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

        /// <summary>
        /// Check if point is in rectangle
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsPointInRectangle(Point topLeft, Point bottomRight, Point point)
        {
            return !(point.X < topLeft.X || point.Y < topLeft.Y || point.X >= bottomRight.X || point.Y >= bottomRight.Y);
        }

        /// <summary>
        /// Save level associated to specific ID
        /// </summary>
        /// <param name="levelId"></param>
        public void SaveLevel(int levelId)
        {
            //_dal.SaveLevel(level);
            _dal.SaveLevel(_game.GetLevel(levelId).Convert());
        }
        #endregion

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
