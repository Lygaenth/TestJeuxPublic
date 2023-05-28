using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
	public class Zone
	{
		public int ZoneId { get; set; }
		public GroundType GroundType { get; set; }
		public Point TopLeft { get; set; }
		public Point BottomRight { get; set; }
	}
}
