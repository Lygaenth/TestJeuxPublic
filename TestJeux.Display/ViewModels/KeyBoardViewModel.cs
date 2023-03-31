using System;
using System.Windows.Input;
using TestJeux.SharedKernel.Enums;
using TestJeux.API.Events;
using TestJeux.Display.Controllers;
using TestJeux.Display.ViewModels.Display.Menu;

namespace TestJeux.Display.ViewModels
{
    public class KeyBoardViewModel
    {
        private IControllerService _controllerManager;

        MainMenuViewModel _mainMenuViewModel;
        ViewEnum _currentView;

        public KeyBoardViewModel(IControllerService controllerManager, MainMenuViewModel mainMenuViewModel)
        {
            _controllerManager = controllerManager;
            _mainMenuViewModel = mainMenuViewModel;

            _controllerManager.ActionPushed += OnActionPushed;
            _controllerManager.NextItemPushed += OnNextItemPushed;
        }

        private void OnNextItemPushed(object sender, EventArgs e)
        {
            if (RequestSwitchEquipment != null)
                RequestSwitchEquipment(sender, e);
        }

        private void OnActionPushed(object sender, EventArgs e)
        {
             if (RequestAction != null)
                RequestAction(sender, e);
        }

        public void SetView(ViewEnum view)
        {
            _currentView = view;
        }

        public void KeyDown(Key key)
        {
            if (_currentView == ViewEnum.Menu)
            {
                _mainMenuViewModel.StartAction(key);
                return;
            }

            if (_currentView == ViewEnum.Game)
            {
                var act = MapKeyToControlAction(key);
				switch (act)
                {
                    case ControlAction.Down:
                    case ControlAction.Up:
                    case ControlAction.Left:
                    case ControlAction.Right:
                        _controllerManager.RegisterKeyDown(act);
                        break;
                    case ControlAction.Interact:
                        if (RequestAction != null)
                            RequestAction(this, new EventArgs());
                        break;
                    case ControlAction.Switch:
                        if (RequestSwitchEquipment != null)
                            RequestSwitchEquipment(this, new EventArgs());
                        break;
                    case ControlAction.Escape:
                        if (RequestSwitchView != null)
                            RequestSwitchView(this, ViewEnum.Menu);
                        break;
                    default:
                        break;
                }
            }
        }

        public void KeyUp(Key key)
        {
            var act = MapKeyToControlAction(key);
            switch (act)
            {
                case ControlAction.Down:
                case ControlAction.Up:
                case ControlAction.Left:
                case ControlAction.Right:
                    _controllerManager.RegisterKeyUp(act);
                    break;
                default:
                    break;
            }
        }

        private ControlAction MapKeyToControlAction(Key key)
        {
            // Can be made modulable
            return key switch
            {
                Key.Left => ControlAction.Left,
                Key.Right => ControlAction.Right,
                Key.Up => ControlAction.Up,
                Key.Down => ControlAction.Down,
                Key.Tab => ControlAction.Switch,
                Key.Space => ControlAction.Interact,
                Key.Escape => ControlAction.Escape,
                _ => ControlAction.None
            };
        }

        public event EventHandler RequestAction;

        public event EventHandler RequestSwitchEquipment;

        public event ViewEventHandler RequestSwitchView;


    }
}
