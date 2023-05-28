using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
	public class Item
	{
		public int ItemId { get; set; }
		public ItemCode Code { get; set; }
		public DirectionEnum Orientation { get; set; }
		public Point StartPosition { get; set; }
		public int DefaultState { get; set; }
	}
}
