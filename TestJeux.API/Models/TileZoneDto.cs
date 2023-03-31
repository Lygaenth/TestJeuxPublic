using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	[XmlType("Sprite")]
    public class TileZoneDto
    {
        private Point _topLeft;
        private Point _bottomRight;

        [XmlAttribute("ID")]
        public GroundSprite Tile { get; set; }

        [XmlIgnore]
        public Point TopLeft
        {
            get => _topLeft;
            set
            {
                _topLeft = value;
                P1 = _topLeft.X + ";" + _topLeft.Y;
            }
        }

        [XmlIgnore]
        public Point BottomRight
        {
            get => _bottomRight;
            set
            {
                _bottomRight = value;
                P2 = _bottomRight.X + ";" + _bottomRight.Y;
            }
        }


        [XmlAttribute]
        public int Angle { get; set; }

        [XmlAttribute("P1")]
        public string P1 { get; set; }

        [XmlAttribute("P2")]
        public string P2 { get; set; }

        [XmlIgnore]
        public List<string> SpriteCodes { get; set; }
    }
}
