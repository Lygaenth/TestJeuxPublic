using System;
using System.Collections.Generic;
using TestJeux.API.Services;
using TestJeux.Business.Entities.Equipments;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers.API
{
	public interface IEquipmentManager : ISubscribingService
    {
        event EventHandler<EquipmentEventArgs> UpdatedEquipments;

        List<Equipment> GetAvailableEquipments();

        Equipment GetEquipment(int id);

        void AddEquipment(Equipment equipment);

        void RemoveEquipment(EquipmentCode equipment);
    }
}
