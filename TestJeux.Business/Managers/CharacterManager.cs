using System;
using System.Collections.Generic;
using TestJeux.API.Models;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	public class CharacterManager : ICharacterManager
	{
		private readonly GameAggregate _gameRoot;

		public CharacterManager(GameAggregate gameRoot)
		{
			_gameRoot = gameRoot;
		}

		public void Reset()
		{
			//_characters.Clear();
		}

		public ItemModel GetCharacter(int itemId, ItemCode code)
		{
			if (!_gameRoot.HasItemInCurrentLevel(itemId))
			{
				if (!_gameRoot.HasItemInCurrentLevel(itemId))
					_gameRoot.AddItemToCurrentLevel(itemId, code);
			}
			return _gameRoot.GetItemFromCurrentLevel(itemId);			
		}

		// TO DO remove direct access, replace by properties access through a DTO 
		public ItemModel GetCharacter(int itemId)
		{
			return _gameRoot.GetItemFromCurrentLevel(itemId);
		}

		/// <summary>
		/// Return character state
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public ItemStateDto GetCharacterState(int itemId)
		{
			var item = _gameRoot.GetItemFromCurrentLevel(itemId);
			var itemStateDto = new ItemStateDto();
			itemStateDto.ID = item.ID;
			itemStateDto.Position = item.Position;
			itemStateDto.Orientation = item.Orientation;

			return itemStateDto;
		}

		

		public List<string> GetCurrentSprites(int itemId)
		{
			if (!_gameRoot.HasItemInCurrentLevel(itemId))
				return new List<string>();

			return _gameRoot.GetItemFromCurrentLevel(itemId).CurrentSprites;
		}

		public void SetCharacterOrientation(int itemId, DirectionEnum direction)
		{
			if (_gameRoot.HasItemInCurrentLevel(itemId))
				_gameRoot.GetItemFromCurrentLevel(itemId).Orientation = direction;
		}

		public DirectionEnum GetCharacterOrientation(int itemId)
		{
			if (!_gameRoot.HasItemInCurrentLevel(itemId))
				throw new InvalidOperationException("Item " + itemId + " does not exist in current level");

			return _gameRoot.GetItemFromCurrentLevel(itemId).Orientation;
		}

		public int GetControlledItemId()
		{
			return _gameRoot.GetControlledItem().ID;
		}
	}
}
