using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class PickAxe : Equipment
    {
        public PickAxe()
            : base(EquipmentCode.PickAxe, "Pickaxe", "PickAxe")
        {

        }

        public override void Equip(ItemModel creature)
        {
            //(creature as HeroModel).CanMine = true;
            //(creature as HeroModel).IsArmed = true;
            creature.Stats.Attack += 3;
        }

        public override void Unequip(ItemModel creature)
        {
            //(creature as HeroModel).CanMine = false;
            //(creature as HeroModel).IsArmed = false;
            creature.Stats.Attack -= 3;
        }
    }
}
