using System.Drawing;
using System.Text;
using TestJeux.API.Models;
using TestJeux.API.Services.Edition;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Business.Services.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.Data.Api;
using TestJeux.SharedKernel.Enums;
using TestJeux.API.Converters;

namespace TestJeux.Business.Edition.Service
{
	/// <summary>
	/// Level service dedicated to edition
	/// </summary>
	public class LevelEditionService : ILevelEditionService
	{
		// TODO split logic between LevelEdition and LevelService (ingame)

		protected GameEditorAggregate _game;
		private readonly IDALLevels _dal;
		private readonly IDALLevels _dalEF;

		public event LevelChange RaiseLevelChange;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="gameAggregate"></param>
		/// <param name="dalLevels"></param>
		/// <param name="dalEf"></param>
		public LevelEditionService(GameEditorAggregate gameAggregate, IDALLevels dalLevels, IDALLevels dalEf)
		{
			_game = gameAggregate;
			_dal = dalLevels;
			_dalEF = dalEf;
		}

		/// <summary>
		/// Update zone service
		/// </summary>
		/// <param name="zoneDto"></param>
		public void UpdateZone(ZoneDto zoneDto)
		{
			_game.UpdateZone(zoneDto.ID, zoneDto.TopLeft, zoneDto.BottomRight, zoneDto.GroundType);
		}

		/// <summary>
		/// Update zone service
		/// </summary>
		/// <param name="zoneDto"></param>
		public void AddZone(ZoneDto zoneDto)
		{
			_game.AddZone(zoneDto.TopLeft, zoneDto.BottomRight, zoneDto.GroundType);
		}

		/// <summary>
		/// Remove zone
		/// </summary>
		/// <param name="zoneId"></param>
		public void RemoveZone(int zoneId)
		{
			_game.RemoveZone(zoneId);
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
		/// Get level ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public LevelDto GetLevel(int id)
		{
			if (!_game.HasLevel(id))
				_game.AddLevel(CreateLevel(id));
			_game.SetLevelAsCurrent(id);

			return CreateLevelDto(_game.GetCurrentLevel());
		}

		/// <summary>
		/// Get all level Ids
		/// </summary>
		/// <returns></returns>
		public List<int> GetAllLevelIds()
		{
			var levels = _dal.LoadAllLevels().Select(l => l.ID).ToList();
			levels.Sort();
			return levels;
		}

		/// <summary>
		/// Get groun
		/// </summary>
		/// <param name="levelId"></param>
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

		public void Reset()
		{
			_game.ClearState();
		}


		#region private methods
		private Level CreateLevel(int levelId)
		{
			var levelDto = _dal.GetDataById(levelId);
			var level = CreateLevel(levelDto);
			return level;
		}

		private Level CreateLevel(LevelDto levelDto)
		{
			var level = levelDto.Convert();

			_game.AddLevel(level);
			_game.SetLevelAsCurrent(level.ID);

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
				dto.Decorations.Add(new DecorationDto() { Decoration = decoration.Enumvalue, TopLeft = decoration.TopLeft, Angle = decoration.Angle });

			dto.TilesZones = Convert(level.Tiles);
			dto.ItemsIDs = level.Items.Select(i => i.ID).ToList();
			dto.Items = Convert(level.Items);
			return dto;
		}

		private bool IsPointInRectangle(Point topLeft, Point bottomRight, Point point)
		{
			return !(point.X < topLeft.X || point.Y < topLeft.Y || point.X >= bottomRight.X || point.Y >= bottomRight.Y);
		}

		public void SaveLevel(int levelId)
		{
			//_dal.SaveLevel(level);
			_dal.SaveLevel(_game.GetLevel(levelId).Convert());
		}
		#endregion

		private List<TileZoneDto> Convert(List<TileZone> tileZones)
		{
			var listDtos = new List<TileZoneDto>();
			foreach (var tileZone in tileZones)
				listDtos.Add(new TileZoneDto() { Tile = tileZone.Enumvalue, TopLeft = tileZone.TopLeft, BottomRight = tileZone.BottomRight, Angle = tileZone.Angle });

			return listDtos;
		}

		private List<ItemDto> Convert(List<ItemModel> items)
		{
			var listDtos = new List<ItemDto>();
			foreach (var item in items)
				listDtos.Add(new ItemDto() { ID = item.ID, Code = item.Code, DefaultState = item.DefaultState, Orientation = item.Orientation, StartPosition = item.Position });
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
