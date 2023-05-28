using System.Drawing;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Data.EFCore.DbItems
{
	public class Item
	{
		public int ItemId { get; set; }
		public int LevelId { get; set; }
		public ItemCode Code { get; set; }
		public DirectionEnum Orientation { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int DefaultState { get; set; }

		public void Update(ItemDto itemDto)
		{
			Code = itemDto.Code;
			Orientation = itemDto.Orientation;
			X = itemDto.StartPosition.X;
			Y = itemDto.StartPosition.Y;
			DefaultState = itemDto.DefaultState;
		}
	}
}
