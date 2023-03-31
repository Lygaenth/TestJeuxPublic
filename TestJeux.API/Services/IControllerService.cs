using System;
using TestJeux.API.Events;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.Controllers
{
	/// <summary>
	/// Controlle service API
	/// </summary>
	public interface IControllerService
	{
		event EventHandler ActionPushed;
		event MoveEventHandler MoveRaised;
		event EventHandler NextItemPushed;

		/// <summary>
		/// Register move key pressed
		/// </summary>
		/// <param name="key"></param>
		void RegisterKeyDown(ControlAction key);

		/// <summary>
		/// Register move key released
		/// </summary>
		/// <param name="key"></param>
		void RegisterKeyUp(ControlAction key);

		/// <summary>
		/// Start checking supported controllers inputs
		/// </summary>
		void Start();

		/// <summary>
		/// Stop checking supported controller inputs
		/// </summary>
		void Stop();
	}
}