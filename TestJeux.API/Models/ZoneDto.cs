using System.Drawing;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	[XmlType("Zone")]
    public class ZoneDto
    {
        private Point _bottomRight;
        private Point _topLeft;



        [XmlAttribute]
        public int ID { get; set; }

        [XmlIgnore]
        public Point BottomRight
        {
            get => _bottomRight;
            set
            {
                _bottomRight = value;
                P2 = BottomRight.X + ";" + BottomRight.Y;
            } 
        }

        [XmlIgnore]
        public Point TopLeft
        {
            get => _topLeft;
            set
            {
                _topLeft = value;
                P1 = TopLeft.X + ";" + TopLeft.Y; 
            }
        }

        [XmlAttribute]
        public string P1 { get; set; }
        [XmlAttribute]
        public string P2 { get; set; }
        [XmlAttribute]
        public GroundType GroundType { get; set; }

		public ZoneDto()
		{

		}

		public ZoneDto(int id, Point bottomRight, Point topLeft, GroundType groundType)
		{
			ID = id;
			BottomRight = bottomRight;
			TopLeft = topLeft;
			GroundType = groundType;
		}
	}
}