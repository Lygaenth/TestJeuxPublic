using TestJeux.Core.Entities.Items;

namespace TestJeux.Business.Entities.Equipments
{
	public abstract class Equipment
    {

        private string _sprite;
        private string _name;
        protected bool _isUnique;

        public Equipment(string name, string sprite)
        {
            _sprite = sprite;
            _name = name;
            _isUnique = true;
        }

        public string Name { get => _name; }

        public int ID { get; set; }
        public string Sprite { get => _sprite; set => _sprite = value; }

        public bool IsUnique { get => _isUnique; }

        public abstract void Equip(ItemModel vm);

        public abstract void Unequip(ItemModel vm);
    }
}
