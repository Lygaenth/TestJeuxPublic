using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class Knife : Equipment
    {
        public Knife()
            : base(EquipmentCode.Knife, "Knife", "Knife")
        {

        }

        public override void Equip(ItemModel creature)
        {
            //(creature as HeroModel).IsArmed = true;
            creature.Stats.Attack += 5;
        }

        public override void Unequip(ItemModel creature)
        {
            //(creature as HeroModel).IsArmed = false;
            creature.Stats.Attack -= 5;
        }
    }
}
