using System;
using System.Collections.Generic;
using System.Linq;
using TestJeux.Business.Entities.Equipments;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	public delegate EventHandler<EquipmentEventArgs> AddEquipment();

    public class EquipmentManager : IEquipmentManager
    {
        private readonly GameAggregate _game;
        private List<Equipment> _availableEquipments;

        public event EventHandler<EquipmentEventArgs> UpdatedEquipments;

        public EquipmentManager(GameAggregate game)
        {
            _game = game;
            _availableEquipments = new List<Equipment>();
        }

        public Equipment GetEquipment(int id)
        {
            switch(id)
            {
                case 1:
                    return new Swimsuit();
                case 2:
                    return new Torch();
                case 3:
                    return new PickAxe();
                case 4:
                    return new DoorKey();
                case 5:
                    return new Knife();
                case 0:
                default:
                    return new NoEquipment();
            }
        }

        public void AddEquipment(Equipment equipment)
        {
            _availableEquipments.Add(equipment);

            var owner = _game.GetControlledItem();
            if (owner.Equipments.Any(e => e.ID == equipment.ID))
            {
                if (!equipment.IsUnique)
                    owner.Equipments.Find(e => e.ID == equipment.ID).Quantity++;
            }
            else
            {
                owner.Equipments.Add(equipment);
                equipment.Quantity = 1;
            }
                
            if (UpdatedEquipments != null)
                UpdatedEquipments(this, new EquipmentEventArgs(equipment.ID, ItemAction.Add));
        }

        public void RemoveEquipment(EquipmentCode code)
        {
            var owner = _game.GetControlledItem();
            if (owner.Equipments.Any(e => e.EquipmentCode == code))
            {
                var equipment = owner.Equipments.FirstOrDefault(e => e.EquipmentCode == code);
                if (!equipment.IsUnique && equipment.Quantity > 1)
                    owner.Equipments.Find(e => e.EquipmentCode == code).Quantity--;
                else
                    owner.Equipments.Remove(equipment);
            }

            if (UpdatedEquipments != null)
                UpdatedEquipments(this, new EquipmentEventArgs(code.GetHashCode(), ItemAction.Remove));
        }

        public void Reset()
        {
            _availableEquipments.Clear();
        }

        public List<Equipment> GetAvailableEquipments()
        {
            return _availableEquipments;
        }

		public void Subscribe()
		{
            foreach (var item in _game.GetItems())
            {
                item.ItemProvided += OnItemProvided;
				item.ItemConsumed += OnItemConsumed;
            }
		}

		public void Unsubscribe()
		{
			if (_game.GetCurrentLevel() == null)
				return;

			foreach (var item in _game.GetItems())
			{
				item.ItemProvided -= OnItemProvided;
				item.ItemConsumed -= OnItemConsumed;
			}
		}


		private void OnItemConsumed(object sender, Core.ObjectValues.ItemExchange e)
		{
            RemoveEquipment(e.EquipmentCode);
		}

		private void OnItemProvided(object sender, Core.ObjectValues.ItemExchange e)
		{
            AddEquipment(GetEquipment(e.EquipmentCode.GetHashCode()));
		}

	}
}
