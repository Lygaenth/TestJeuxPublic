using System.Collections.Generic;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Entities.LevelElements
{
	public class Level : Entity
    {
		public List<ZoneModel> Zones { get; set; }
		public List<TileZone> Tiles { get; set; }
		public List<ItemModel> Items { get; set; }
		public List<Decoration> Decorations { get; set; }
		public ShaderType Shader { get; set; }
		public Musics Music { get; set; }
		public GroundSprite DefaultTile { get; set; }


		public Level(int id)
            : base(id)
        {
            Zones = new List<ZoneModel>();
            Tiles = new List<TileZone>();
            Items = new List<ItemModel>();
            Decorations = new List<Decoration>();
        }

    }
}
