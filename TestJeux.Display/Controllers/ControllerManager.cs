using System;
using TestJeux.API.Events;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.Controllers
{
	/// <summary>
	/// Controller service for keyboard and xbox controller input
	/// </summary>
	public class ControllerService : IControllerService
	{
		// TODO create a manger for wrapping keyboard and xbox controller
		private readonly KeyBoardController _keyBoard;
		private readonly Xboxcontroller _xboxcontroller;

		public event MoveEventHandler MoveRaised;
		public event EventHandler ActionPushed;
		public event EventHandler NextItemPushed;

		/// <summary>
		/// Constructor
		/// </summary>
		public ControllerService()
		{
			_keyBoard = new KeyBoardController();
			_xboxcontroller = new Xboxcontroller();
		}

		/// <summary>
		/// Start checking inputs
		/// </summary>
		public void Start()
		{
			Subscribe();
			_keyBoard.Start();
			_xboxcontroller.Start();
		}

		/// <summary>
		/// Stop checking inputs
		/// </summary>
		public void Stop()
		{
			Unsubscribe();
			_keyBoard.Stop();
			_xboxcontroller.Stop();
		}

		/// <summary>
		/// On action received from controllers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnActionPushed(object sender, EventArgs e)
		{
			if (ActionPushed != null)
				ActionPushed(this, e);
		}

		/// <summary>
		/// On move received from controllers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="direction"></param>
		private void OnMovedRaisedKey(object sender, DirectionEnum direction)
		{
			if (MoveRaised != null)
				MoveRaised(this, direction);
		}

		/// <summary>
		/// Register current key down on hold if it's a move
		/// </summary>
		/// <param name="key"></param>
		public void RegisterKeyDown(ControlAction key)
		{
			_keyBoard.RegisterKeyDown(key);
		}

		/// <summary>
		/// Register current key up on hold if it's a move
		/// </summary>
		/// <param name="key"></param>
		public void RegisterKeyUp(ControlAction key)
		{
			_keyBoard.RegisterKeyUp(key);
		}

		/// <summary>
		/// Subscribe to controllers event
		/// </summary>
		private void Subscribe()
		{
			_keyBoard.MoveRaised += OnMovedRaisedKey;
			_xboxcontroller.MoveRaised += OnMovedRaisedKey;

			_keyBoard.ActionPushed += OnActionPushed;
			_xboxcontroller.ActionPushed += OnActionPushed;
		}

		/// <summary>
		/// Unsubscribe from  controllers event
		/// </summary>
		private void Unsubscribe()
		{
			_keyBoard.MoveRaised -= OnMovedRaisedKey;
			_xboxcontroller.MoveRaised -= OnMovedRaisedKey;

			_keyBoard.ActionPushed -= OnActionPushed;
			_xboxcontroller.ActionPushed -= OnActionPushed;
		}
	}
}
