using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.ObjectValues
{
	public class Reaction
    {
        public Reactions ReactionType { get; }
        public int ScriptId { get; }

        public Reaction(Reactions reactionType, int scriptId)
        {
            ReactionType = reactionType;
            ScriptId = scriptId;
        }
    }
}
