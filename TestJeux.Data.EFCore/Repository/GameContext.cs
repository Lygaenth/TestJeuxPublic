using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestJeux.API.Models;
using TestJeux.Data.Api;
using TestJeux.Data.EFCore.DbItems;
using TestJeux.Data.EFCore.Helper;

namespace TestJeux.Core.Context
{
	public class GameContext : DbContext, IDALLevels
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Decoration> Decorations { get; set; }
        public DbSet<Tile> Tiles { get; set; }

		const string connectionString = "Host=localhost:5432; Database=TestGame; Username=postgres; Password=test";

		public GameContext()
            : base()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(connectionString);
        }

        public LevelDto GetDataById(int id)
        {
            var levels = Levels.Include(l => l.Zones).Include(l => l.Items).Include(l => l.Decorations).Include(l => l.Tiles);
			return levels.FirstOrDefault(l => l.LevelId == id).Convert();
        }

        public List<LevelDto> LoadAllLevels()
        {
            return Levels.Include(l => l.Zones).Include(l => l.Items).Include(l => l.Decorations).Include(l => l.Tiles).Select(l => l.Convert()).ToList();
        }

        public void SaveLevel(LevelDto level)
        {

            var updatedLevel = Levels.Include(l => l.Zones).Include(l => l.Items).Include(l => l.Decorations).Include(l => l.Tiles).FirstOrDefault(l => l.LevelId == level.ID);
            if (updatedLevel != null)
            {
                updatedLevel.Update(level);
                //Zones.UpdateRange(level.Zones.Select(z => z.Convert()));
                //Decorations.UpdateRange(level.Decorations.Select(c => c.Convert()));
                //Tiles.UpdateRange(level.TilesZones.Select(z => z.Convert()));   
            }
            else
            {
                var newLevel = new Level();
                newLevel.Update(level);
                Levels.Add(newLevel);
            }
			ChangeTracker.DetectChanges();
			SaveChanges();
        }
    }
}
