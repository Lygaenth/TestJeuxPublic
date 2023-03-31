using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TestJeux.SharedKernel.Enums;
using TestJeux.Business.Command;
using TestJeux.Business.Managers.API;

namespace TestJeux.Display.ViewModels.Display.Menu
{
	public class MainMenuViewModel : BindableBase
    {
        private readonly ISoundService _soundManager;
        int _selectedIndex = 0;

        private DateTime _lastMoveCursor;

        public string Background { get => Path.GetFullPath(@".\ressource\UI\MenuBackground.bmp"); }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public event EventHandler RequestGameLoad;
        public event EventHandler RequestGameQuit;

        #region ICommands
        public ICommand CmdStart { get; set; }
        public ICommand CmdContinue { get; set; }
        public ICommand CmdOptions { get; set; }
        public ICommand CmdQuit { get; set; }
        #endregion

        public MainMenuViewModel(ISoundService soundManager)
        {
            _soundManager = soundManager;

            CmdStart = new Command(ActionStart);
            CmdContinue = new Command(ActionContinue);
            CmdOptions = new Command(ActionOptions);
            CmdQuit = new Command(ActionQuit);

            MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("Start", CmdStart),
                new MenuItemViewModel("Continue", CmdContinue),
                new MenuItemViewModel("Options", CmdOptions),
                new MenuItemViewModel("Quit", CmdQuit)
            };

            MenuItems[_selectedIndex].IsSelected = true;
        }

        public void MoveCursor(DirectionEnum direction)
        {
            if (direction == DirectionEnum.Bottom)
                ChangeSelectedIndex(1);
            else if (direction == DirectionEnum.Top)
                ChangeSelectedIndex(-1);
        }

        private void ActionStart()
        {
            if (RequestGameLoad != null)
                RequestGameLoad(this, new EventArgs());
        }

        private void ActionContinue()
        {
            if (RequestGameLoad != null)
                RequestGameLoad(this, new EventArgs());
        }

        private void ActionOptions()
        {
            // To implement one day
        }

        private void ActionQuit()
        {
            _soundManager.Reset();
            Application.Current.Shutdown();
        }

        public void StartAction(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    ChangeSelectedIndex(-1);
                    break;
                case Key.Down:
                    ChangeSelectedIndex(1);
                    break;
                case Key.Space:
                case Key.Enter:
                    MenuItems[_selectedIndex].Command.Execute(null);
                    break;
            }
        }

        private void ChangeSelectedIndex(int indexOffset)
        {
            if ((DateTime.Now - _lastMoveCursor).TotalMilliseconds < 200)
                return;

            _lastMoveCursor = DateTime.Now;
            if (_selectedIndex == 0 && indexOffset < 0 || _selectedIndex == MenuItems.Count - 1 && indexOffset > 0)
                return;

            MenuItems[_selectedIndex].IsSelected = false;
            _selectedIndex += indexOffset;
            MenuItems[_selectedIndex].IsSelected = true;
            _soundManager.PlaySoundEffect(SoundEffects.MoveCursor);
        }
    }
}
