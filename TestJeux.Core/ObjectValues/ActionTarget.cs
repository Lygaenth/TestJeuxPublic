using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.ObjectValues
{
	public class ActionTarget
    {
        /// <summary>
        /// Ground type under target
        /// </summary>
        public GroundType GroundType { get; set; }

        /// <summary>
        /// Interaction source ID
        /// </summary>
        public int SourceItemId { get; set; }

        /// <summary>
        /// Interaction target ID
        /// </summary>
        public int? TargetItemId { get; set; }

        /// <summary>
        /// Interaction direction
        /// </summary>
        public DirectionEnum Orientation { get; set; }
    }
}
