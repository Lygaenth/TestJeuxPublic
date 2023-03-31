using System.Drawing;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	[XmlType("Decoration")]
    public class DecorationDto
    {
        private Point _topLeft;

        [XmlAttribute("ID")]
        public Decorations Decoration { get; set; }

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

        [XmlAttribute]
        public string P1 { get; set; }
    }
}
