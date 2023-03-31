using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJeux.Business.Managers.API
{
    public enum ItemAction
    {
        New,
        Add,
        Remove
    }

    public class EquipmentEventArgs : EventArgs
    {
        public int ID { get; set; }
        public ItemAction Action { get; set; }
        public EquipmentEventArgs(int id, ItemAction action)
        {
            ID = id;
            Action = action;
        }
    }
}
