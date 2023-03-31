using System.Collections.Generic;

namespace TestJeux.Business.Entities.Sprites
{
    public class CreatureSpriteModel : SpriteModel
    {
        public CreatureSpriteModel()
        {
            DeathSprite = new List<string> { "" };
        }

        public List<string> DeathSprite { get; set; }
    }
}
