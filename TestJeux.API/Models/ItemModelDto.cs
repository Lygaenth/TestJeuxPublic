using TestJeux.API.Sprites;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models.ItemTypeInterface
{
	public class ItemModelDto
    {
        public ItemType ItemType { get; set; }
        public bool CanBePushed { get; set; }
        public bool CanFloat { get; set; }
        public int ID { get; set; }
        public bool IsGround { get; set; }
        public bool IsLightSource { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public SpritesDto SpriteModel { get; set; }
        public StatsDto StatsModel { get; set; }
        public int AnimationDuration { get; set; }
    }
}