using System;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Entities;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Action
{
	public class SoundAction : ActionBase
    {
        private ISoundService _soundManager;
        private SoundEffects _soundEffect;
        bool _waitForEnd;

        public override bool IsBlocking => _waitForEnd;

        public SoundAction(ISoundService soundManager, SoundEffects soundEffect, bool waitForEnd)
        {
            _soundManager = soundManager;
            _soundEffect = soundEffect;
            _waitForEnd = waitForEnd;
        }

        public override bool Acq()
        {
            return false;
        }

        public override bool Execute()
        {
            _soundManager.PlaySoundEffect(_soundEffect);
            if (!_waitForEnd)
                IsCompleted = true;
            else
                _soundManager.SoundEffectEnded += OnSoundEffectEnded;
            return false;
        }

        private void OnSoundEffectEnded(object sender, EventArgs e)
        {
            _soundManager.SoundEffectEnded -= OnSoundEffectEnded;
            IsCompleted = true;
        }
    }
}
