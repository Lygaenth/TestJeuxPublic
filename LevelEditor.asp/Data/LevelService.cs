using LevelEditor.asp.Data.Api;
using System.Text;
using System.Text.Json;
using TestJeux.API.Models;

namespace LevelEditor.asp.Data
{
	public class LevelService : ILevelService
	{

		private readonly HttpClient _httpClient;
		private JsonSerializerOptions _serializerOptions;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpClient"></param>
		public LevelService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
		}

		/// <summary>
		/// Get all level IDs
		/// </summary>
		/// <returns></returns>
		public async Task<List<int>> GetLevelIds()
		{
			var ids = await JsonSerializer.DeserializeAsync<IEnumerable<int>>(await _httpClient.GetStreamAsync($"/Levels/ids"), _serializerOptions);
			if (ids == null)
				return new List<int>();
			return ids.ToList();
		}

		/// <summary>
		/// Get level by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<LevelDto> GetLevel(int id)
		{
			var res = await JsonSerializer.DeserializeAsync<LevelDto>(await _httpClient.GetStreamAsync($"/Levels/{id}"), _serializerOptions);
			if (res != null)
				return res;
			throw new Exception("Failed to receive Levels data");
		}

		/// <summary>
		/// Get level decorations
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<List<DecorationDto>> GetLevelDecorations(int id)
		{
			return await JsonSerializer.DeserializeAsync<List<DecorationDto>>(await _httpClient.GetStreamAsync($"/Levels/{id}/Decorations"), _serializerOptions);
		}

		/// <summary>
		/// Post level
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<LevelDto> PostLevel(LevelDto level)
		{
			var content = new StringContent(JsonSerializer.Serialize(level), Encoding.UTF8, "application/json");

			var res = await _httpClient.PostAsync("Levels/", content);
			if (res.IsSuccessStatusCode)
				return await JsonSerializer.DeserializeAsync<LevelDto>(res.Content.ReadAsStream(), _serializerOptions);
			else
				throw new Exception("Failed to post element");
		}
	}
}
