using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class DoorKey : Equipment
    {
        public DoorKey()
            : base(EquipmentCode.DoorKey, "Key", "Key")
        {
            _isUnique = false;
        }

        public override void Equip(ItemModel vm)
        {
            // Nothing
        }

        public override void Unequip(ItemModel vml)
        {
            // nothing
        }
    }
}
