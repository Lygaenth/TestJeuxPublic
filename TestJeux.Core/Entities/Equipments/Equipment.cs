using TestJeux.Business.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Equipments
{
    public abstract class Equipment
    {

        private string _sprite;
        private string _name;
        protected bool _isUnique;
        private int _quantity;

        public Equipment(EquipmentCode equipmentCode, string name, string sprite)
        {
            EquipmentCode = equipmentCode;
            _sprite = sprite;
            _name = name;
            _isUnique = true;
        }

        public string Name { get => _name; }

        public EquipmentCode EquipmentCode { get; } 
        public int ID { get => EquipmentCode.GetHashCode(); }
        public string Sprite { get => _sprite; set => _sprite = value; }

        public bool IsUnique { get => _isUnique; }

        public int Quantity { get => _quantity; set => _quantity = value; }

        public abstract void Equip(ItemModel vm);

        public abstract void Unequip(ItemModel vm);
    }
}
