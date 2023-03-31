using System;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Events
{
	public class MoveEventArgs : EventArgs
	{
		public int ItemId { get; private set; }
		public DirectionEnum Direction { get; private set; }

		public MoveEventArgs(int itemId, DirectionEnum direction)
		{
			ItemId = itemId;
			Direction = direction;
		}
	}
}
