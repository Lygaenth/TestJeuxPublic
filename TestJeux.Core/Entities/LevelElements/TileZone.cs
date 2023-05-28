using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Entities.LevelElements
{
	public class TileZone : GraphicalEntity<GroundSprite>
	{
		public static TileZone None = new TileZone(0, GroundSprite.Grass, new Point(0, 0), new Point(0, 0), 0);

		/// <summary>
		/// End of zone
		/// </summary>
		public Point BottomRight { get; set; }

	
		/// <summary>
		/// Tile zone constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="enumValue"></param>
		/// <param name="topLeft"></param>
		/// <param name="bottomRight"></param>
		/// <param name="angle"></param>
		public TileZone(int id, GroundSprite enumValue, Point topLeft, Point bottomRight, int angle)
			: base(id, enumValue, topLeft, angle)
		{
			BottomRight = bottomRight;
		}
	}
}
