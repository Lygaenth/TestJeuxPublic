using System.Drawing;
using System.Xml.Serialization;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	/// <summary>
	/// Item DTO
	/// </summary>
	[XmlType("Item")]
    public class ItemDto
    {
        /// <summary>
        /// ID
        /// </summary>
        [XmlAttribute]
        public int ID { get; set; }

        /// <summary>
        /// Item type code
        /// </summary>
        [XmlAttribute("Code")]
        public ItemCode Code { get; set; }

        /// <summary>
        /// Start position of the item in level
        /// </summary>
        public Point StartPosition { get; set; }

        /// <summary>
        /// Item Orientation
        /// </summary>
        [XmlAttribute("Orientation")]
        public DirectionEnum Orientation { get; set; }

        /// <summary>
        /// Default state of the item
        /// </summary>
        [XmlAttribute("State")]
        public int DefaultState { get; set; }
    }
}
