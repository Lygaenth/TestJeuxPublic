using TestJeux.Core.Entities;

namespace TestJeux.Business.Action
{
    public class ActionChangeState : ActionBase
    {
        private readonly System.Action _action;

        public ActionChangeState(System.Action action)
        {
            _action = action;
        }

        public override bool Acq()
        {
            return false;
        }

        public override bool Execute()
        {
            _action.Invoke();
            IsCompleted = true;
            return false;
        }
    }
}
