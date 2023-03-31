using System.Collections.Generic;

namespace TestJeux.API.Sprites
{
	public class SpritesDto
    {
        List<string> Back { get; set; }
        List<string> Front { get; set; }
        List<string> Left { get; set; }
        List<string> Right { get; set; }
        List<string> MovingBack { get; set; }
        List<string> MovingFront { get; set; }
        List<string> MovingLeft { get; set; }
        List<string> MovingRight { get; set; }

        List<string> OptionalSprite1 { get; set; }
        List<string> OptionalSprite2 { get; set; }
        List<string> OptionalSprite3 { get; set; }
    }
}