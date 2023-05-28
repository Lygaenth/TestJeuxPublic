using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class NoEquipment : Equipment
    {
        public NoEquipment()
            : base(EquipmentCode.None, "None", "")
        {

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
