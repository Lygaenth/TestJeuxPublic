using TestJeux.Business.Entities.Equipments;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class InventoryAction : ActionBase
    {
        private IEquipmentManager _equipmentManager;
        private readonly ItemModel _item;
        public override ActionType ActionType => ActionType.Inventory;
        private readonly ItemAction _action;
        private readonly Equipment _equipment;
        private readonly EquipmentCode _equipmentCode;

        public override bool IsBlocking => false;

        public override bool Acq()
        {
            return true;
        }

        public InventoryAction(IEquipmentManager equipmentManager, ItemModel item, ItemAction action, Equipment equipment)
        {
            _equipmentManager = equipmentManager;
            _item = item;
            _action = action;
            _equipment = equipment;
            _equipmentCode = (EquipmentCode)equipment.ID;
        }

        public InventoryAction(IEquipmentManager equipmentManager, ItemModel item, ItemAction action, EquipmentCode code)
        {
            _equipmentManager = equipmentManager;
            _item = item;
            _action = action;
            _equipmentCode = code;
            _equipment = _equipmentManager.GetEquipment(code.GetHashCode());
        }

        public override bool Execute()
        {
            if (_action == ItemAction.Add)
            {
                _item.Equipments.Add(_equipment);
                _equipmentManager.AddEquipment(_equipment);
            }
            else if (_action == ItemAction.Remove)
            {
                _equipmentManager.RemoveEquipment(_equipmentCode);
            }
            IsCompleted = true;
            return true;
        }
    }
}
