using System.Collections.Generic;
using System.Linq;
using TestJeux.Business.Entities.Items;
using TestJeux.Core.Entities;
using TestJeux.Core.Entities.LevelElements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.LevelElements
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

        public override void Copy(Entity entity)
        {
            if (!(entity is Level level))
                return;

            Shader = level.Shader;
            Music = level.Music;
            DefaultTile = level.DefaultTile;

            // Zones
            CopyElement(Zones.Select(t => t as Entity).ToList(), level.Zones.Select(t => t as Entity).ToList());
            CopyElement(Tiles.Select(t => t as Entity).ToList(), level.Tiles.Select(t => t as Entity).ToList());
            CopyElement(Decorations.Select(t => t as Entity).ToList(), level.Decorations.Select(t => t as Entity).ToList());
            // Not updating copy of items yet
        }

        private void CopyElement(List<Entity> entities, List<Entity> copiedEntities)
        {
            entities.RemoveAll(nt => !copiedEntities.Any(t => t.ID == nt.ID));
            foreach (var entity in entities.Where(nz => copiedEntities.Any(t => t.ID == nz.ID)))
                entity.Copy(copiedEntities.Find(t => t.ID == entity.ID));
            entities.AddRange(copiedEntities.Where(nt => entities.Any(t => t.ID == nt.ID)));
        }
    }
}
