using System.Collections.Generic;
using TestJeux.API.Services;
using TestJeux.Core.Entities;

namespace TestJeux.Business.Managers.API
{
	public interface IActionManager : ISubscribingService
    {
        void Acq();
        void AddAction(ActionBase action);
        void AddRangeActions(List<ActionBase> actions);
        bool IsActionBlocking();
        bool IsActionWaitingForAquittement();
        void Start();
        void Stop();
    }
}