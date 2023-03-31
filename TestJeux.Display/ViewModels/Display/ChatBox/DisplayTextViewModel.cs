using System;
using System.Threading;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Display.ViewModels.Base;

namespace TestJeux.Display.ViewModels.Display.ChatBox
{
    public class DisplayTextViewModel : BaseViewModel
    {
        private string _line;
        private string _partialLine;
        private Speaker _speaker;
        private IChatService _chatManager;
        private bool _talkingVisibility;
        private bool _oneshot;

        public bool DisplayTalking
        {
            get { return _talkingVisibility; }
            set { SetProperty(ref _talkingVisibility, value); }
        }

        public Speaker Speaker
        {
            get { return _speaker; }
            set { SetProperty(ref _speaker, value); }
        }

        public string Line
        {
            get { return _line; }
            set { SetProperty(ref _line, value); }
        }

        public bool Finished { get; set; }

        public DisplayTextViewModel(IChatService chatManager)
        {
            _chatManager = chatManager;
            _chatManager.OnDialogToLoad += OnDialogToDisplay;
            _chatManager.OnDialogSpeedUp += OnDialogSpeedUp;
            _chatManager.OnDialogEnd += OnDialogEnded;
            DisplayTalking = false;
            Line = "";
        }

        private void OnDialogSpeedUp(object sender, EventArgs args)
        {
            SpeedIt();
        }

        private void OnDialogEnded(object sender, EventArgs args)
        {
            ExecuteUithread(() =>
            {

                DisplayTalking = false;
                Line = "";
            });
        }

        private void OnDialogToDisplay(object sender, DialogEventArgs args)
        {
            ExecuteUithread(() => { DisplayTalking = true; });
            Display(args.Speaker);

        }

        public void Display(SpeakerDto speaker)
        {
            string libelle = "";

            ExecuteUithread(() =>
            {
                Finished = false;
                _oneshot = false;
                _partialLine = "";
                Line = "";
                DisplayTalking = true;
                Speaker = new Speaker(speaker);
            });

            for (int i = 0; i < speaker.Line.Length; i += 2)
            {
                ExecuteUithread(() =>
                {
                    if (_oneshot)
                    {
                        _partialLine = Speaker.Line;
                        i += speaker.Line.Length;
                    }
                    else
                    {
                        libelle += speaker.Line.Substring(i, Math.Min(2, speaker.Line.Length - i));
                        _partialLine = libelle;
                    }
                });
                Thread.Sleep(30);
            }
            _oneshot = false;
            _chatManager.NotifyDisplayCompleted();
            Finished = true;
        }

        public void Refresh()
        {
            if (DisplayTalking)
                Line = _partialLine;
        }

        public void HideText()
        {
            DisplayTalking = false;
        }

        public void SpeedIt()
        {
            _oneshot = true;
        }

        public bool IsTalking()
        {
            return _chatManager.IsTalking();
        }

        public void Reset()
        {
            _chatManager.Reset();
        }
    }
}
