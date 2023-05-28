using System;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Display.ViewModels;
using TestJeux.Display.ViewModels.Display;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Supervisor
{
	public class CharacterBuilder : ICharacterBuilder
	{
		private readonly ICharacterManager _characterManager;
		private readonly IMoveService _moveManager;
		private readonly IScriptService _scriptManager;

		// Split Character builder for editor and for game later
		bool _useScript;
		bool _useMove;

		private readonly GameViewModel _gameViewModel;

		public CharacterBuilder(GameViewModel gameViewModel, IMoveService moveManager, ICharacterManager characterManager, IScriptService scriptManager)
		{
			_gameViewModel = gameViewModel;
			_characterManager = characterManager;
			_moveManager = moveManager;
			_scriptManager = scriptManager;
		}

		public CharacterBuilder(GameViewModel gameViewModel, ICharacterManager characterManager)
		{
			_gameViewModel = gameViewModel;
			_characterManager = characterManager;
		}

		public ItemViewModel CreateItem(ItemModel item)
		{
			if (item.ID == 1 && _gameViewModel.DisplayVm.LevelVm.SelectedItem != null)
				return _gameViewModel.DisplayVm.LevelVm.SelectedItem;

			var model = _characterManager.GetCharacter(item.ID, item.Code);
			if (model.ItemType != ItemType.Objet)
			{
				if (model.HasScript)
					_scriptManager.Add(model.ID);
			}

			model.X = item.X;
			model.Y = item.Y;
			model.Orientation = item.Orientation;
			model.RequireDestruction += OnItemDestructionRequired;

			var itemVm = new ItemViewModel(model, _characterManager, _moveManager);

			return itemVm;
		}

		private void OnItemDestructionRequired(object sender, EventArgs e)
		{
			var item = sender as ItemModel;
			if (item == null)
				return;

			item.RequireDestruction -= OnItemDestructionRequired;
			_gameViewModel.DisplayVm.LevelVm.RemoveItem(item.ID);
			_moveManager.Unregister(item.ID);
		}
	}
}
