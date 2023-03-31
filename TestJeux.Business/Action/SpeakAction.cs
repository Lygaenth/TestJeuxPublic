using System.Collections.Generic;
using System.Threading;
using TestJeux.API.Services.TextInteractions;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class SpeakAction : ActionBase
    {
        IChatService _chatManager;
        public override ActionType ActionType { get => ActionType.Speak; }
        public List<SpeakerDto> TextLines { get; set; }

        public override bool IsBlocking => true;

        public SpeakerDto SpeakerViewModel { get; set; }

        public SpeakAction(IChatService chatManager, List<SpeakerDto> textLines)
        {
            _chatManager = chatManager;
            TextLines = textLines;
        }

        public override bool Execute()
        {
            var speakThread = new Thread(() => _chatManager.LoadDialog(TextLines));
            speakThread.Start();
            IsCompleted = false;
            return true;
        }

        public override bool Acq()
        {
            var done = _chatManager.Next();
            if (done)
                IsCompleted = true;
            return done;
        }
    }
}
