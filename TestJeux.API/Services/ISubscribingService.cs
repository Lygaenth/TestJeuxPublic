namespace TestJeux.API.Services
{
	public interface ISubscribingService : IService
	{
		/// <summary>
		/// Subscribe to items events
		/// </summary>
		void Subscribe();

		/// <summary>
		/// Unsubscribe from items events
		/// </summary>
		void Unsubscribe();
	}
}
