using System;
using System.Threading;
using System.Threading.Tasks;
using TestJeux.API.Events;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.Controllers
{
	public class KeyBoardController
    {
		ControlAction _lastInput;

        Thread _moveStateThread;
        bool _interrupt;

        public event MoveEventHandler MoveRaised;
        public event EventHandler ActionPushed;
        public event EventHandler NextItemPushed;

        public KeyBoardController()
        {
            _lastInput = ControlAction.None;
            _moveStateThread = new Thread(ScanState);
            _interrupt = false;
        }

        public void Start()
        {
            _lastInput = ControlAction.None;
            _interrupt = false;
            _moveStateThread = new Thread(ScanState);
            _moveStateThread.Start();
        }

        public void Stop()
        {
            _interrupt = true;
        }

        public bool RegisterKeyDown(ControlAction key)
        {
            switch (key)
            {
                case ControlAction.Up:
                case ControlAction.Down:
                case ControlAction.Left:
                case ControlAction.Right:
                    _lastInput = key;
                    return true;
                default:
                    return false;
            }
        }

        public void RegisterKeyUp(ControlAction key)
        {
            if (key == _lastInput)
                _lastInput = ControlAction.None;
        }

        protected void ScanState()
        {
            while (!_interrupt)
            {
                Thread.Sleep(50);
                Task.Run(() =>
                {
                    if (_lastInput != ControlAction.None && MoveRaised != null)
                        MoveRaised(this, ConvertKeyToDirection(_lastInput));
                });
            }
        }

        private DirectionEnum ConvertKeyToDirection(ControlAction key)
        {
            return key switch
            {
				ControlAction.Left => DirectionEnum.Left,
				ControlAction.Right => DirectionEnum.Right,
				ControlAction.Up => DirectionEnum.Top,
				ControlAction.Down => DirectionEnum.Bottom,
                _ => throw new NotImplementedException()
            };
        }
    }
}
