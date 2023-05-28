using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Entities.LevelElements;

namespace TestJeux.Core.Aggregates
{
	public class GameAggregateBase
	{
		// To transform to have only one level, multiple level makes sense only in editor so it's a different aggregate
		protected Level _currentLevel;
		protected List<Level> _levels;

		// before creating repository
		protected List<ItemModel> _storedItems;

		public GameAggregateBase()
		{
			_storedItems = new List<ItemModel>();
			_levels = new List<Level>();
		}

		public void ClearState()
		{
			_levels.Clear();
			_currentLevel = null;
		}

		public Level GetCurrentLevel()
		{
			return _currentLevel;
		}

		public bool HasLevel(int levelId)
		{
			return _levels.Any(l => l.ID == levelId);
		}

		public void AddLevel(Level level)
		{
			_levels.Add(level);
		}

		public Level GetLevel(int levelId)
		{
			if (HasLevel(levelId))
				return _levels.Find(l => l.ID == levelId);

			throw new InvalidOperationException("No level with this ID");
		}

		public void SetLevelAsCurrent(int levelId)
		{
			if (!HasLevel(levelId))
				throw new InvalidOperationException("No level with this ID");

			_currentLevel = GetLevel(levelId);
		}

		protected void CheckCurrentLevel()
		{
			if (_currentLevel == null)
				throw new InvalidOperationException("Current level not set");
		}

	}
}
