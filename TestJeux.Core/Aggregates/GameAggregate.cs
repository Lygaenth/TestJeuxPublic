using System;
using System.Collections.Generic;
using System.Linq;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Aggregates
{
	public class GameAggregate : GameAggregateBase
	{
		// To transform to have only one level, multiple level makes sense only in editor so it's a different aggregate
		
		public ItemModel GetControlledItem()
		{
			return _currentLevel.Items.FirstOrDefault(i => i.ItemType == ItemType.Character);
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
				// Make model loadig generic
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

	}
}
