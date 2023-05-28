using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Data.EFCore.DbItems
{
	public class Decoration
	{
		public int DecorationId { get; set; }
		public int LevelId { get; set; }
		public Decorations Type { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public void Update(DecorationDto decorationDto)
		{
			Type = decorationDto.Decoration;
			X = decorationDto.TopLeft.X;
			Y = decorationDto.TopLeft.Y;
		}
	}
}
