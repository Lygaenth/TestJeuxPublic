using TestJeux.API.Models;

namespace LevelEditor.asp.Data.Api
{
	public interface ILevelService
	{
		/// <summary>
		/// GetSpecific levels
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<LevelDto> GetLevel(int id);

		/// <summary>
		/// Get specific level decorations
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<DecorationDto>> GetLevelDecorations(int id);

		/// <summary>
		/// Get list of level ids
		/// </summary>
		/// <returns></returns>
		Task<List<int>> GetLevelIds();

		/// <summary>
		/// Post level
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		Task<LevelDto> PostLevel(LevelDto level);
	}
}