using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class Swimsuit : Equipment
    {
        public Swimsuit()
            : base(EquipmentCode.Swimsuit, "Swimsuit", "Maillot")
        {

        }

        public override void Equip(ItemModel vm)
        {
            vm.CanFloat = true;
        }

        public override void Unequip(ItemModel vm)
        {
            vm.CanFloat = false;
        }
    }
}
