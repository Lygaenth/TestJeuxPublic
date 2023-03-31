using Test.Jeux.Data;
using Test.Jeux.Data.Api;
using TestJeux.API.Models;

namespace LevelEditor.asp.Data
{
	public class LevelService
	{
		private readonly IDALLevels _dal;
		private List<LevelDto> _levels;

		public LevelService()
		{
			_dal = new DALLevels();
			_levels = new List<LevelDto>();
		}

		public List<int> GetLevelList()
		{
			if (_levels.Any())
				return _levels.Select(s => s.ID).ToList();
			return _dal.LoadAllLevels().Select(a => a.ID).ToList();
		}

		public LevelDto GetLevel(int id)
		{
			if (HasLevel(id))
				return _levels.First(l => l.ID == id);
			
			var levelDto = _dal.GetDataById(id);
			_levels.Add(levelDto);
			return levelDto;
		}

		public bool HasLevel(int id)
		{
			return (_levels.Any(l => l.ID == id));	
		}

		public void AddLevel(LevelDto levelDto)
		{
			_levels.Add(levelDto);
		}

		// Add reload and save
	}
}
