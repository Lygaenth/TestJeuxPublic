using TestJeux.Business.ObjectValues;

namespace TestJeux.Core.DomainEvents
{
	public class LightChangedEventArgs
	{
		/// <summary>
		/// Item changed ID
		/// </summary>
		public int ItemID { get; private set; }

		/// <summary>
		/// Light new values
		/// </summary>
		public LightState LightSource { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="lightSourceDto"></param>
		public LightChangedEventArgs(int id, LightState lightState)
		{
			ItemID = id;
			LightSource = lightState;
		}
	}
}
