﻿using TestJeux.Business.Entities.Items;
using TestJeux.Core.Entities;

namespace TestJeux.Business.Action
{
	public class RemoveItemAction : ActionBase
    {
        private readonly ItemModel _item;

        public RemoveItemAction(ItemModel itemViewModel)
        {
            _item = itemViewModel;
        }

        public override bool IsBlocking => false;

        public override bool Acq()
        {
            return false;
        }

        public override bool Execute()
        {
            _item.Remove();
            IsCompleted = true;
            return false;
        }
    }
}
