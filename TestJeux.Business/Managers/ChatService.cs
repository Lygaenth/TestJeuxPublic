using System;
using System.Collections.Generic;
using System.Linq;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Core.Aggregates;

namespace TestJeux.Business.Managers
{
	public class ChatService : IChatService
    {
        private readonly GameAggregate _game;
        private bool _isTalking;
        private int _currentDialogStep;
        private List<SpeakerDto> _currentDialog;
        private bool _currentDialogDisplaydEnded;
        public event DialogReady OnDialogToLoad;
        public event DialogSpeedUp OnDialogSpeedUp;
        public event DialogEnd OnDialogEnd;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public ChatService(GameAggregate game)
        {
            _game = game;
            _isTalking = false;
            _currentDialog = new List<SpeakerDto>();
        }

        /// <summary>
        /// Chatting event is ongoing 
        /// </summary>
        /// <returns></returns>
        public bool IsTalking()
        {
            return _isTalking;
        }

        public void Reset()
        {
			_isTalking = false;
            _currentDialog.Clear();
        }

        public void LoadDialog(List<SpeakerDto> dialogLines)
        {
            _isTalking = true;
            _currentDialog.AddRange(dialogLines);
            _currentDialogStep = 0;
            NotifyNewDialogLine();
            _currentDialogStep++;
        }

        /// <summary>
        /// Load next line
        /// </summary>
        /// <returns></returns>
        public bool Next()
        {
            if (!_currentDialogDisplaydEnded)
            {
                if (OnDialogSpeedUp != null)
                    OnDialogSpeedUp(this, new EventArgs());
                return false;
            }

            if (_currentDialogStep < _currentDialog.Count)
            {
                NotifyNewDialogLine();
                _currentDialogStep++;
                return false;
            }

            _isTalking = false;
            if (OnDialogEnd != null)
                OnDialogEnd(this, new EventArgs());

            _currentDialog.Clear();
            return true;
        }

        private void NotifyNewDialogLine()
        {
            if (OnDialogToLoad != null)
                OnDialogToLoad(this, new DialogEventArgs(_currentDialog[_currentDialogStep]));
        }

        public void NotifyDisplayCompleted()
        {
            _currentDialogDisplaydEnded = true;
        }

	}
}
