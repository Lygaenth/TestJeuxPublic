using System;
using System.Collections.Generic;
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
                // implemtent equipments 
                default:
                    return new NoEquipment();
            }
        }

        public void AddEquipment(Equipment equipment)
        {
            _availableEquipments.Add(equipment);

            if (UpdatedEquipments != null)
                UpdatedEquipments(this, new EquipmentEventArgs(equipment.ID, ItemAction.Add));
        }

        public void RemoveEquipment(EquipmentCode code)
        {
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
