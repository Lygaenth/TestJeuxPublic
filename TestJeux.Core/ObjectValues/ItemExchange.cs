using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.ObjectValues
{
	public class ItemExchange
	{
		public int OwnerID { get; }
		public EquipmentCode EquipmentCode { get; }

		public ItemExchange(int ownerId, EquipmentCode equipmentCode)
		{
			OwnerID = ownerId;
			EquipmentCode = equipmentCode;
		}
	}
}
