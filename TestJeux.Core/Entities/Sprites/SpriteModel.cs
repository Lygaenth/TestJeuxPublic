using System.Collections.Generic;

namespace TestJeux.Business.Entities.Sprites
{
	public class SpriteModel
    {
        public List<string> Front { get; set; }
        public List<string> Back { get; set; }
        public List<string> Left { get; set; }
        public List<string> Right { get; set; }
        public List<string> MovingFront { get; set; }
        public List<string> MovingBack { get; set; }
        public List<string> MovingLeft { get; set; }
        public List<string> MovingRight { get; set; }
        public List<string> OptionalSprite1 { get; set; }
        public List<string> OptionalSprite2 { get; set; }
        public List<string> OptionalSprite3 { get; set; }
    }
}
