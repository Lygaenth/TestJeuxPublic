using System;
using System.Threading;
using SharpDX.XInput;
using TestJeux.API.Events;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.Controllers
{
	public class Xboxcontroller
    {
        private const int deadBand = 2500;

        private Controller _controller;
        private bool _interrupt;

        public event MoveEventHandler MoveRaised;
        public event EventHandler ActionPushed;
        public event EventHandler NextItemPushed;

        Thread _scanThread;

        public Xboxcontroller() 
        {
            _controller = new Controller(UserIndex.One);
        }

        bool _aButtonOn;
        bool _bButtonOn;

        public void Start()
        {
            _interrupt = false;
            _scanThread = new Thread(() =>
            {
                while (!_interrupt)
                {
                    ScanState();
                    Thread.Sleep(30);
                }
            });
            _scanThread.Start();
        }

        public void Stop()
        {
            _interrupt = false;
        }

        protected void ScanState()
        {
            if (!_controller.IsConnected)
                return;

            var state = _controller.GetState();

            var leftThumbX = state.Gamepad.LeftThumbX;
            var leftThumbY = state.Gamepad.LeftThumbY;

            var x = Math.Abs((float)leftThumbX) < deadBand ? 0 : (float)leftThumbX / short.MinValue * -100;
            var y = Math.Abs((float)leftThumbY) < deadBand ? 0 : (float)leftThumbY / short.MinValue * -100;

            if (MoveRaised!= null && (x != 0 || y != 0))
            {
                if (Math.Abs(x) > Math.Abs(y))
                {
                    if (x < 0 )
                        MoveRaised(this, DirectionEnum.Left);
                    else
                        MoveRaised(this, DirectionEnum.Right);
                }
                else
                {
                    if (y > 0)
                        MoveRaised(this, DirectionEnum.Top);
                    else
                        MoveRaised(this, DirectionEnum.Bottom);
                }
            }

            if((state.Gamepad.Buttons & GamepadButtonFlags.A) > 0)
            {
                if(!_aButtonOn)
                {
                    _aButtonOn = true;
                    if (ActionPushed != null)
                        ActionPushed(this, new EventArgs());
                }
            }
            else
                _aButtonOn = false;

            if ((state.Gamepad.Buttons & GamepadButtonFlags.B) > 0)
            {
                if (!_bButtonOn)
                {
                    _bButtonOn = true;
                    if (NextItemPushed != null)
                        NextItemPushed(this, new EventArgs());
                }
            }
            else
                _bButtonOn = false;
        }

    }
}
