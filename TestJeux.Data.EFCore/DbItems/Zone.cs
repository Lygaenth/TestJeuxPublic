using System.Drawing;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Data.EFCore.DbItems
{
	public class Zone
	{
		public int ZoneId { get; set; }
		public int LevelId { get; set; }
		public GroundType GroundType { get; set; }
		public int X1 { get; set; }
		public int Y1 { get; set; }
		public int X2 { get; set; }
		public int Y2 { get; set; }

		public void Update(ZoneDto zone)
		{
			GroundType = zone.GroundType;
			X1 = zone.TopLeft.X;
			Y1 = zone.TopLeft.Y;
			X2 = zone.BottomRight.X;
			Y2 = zone.BottomRight.Y;
		}
	}
}
