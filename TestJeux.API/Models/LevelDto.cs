using System.Collections.Generic;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	[XmlRootAttribute("Level", IsNullable = false)]
    public class LevelDto
    {
        public LevelDto()
        {
            Zones = new List<ZoneDto>();
            Items = new List<ItemDto>();
            TilesZones = new List<TileZoneDto>();
            Decorations = new List<DecorationDto>();
            ItemsIDs = new List<int>();
        }

        [XmlElement]
        public int ID { get; set; }
        [XmlElement]
        public ShaderType Shader { get; set; }
        [XmlElement]
        public Musics LevelMusic { get; set; }

        [XmlArrayAttribute("Zones")]
        public List<ZoneDto> Zones { get; set; }

        [XmlArrayAttribute("Items")]
        public List<ItemDto> Items { get; set; }

        [XmlArrayAttribute("Ground")]
        public List<TileZoneDto> TilesZones { get; set; }

        [XmlAttribute("DefaultTile", DataType ="int", Namespace ="Ground")]
        public int DefaultTile { get; set; }

        [XmlArrayAttribute("Decorations")]
        public List<DecorationDto> Decorations { get; set; }

        [XmlIgnore]
        public List<int> ItemsIDs { get; set; }
    }
}