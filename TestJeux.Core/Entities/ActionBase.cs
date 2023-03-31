using TestJeux.Core.Entities.Items;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Core.Entities
{
	public abstract class ActionBase
    {
        /// <summary>
        /// Action type
        /// </summary>
        public virtual ActionType ActionType { get; }

        /// <summary>
        /// Source of action
        /// </summary>
        public ItemModel Source { get; set; }

        /// <summary>
        /// Prevents from passing to next action
        /// </summary>
        public virtual bool IsBlocking { get; }

        /// <summary>
        /// Action is completed or not
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Acq current state
        /// </summary>
        /// <returns></returns>
        public abstract bool Acq();

        /// <summary>
        /// Execute action
        /// </summary>
        /// <returns></returns>
        public abstract bool Execute();
    }
}
