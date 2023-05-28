using TestJeux.API.Models;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Business.Supervisor;
using TestJeux.Display.ViewModels.Display;

namespace LevelEditor.wpf.Builders
{
	public class CharacterBuilderEditor : ICharacterBuilder
	{
		private readonly ICharacterManager _characterManager;

		public CharacterBuilderEditor(ICharacterManager characterManager)
		{
			_characterManager = characterManager;
		}

		public ItemViewModel CreateItem(ItemModel itemDto)
		{
			var model = _characterManager.GetCharacter(itemDto.ID, itemDto.Code);
			model.X = itemDto.X;
			model.Y = itemDto.Y;
			model.Orientation = itemDto.Orientation;

			return new ItemViewModel(model, _characterManager, null);
		}
	}
}
