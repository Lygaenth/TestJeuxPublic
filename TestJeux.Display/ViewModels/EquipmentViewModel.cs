using Prism.Mvvm;
using System.Windows.Media.Imaging;
using TestJeux.Business.Entities.Equipments;
using TestJeux.Business.Managers.API;
using TestJeux.Display.Helper;

namespace TestJeux.Display.ViewModel
{
	public class EquipmentViewModel : BindableBase
    {
        private readonly Equipment _model;
        private bool _isEquiped;
        private int _quantity;
        private ICharacterManager _characterManager;

        public CachedBitmap Sprite { get => ImageHelper.GetImage(_model.Sprite); }
        public bool IsEquiped { get => _isEquiped; set => SetProperty(ref _isEquiped, value); }
        public int ID { get => _model.ID; }
        public int Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }

        public bool IsStackable { get => !_model.IsUnique; }

        public EquipmentViewModel(Equipment model, ICharacterManager characterManager)
        {
            _model = model;
            _characterManager = characterManager;
            _isEquiped = false;
            Quantity = 1;
        }

        public void Equip(int itemId)
        {
            _model.Equip(_characterManager.GetCharacter(itemId));
            IsEquiped = true;
        }

        public void Unequip(int itemId)
        {
            _model.Unequip(_characterManager.GetCharacter(itemId));
            IsEquiped = false;
        }
    }
}
