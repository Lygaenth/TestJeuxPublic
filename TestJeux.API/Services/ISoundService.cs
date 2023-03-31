using System;
using TestJeux.API.Services;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Business.Managers.API
{
	public interface ISoundService : ISubscribingService
    {
        event EventHandler<EventArgs> SoundEffectEnded;

        void PlayBackGroundMusic(Musics music);
        void PlaySoundEffect(SoundEffects soundEffect);
        void SetMusicVolume(int volume);
        void StartNewMusic();
    }
}