using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class NoEquipment : Equipment
    {
        public NoEquipment()
            : base("None", "")
        {
            ID = EquipmentCode.None.GetHashCode();
        }

        public override void Equip(ItemModel vm)
        {
            // Nothing
        }

        public override void Unequip(ItemModel vm)
        {
            // Nothing
        }
    }
}
