using TestJeux.API.Models;
using TestJeux.Business.Entities.Items;
using TestJeux.Business.ObjectValues;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
	public class Torch : Equipment
    {
        public Torch()
            : base(EquipmentCode.Torch, "Torch", "TorchItem")
        {

        }

        public override void Equip(ItemModel creature)
        {
            if (creature is ILightSource lightSource)
            {
                lightSource.LightState = new LightState(true, 75);
            }
        }

        public override void Unequip(ItemModel creature)
        {
			if (creature is ILightSource lightSource && lightSource.LightState != null)
			{
				lightSource.LightState = lightSource.LightState.TurnOff();
			}
		}
	}
}
