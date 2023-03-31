using TestJeux.SharedKernel.Enums;

namespace TestJeux.API.Action
{
	public class ActionDto
    {
        /// <summary>
        /// Action type
        /// </summary>
        ActionType ActionType { get; }

        /// <summary>
        /// Prevents from passing to next action
        /// </summary>
        bool IsBlocking { get; }

        /// <summary>
        /// Action is completed or not
        /// </summary>
        bool IsCompleted { get; set; }
    }
}
