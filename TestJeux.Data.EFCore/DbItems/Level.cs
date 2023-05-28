using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Data.EFCore.DbItems
{
	public class Level
	{
		public int LevelId { get; set; }
		public string Name { get; set; }
		public ShaderType Shader { get; set; }
		public Musics Music { get; set; }
		public List<Zone> Zones { get; set; }
		public List<Decoration> Decorations { get; set; }
		public List<Item> Items { get; set; }
		public List<Tile> Tiles { get; set; }

		public Level()
		{
			Name = "";
			Zones = new List<Zone>();
			Decorations = new List<Decoration>();
			Items = new List<Item>();
			Tiles = new List<Tile>();
		}

		public void Update(LevelDto level)
		{
			Name = "Level" + LevelId;
			Shader = level.Shader;
			Music = level.LevelMusic;

			// Update Zones
			foreach(var zone in Zones.Where(z => !level.Zones.Any(t => t.ID == z.ZoneId)).ToList())
				Zones.Remove(zone);

			foreach (var zone in Zones.Where(z => level.Zones.Any(t => t.ID == z.ZoneId)).ToList())
				zone.Update(level.Zones.First(z => z.ID == zone.ZoneId));

			foreach (var zone in level.Zones.Where(z => z.ID == 0))
				Zones.Add(new Zone() { GroundType = zone.GroundType, X1 = zone.TopLeft.X, Y1 = zone.TopLeft.Y, X2 = zone.BottomRight.X, Y2 = zone.BottomRight.Y });

			// Update Decorations
			foreach(var decoration in Decorations.Where(z => !level.Decorations.Any(d => d.ID == z.DecorationId)).ToList())
				Decorations.Remove(decoration);
			foreach (var decoration in Decorations.Where(z => level.Decorations.Any(d => d.ID == z.DecorationId)).ToList())
				decoration.Update(level.Decorations.FirstOrDefault(d => d.ID == decoration.DecorationId));
			foreach (var decoration in level.Decorations.Where(d => d.ID == 0))
				Decorations.Add(new Decoration() { Type = decoration.Decoration, X = decoration.TopLeft.X, Y = decoration.TopLeft.Y });

			// Update Tiles
			foreach (var tile in Tiles.Where(t => !level.TilesZones.Any(tDto => tDto.ID == t.TileId)).ToList())
				Tiles.Remove(tile);
			foreach (var tile in Tiles.Where(t => level.TilesZones.Any(tDto => tDto.ID == t.TileId)).ToList())
				tile.Update(level.TilesZones.First(d => d.ID == tile.TileId));
			foreach (var tile in level.TilesZones.Where(t => t.ID == 0))
				Tiles.Add(new Tile() { Type = tile.Tile, X1 = tile.TopLeft.X, Y1 = tile.TopLeft.Y, X2 = tile.BottomRight.X, Y2 = tile.BottomRight.Y, Angle = tile.Angle });

			// Update Items
			foreach (var item in Items.Where(item => !level.Items.Any(i => item.ItemId == i.ID)).ToList())
				Items.Remove(item);
			foreach (var item in Items.Where(item => level.Items.Any(i => item.ItemId == i.ID)).ToList())
				item.Update(level.Items.First(d => d.ID == item.ItemId));
			foreach (var item in level.Items.Where(item => item.ID == 0))
				Items.Add(new Item() { Code = item.Code, X = item.StartPosition.X, Y = item.StartPosition.Y, Orientation = item.Orientation , DefaultState = item.DefaultState });
		}


	}
}
