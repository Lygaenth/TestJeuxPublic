using System.Drawing;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Models
{
	/// <summary>
	/// Item DTO
	/// </summary>
	public class ItemStateDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Item type code
        /// </summary>
        public ItemCode Code { get; set; }

        /// <summary>
        /// Start position of the item in level
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Item Orientation
        /// </summary>
        public DirectionEnum Orientation { get; set; }

    }
}
