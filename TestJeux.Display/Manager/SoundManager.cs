using System;
using System.Windows;
using System.Windows.Media;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.ObjectValues;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.Managers
{
	public class SoundManager : ISoundService
	{
		// TODO inject a wrapper for WPF media player later and make a proper business API
		private GameAggregate _game;
		private readonly MediaPlayer _musicPlayer;
		private readonly MediaPlayer _soundEffectPlayer;
		private Musics _currentMusic;

		private double _currentMusicVolume;
		private double _lastAppliedValue;

		public event EventHandler<EventArgs> SoundEffectEnded;

		/// <summary>
		/// Constructor
		/// </summary>
		public SoundManager(GameAggregate game)
		{
			_game = game;
			_musicPlayer = new MediaPlayer();
			_musicPlayer.Volume = 0;
			_soundEffectPlayer = new MediaPlayer();
			_soundEffectPlayer.Volume = 1;
			_currentMusic = Musics.None;
			_currentMusicVolume = 1;
			_lastAppliedValue = 1;
		}

		public void PlayBackGroundMusic(Musics music)
		{
			if (_currentMusic == music)
				return;

			Application.Current.Dispatcher.Invoke(() =>
			{
				if (_musicPlayer.BufferingProgress > 0)
				{
					ChangeMusicVolume(100, 0);
					_musicPlayer.Stop();
				}
				_currentMusic = music;
				StartNewMusic();
			});
		}

		public void PlaySoundEffect(SoundEffects soundEffect)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				_soundEffectPlayer.MediaEnded += OnSoundEffectEnded;
				_soundEffectPlayer.Open(GetSoundPath(soundEffect));
				_soundEffectPlayer.Play();
			});
		}

		private void OnSoundEffectEnded(object sender, EventArgs e)
		{
			_soundEffectPlayer.MediaEnded -= OnSoundEffectEnded;
			_soundEffectPlayer.Pause();
			if (SoundEffectEnded != null)
				SoundEffectEnded(this, EventArgs.Empty);
		}

		private void _soundPlayer_MediaEnded(object sender, EventArgs e)
		{
			PlayMusic();
		}

		public void StartNewMusic()
		{

			PlayMusic();
			ChangeMusicVolume(1000, _currentMusicVolume);
		}

		private void PlayMusic()
		{
			_musicPlayer.MediaEnded -= _soundPlayer_MediaEnded;
			_musicPlayer.Open(GetMusicPath(_currentMusic));
			_musicPlayer.Play();
			_musicPlayer.MediaEnded += _soundPlayer_MediaEnded;
		}

		public void Reset()
		{
			_musicPlayer.Stop();
			_soundEffectPlayer.Stop();
		}

		private void ChangeMusicVolume(int timeSpan, double volume)
		{
			double targetVolume = volume;
			if (targetVolume < 0)
				targetVolume = _currentMusicVolume;
			if (timeSpan >= 0)
			{
				SetPlayerVolume(_musicPlayer, targetVolume);
				return;
			}

			//var task = new Task(() =>
			//{
			//var currentVolume = _lastAppliedValue;
			//if (targetVolume == currentVolume)
			//    return;
			//for (int i = 0; i < 10; i++)
			//{
			//    Thread.Sleep(timeSpan / 10);
			//    SetPlayerVolume(_musicPlayer, (targetVolume - currentVolume) * i / 10 + currentVolume);
			//}
			//_lastAppliedValue = targetVolume;
			//    return;
			//});
			//task.Start();
			//Task.WaitAny(task);
		}

		private void SetPlayerVolume(MediaPlayer mediaPlayer, double volume)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				mediaPlayer.Volume = volume;
			});
		}

		public void SetMusicVolume(int volume)
		{
			_currentMusicVolume = (double)volume / 100;
			ChangeMusicVolume(1000, _currentMusicVolume);
		}

		private Uri GetMusicPath(Musics music)
		{
			switch (music)
			{
				case Musics.Menu:
					return new Uri(@"D:\Musique\TestmusiqueMenu.mp3");
				case Musics.LevelEasy:
					return new Uri(@"D:\Musique\TestmusiqueJEux.mp3");
				case Musics.Death:
					return new Uri(@"D:\Musique\TestmusiqueDeath.mp3");
				case Musics.Cave:
					return new Uri(@"D:\Musique\TestmusiqueCave.mp3");
			}
			return null;
		}

		private Uri GetSoundPath(SoundEffects effect)
		{
			return effect switch
			{
				SoundEffects.SwitchScreen => new Uri(@"D:\Musique\TestSoundEffectJEux.mp3"),
				SoundEffects.MoveCursor => new Uri(@"D:\Musique\CursorMove.mp3"),
				SoundEffects.PushBox => new Uri(@"D:\Musique\TestSoundEffectPushBox.mp3"),
				SoundEffects.OpenChest => new Uri(@"D:\Musique\TestSoundEffectOpenChest.mp3"),
				SoundEffects.Mining => new Uri(@"D:\Musique\TestSoundEffectMining.mp3"),
				_ => null
			};
		}

		public void Subscribe()
		{
			foreach(var item in _game.GetItems())
				item.MadeSound += OnSoundMade;
		}

		public void Unsubscribe()
		{
			if (_game.GetCurrentLevel() == null)
				return;

			foreach (var item in _game.GetItems())
				item.MadeSound -= OnSoundMade;
		}

		private void OnSoundMade(object? sender, MakeSound e)
		{
			PlaySoundEffect(e.Sound);
		}
	}
}
