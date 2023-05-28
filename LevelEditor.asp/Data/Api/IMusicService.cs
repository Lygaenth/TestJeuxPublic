using TestJeux.API.Models;

namespace LevelEditor.asp.Data.Api
{
	/// <summary>
	/// Music
	/// </summary>
	public interface IMusicService
	{
		/// <summary>
		/// Get all musics
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<MusicDto>> GetAllMusics();
	}
}
