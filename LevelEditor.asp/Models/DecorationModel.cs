using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
	public class DecorationModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }

        public Decorations Decoration { get; set; }

        public DecorationModel(DecorationDto decorationDto, string image)
        {
            Decoration = decorationDto.Decoration;
            X = decorationDto.TopLeft.X;
            Y = decorationDto.TopLeft.Y;
            Image = image;
        }
    }
}
