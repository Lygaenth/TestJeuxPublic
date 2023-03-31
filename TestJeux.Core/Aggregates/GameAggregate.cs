using System;
using System.Collections.Generic;
using System.Linq;
using TestJeux.Core.Entities.Items;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Aggregates
{
	public class GameAggregate
	{
		// To transform to have only one level, multiple level makes sense only in editor so it's a different aggregate
		private Level _currentLevel;
		private List<Level> _levels;
		
		// before creating repository
		private List<ItemModel> _storedItems;

		/// <summary>
		/// Constructor
		/// </summary>
		public GameAggregate()
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

		public IReadOnlyList<ItemModel> GetItems()
		{
			CheckCurrentLevel();
			return _currentLevel.Items;
		}

		public ItemModel GetItemFromCurrentLevel(int itemId)
		{
			CheckCurrentLevel();
			return _currentLevel.Items.Find(i => i.ID == itemId);
		}

		public void AddItemToCurrentLevel(int itemID, ItemCode code)
		{
			CheckCurrentLevel();
			ItemModel item;
			if (!_storedItems.Any(i => i.ID == itemID && i.Code == code))
			{
				item = CreateCharacters(itemID, code);
				RemoveOldItemWithIDIfExists(itemID);

				_storedItems.Add(item);
			}
			item = _storedItems.Find(i => i.ID == itemID);
			_currentLevel.Items.Add(item);
		}

		private void RemoveOldItemWithIDIfExists(int itemID)
		{
			if (_storedItems.Any(i => i.ID == itemID))
			{
				var oldItem = _storedItems.Find(i => i.ID == itemID);
				_storedItems.Remove(oldItem);
			}
		}

		private ItemModel CreateCharacters(int id, ItemCode code)
		{
			ItemModel character;
			character = code switch
			{
				// Implement items models
				_ => throw new Exception("item code not found")
			};
			character.Initialize();
			return character;
		}

		public bool HasItemInCurrentLevel(int itemId)
		{
			CheckCurrentLevel();
			return _currentLevel.Items.Any(i => i.ID == itemId);
		}

		private void CheckCurrentLevel()
		{
			if (_currentLevel == null)
				throw new InvalidOperationException("Current level not set");
		}
	}
}
