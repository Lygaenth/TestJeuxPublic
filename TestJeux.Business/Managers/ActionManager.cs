using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Business.Action;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers
{
	public class ActionManager : IActionManager
	{
		private readonly GameAggregate _game;
		private readonly IChatService _chatService;

		private List<ActionBase> _actions;
		private Thread _loopThread;
		private bool _interruptThread;
		private bool _terminatedThread;
		private bool _addingActions;
		private bool _waitForAcq;

		private ActionBase _bufferedAction;

		public ActionManager(GameAggregate gameAggregate, IChatService chatService)
		{
			_game = gameAggregate;
			_chatService = chatService;

			_actions = new List<ActionBase>();
			_terminatedThread = true;
		}

		public void AddAction(ActionBase action)
		{
			if (action.ActionType == ActionType.MovePj)
			{
				var currentActions = new List<ActionBase>();
				currentActions.AddRange(_actions.Where(a => a != null).ToList());
				if (!currentActions.Any(a => a.ActionType == ActionType.MovePj))
					_bufferedAction = action;
				return;
			}

			_actions.Add(action);
		}

		public void AddRangeActions(List<ActionBase> actions)
		{
			_addingActions = true;
			foreach (var action in actions)
			{
				if (action.ActionType == ActionType.MovePj && _actions.Count(a => a.ActionType == ActionType.MovePj) > 1)
					continue;

				_actions.Add(action);
			}
			_addingActions = false;
		}

		public void Start()
		{
			if (_loopThread != null && _loopThread.IsAlive)
				return;

			_actions.Clear();
			_interruptThread = false;
			_loopThread = new Thread(() => LoopActions());
			_loopThread.Start();
		}

		public void Stop()
		{
			_interruptThread = true;
			_actions.Clear();
			while (!_terminatedThread)
				Thread.Sleep(50);
		}

		public void Reset()
		{
			_actions.Clear();
		}

		public void Acq()
		{
			var acqThread = new Thread(() => _actions[0].Acq());
			acqThread.Start();
		}

		public bool IsActionBlocking()
		{
			return _actions.Any();
		}

		public bool IsActionWaitingForAquittement()
		{
			return _waitForAcq && _actions.Count > 0;
		}

		private void LoopActions()
		{
			while (!_interruptThread)
			{
				while (_addingActions)
					Thread.Sleep(10);

				while (_actions.Count > 0)
				{
					var firstAction = _actions[0];
					if (firstAction != null && firstAction.IsCompleted)
						_actions.Remove(firstAction);
					else
						break;
				}

				if (_actions.Count == 0)
				{
					if (_bufferedAction != null)
					{
						_actions.Add(_bufferedAction);
						_bufferedAction = null;
					}
				}

				if (_actions.Count != 0)
				{

					var action = _actions[0];
					if (action == null)
						continue;

					_waitForAcq = action.Execute();

					if (action.IsBlocking)
					{
						do
						{
							Thread.Sleep(50);
						}
						while (!action.IsCompleted);
					}
				}
			}
			_terminatedThread = true;
		}

		public void Subscribe()
		{
			foreach (var item in _game.GetItems())
				item.SpeakingRequested += OnSpeakingRequested;
		}

		public void Unsubscribe()
		{
			if (_game.GetCurrentLevel() == null)
				return;

			foreach (var item in _game.GetItems())
				item.SpeakingRequested -= OnSpeakingRequested;
		}

		private void OnSpeakingRequested(object sender, Core.ObjectValues.Speaker e)
		{
			_actions.Add(new SpeakAction(_chatService, e.Lines.Select(l => new SpeakerDto(e.Icon, e.Name, l)).ToList()));
		}
	}
}
