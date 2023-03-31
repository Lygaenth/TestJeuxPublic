using System;
using System.Collections.Generic;
using TestJeux.API;

namespace TestJeux.API.Services.TextInteractions
{
    public class DialogEventArgs : EventArgs
    {
        public SpeakerDto Speaker { get; private set; }

        public DialogEventArgs(SpeakerDto speaker)
        {
            Speaker = speaker;
        }
    }

    public delegate void DialogReady(object sender, DialogEventArgs args);
    public delegate void DialogSpeedUp(object sender, EventArgs args);
    public delegate void DialogEnd(object sender, EventArgs args);

    public interface IChatService : IService
    {
        bool IsTalking();

        void LoadDialog(List<SpeakerDto> speaker);

        bool Next();

        void NotifyDisplayCompleted();

        event DialogReady OnDialogToLoad;
        event DialogSpeedUp OnDialogSpeedUp;
        event DialogEnd OnDialogEnd;
    }
}