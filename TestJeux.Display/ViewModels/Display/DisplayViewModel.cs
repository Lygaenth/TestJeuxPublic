using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using TestJeux.Business.Entities.LevelElements;
using TestJeux.Display.ViewModels.Base;
using TestJeux.Display.ViewModels.Display.ChatBox;
using TestJeux.Display.ViewModels.Display.Levelelements;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.ViewModels.Display
{
	public class DisplayViewModel : BaseViewModel
    {
        #region attributes

        bool _menuVisibility;
        bool _gameVisibility;

        private bool _isLost;
        private bool _isWin;

        private bool _isInterrupted;

        private Thread _refreshThread;

        private DisplayTextViewModel _displayText;
        private ShaderViewModel _shader;
        private ItemViewModel _selectedCharacter;
        #endregion

        #region properties
        public bool IsLost
        {
            get { return _isLost; }
            set { SetProperty(ref _isLost, value); }
        }

        public bool IsWin
        {
            get { return _isWin; }
            set { SetProperty(ref _isWin, value); }
        }

        public bool DisplayGame
        {
            get { return _gameVisibility; }
            set { SetProperty(ref _gameVisibility, value); }
        }

        public bool DisplayMenu
        {
            get { return _menuVisibility; }
            set { SetProperty(ref _menuVisibility, value); }
        }

        public DisplayTextViewModel DisplayText
        {
            get { return _displayText; }
            set { SetProperty(ref _displayText, value); }
        }

        public ShaderViewModel Shader
        {
            get { return _shader; }
            set 
            {
                SetProperty(ref _shader, value);
            }
        }

        public ItemViewModel SelectedCharacter
        {
            get => _selectedCharacter;
            set => SetProperty(ref _selectedCharacter, value);  
        }

        public DecorationDisplayViewModel DecorationVm { get; set; }

        private LevelViewModel _level;
        public LevelViewModel LevelVm { get => _level; set => SetProperty(ref _level, value); }

        public ObservableCollection<ItemViewModel> Characters { get; set; }

        public ObservableCollection<ZoneViewModel> DebugZones { get; set; }

        private int _fps;
        public int FPS
        {
            get => _fps;
            set
            {
                SetProperty(ref _fps, value);
            }
        }

        private int _refreshTime;
        public int RefreshTime
        {
            get => _refreshTime;
			set
			{
				SetProperty(ref _refreshTime, value);
			}
		}

        #endregion

        public DisplayViewModel(DisplayTextViewModel displayTextVm, LevelViewModel levelVm, ShaderViewModel shaderVm)
        {
            LevelVm = levelVm;
            Characters = new ObservableCollection<ItemViewModel>();
            DebugZones = new ObservableCollection<ZoneViewModel>();
            DisplayGame = false;
            DisplayMenu = false;
            DisplayText = displayTextVm;
            Shader = shaderVm;

            _refreshThread = new Thread(() => RefreshThread());
        }

        #region public methods
        public void Start()
        {
            _isInterrupted = false;
            if (_refreshThread.IsAlive)
                return;

            foreach (var item in LevelVm.Items)
                item.Subscribe();

			_refreshThread = new Thread(() => RefreshThread());
            _refreshThread.Start();
        }

        public void Stop()
        {
			foreach (var item in LevelVm.Items)
				item.Unsubscribe();

			_isInterrupted = true;
        }

        public void DisplayView(ViewEnum view)
        {
            if (view == ViewEnum.Menu)
            {
                DisplayGame = false;
                DisplayText.DisplayTalking = false;
                DisplayMenu = true;
            }
            else
            {
                DisplayGame = true;
                DisplayMenu = false;
            }
        }

        public void UpdateCharacters(List<ItemViewModel> characters)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
				foreach (var character in Characters)
					character.Unsubscribe();

				Characters.Clear();
                foreach (var charac in characters)
                {
                    Characters.Add(charac);
                    charac.Subscribe();
                }
            });
        }
        #endregion

        #region privae methods
        private void RefreshThread()
        {
            DateTime _fpsCountStartTi = DateTime.Now;
            int fpscount = 0;
            while (!_isInterrupted)
            {
                var startTime = DateTime.Now;
                var characs = LevelVm.Items.OrderBy(c => c.Priority).ToList();
                ExecuteUithread(() =>
                {
                    foreach (var tile in LevelVm.Tiles)
                        if (tile.CanRefresh())
                            tile.Refresh();

					LevelVm.Items.Clear();
                    foreach (var item in characs)
                    {
                        item.RefreshSprite();
						LevelVm.Items.Add(item);
                    }
                    DisplayText.Refresh();

                    RefreshTime = (DateTime.Now - startTime).Milliseconds;
                });

                Thread.Sleep(Math.Max(10, 50 - RefreshTime));
                fpscount++;
                if ((DateTime.Now - _fpsCountStartTi).TotalMilliseconds > 1000)
                {
                    ExecuteUithread(() => FPS = fpscount);
                    _fpsCountStartTi = DateTime.Now;
                    fpscount = 0;
                }
            }
        }
        #endregion
    }
}
