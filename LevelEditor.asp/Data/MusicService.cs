using LevelEditor.asp.Data.Api;
using System.Text.Json;
using TestJeux.API.Models;

namespace LevelEditor.asp.Data
{
	/// <summary>
	/// Music service
	/// </summary>
	public class MusicService : IMusicService
	{
		private readonly HttpClient _httpClient;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpClient"></param>
		public MusicService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		/// <summary>
		/// Get all shaders
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<MusicDto>> GetAllMusics()
		{
			var res = await _httpClient.GetStreamAsync($"/Musics");
			return await JsonSerializer.DeserializeAsync<IEnumerable<MusicDto>>(res, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
		}
	}
}
