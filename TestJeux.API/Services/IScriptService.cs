using TestJeux.API.Services;

namespace TestJeux.Business.Managers.API
{
    /// <summary>
    /// Service for managing items scripts
    /// </summary>
	public interface IScriptService : IService
    {
        /// <summary>
        /// Register item if it has a script
        /// </summary>
        /// <param name="itemId"></param>
        void Add(int itemId);
    }
}
