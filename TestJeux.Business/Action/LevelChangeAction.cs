using TestJeux.Business.Services.API;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Entities.Action
{
	public class LevelChangeAction : ActionBase
    {
        ILevelService _levelManager;
        public override ActionType ActionType => ActionType.LevelChange;

        public override bool IsBlocking => true;

        private readonly int _targetLevel;

        public LevelChangeAction(ILevelService levelManager, int id)
        {
            _levelManager = levelManager;
            _targetLevel = id;
        }

        public override bool Acq()
        {
            return true;
        }

        public override bool Execute()
        {
            // To do get ID next level from position of Point on level through manager
            var currentLevelId = _levelManager.GetCurrentLevel();
            _levelManager.ChangeLevel(_targetLevel);

            IsCompleted = true;
            return false;
        }
    }
}
