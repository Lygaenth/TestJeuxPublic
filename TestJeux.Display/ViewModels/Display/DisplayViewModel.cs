using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
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
            _refreshThread = new Thread(() => RefreshThread());
            _refreshThread.Start();
        }

        public void Stop()
        {
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
                Characters.Clear();
                foreach (var charac in characters)
                    Characters.Add(charac);
            });
        }
        #endregion

        #region privae methods
        private void RefreshThread()
        {
            while (!_isInterrupted)
            {
                var characs = LevelVm.Items.OrderBy(c => c.Priority).ToList();
                ExecuteUithread(() =>
                {
                    foreach (var tile in LevelVm.Tiles)
                        tile.Refresh();

					LevelVm.Items.Clear();
                    foreach (var item in characs)
                    {
                        item.RefreshSprite();
						LevelVm.Items.Add(item);
                    }
                    DisplayText.Refresh();

                    //if (Shader.Sources.Count != Characters.Select(c => c.IsLightSource).Count())
                    //    Shader = new ShaderViewModel(Shader.ShaderType, Characters.ToList());

                });

                Thread.Sleep(50);
            }
        }
        #endregion
    }
}
