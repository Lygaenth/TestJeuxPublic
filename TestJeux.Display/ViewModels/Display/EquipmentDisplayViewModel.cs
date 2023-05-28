using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TestJeux.Business.Managers.API;
using TestJeux.Display.ViewModel;

namespace TestJeux.Display.ViewModels.Display
{
	public class EquipmentDisplayViewModel : BindableBase
    {
        public ObservableCollection<EquipmentViewModel> Equipments { get; set; }

        private readonly IEquipmentManager _equipmentManager;
        private readonly ICharacterManager _characterManager;

        private int _characterId;

        int _selectedEquipmentIndex = 0;

        public EquipmentDisplayViewModel(IEquipmentManager equipmentManager, ICharacterManager characterManager)
        {
            Equipments = new ObservableCollection<EquipmentViewModel>();
            _equipmentManager = equipmentManager;
            _equipmentManager.UpdatedEquipments += OnEquipmentUpdated;

            _characterManager = characterManager;
        }

        public void SetCharacter(int characterId)
        {
            _characterId = characterId;
        }

        public void SwitchEquipment()
        {
            if (Equipments.Count < 1)
                return;

            if (Equipments.Count > _selectedEquipmentIndex)
                Equipments[_selectedEquipmentIndex].Unequip(_characterManager.GetCharacter(_characterId).ID);

            _selectedEquipmentIndex = (_selectedEquipmentIndex + 1) % Equipments.Count;
            Equipments[_selectedEquipmentIndex].Equip(_characterManager.GetCharacter(_characterId).ID);
        }

        private void OnEquipmentUpdated(object sender, EquipmentEventArgs args)
        {
            var equipments = _equipmentManager.GetAvailableEquipments();
            
            foreach (var item in Equipments)
            {
                if (item.ID != args.ID)
                    continue;

                item.Quantity += (args.Action == ItemAction.Add ? 1 : -1);
                if (item.Quantity == 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Equipments.Remove(item);
                    });
                }
                return;
            }

            foreach (var equip in equipments)
            {
                if (Equipments.Where(e => equip.ID == e.ID).Count() > 0)
                    continue;

                var newEquip = new EquipmentViewModel(equip, _characterManager);
                if (!Equipments.Any(e => e.IsEquiped) && equip.IsUnique)
                {
                    newEquip.Equip(_characterManager.GetCharacter(_characterId).ID);
                    _selectedEquipmentIndex = Equipments.Count;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Equipments.Add(newEquip);
                });
            }
        }

        public void Reset()
        {
            _equipmentManager.Reset();
            Equipments.Clear();
        }

    }
}
