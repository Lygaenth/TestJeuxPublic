using TestJeux.Core.Entities.Items;
using TestJeux.Display.ViewModels.Display;

namespace TestJeux.Business.Supervisor
{
	public interface ICharacterBuilder
	{
		ItemViewModel CreateItem(ItemModel itemDto);
	}
}