using System.Drawing;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Data.EFCore.DbItems
{
	public class Tile
	{
		public int TileId { get; set; }
		public int LevelId { get; set; }
		public GroundSprite Type { get; set; }
		public int Angle { get; set; }
		public int X1 { get; set; }
		public int Y1 { get; set; }
		public int X2 { get; set; }
		public int Y2 { get; set; }

		public void Update(TileZoneDto tileDto)
		{
			Type = tileDto.Tile;
			Angle = tileDto.Angle;
			X1 = tileDto.TopLeft.X;
			Y1 = tileDto.TopLeft.Y;
			X2 = tileDto.BottomRight.X;
			Y2 = tileDto.BottomRight.Y;
		}
	}
}
