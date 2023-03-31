using System;

namespace TestJeux.API.Models.ItemTypeInterface
{
    public class StatsDto
    {
        int Attack { get; set; }
        int Defense { get; set; }
        int CurrentHP { get; set; }
        int HPMax { get; set; }
        int Magic { get; set; }
        int Move { get; set; }
        int CurrentMP { get; set; }
        int MPMax { get; set; }
        int Strength { get; set; }
        int Sanity { get; set; }
        int SanityMax { get; set; }
    }
}