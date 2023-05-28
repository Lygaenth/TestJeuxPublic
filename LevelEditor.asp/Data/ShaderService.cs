using LevelEditor.asp.Data.Api;
using System.Text.Json;
using TestJeux.API.Models;

namespace LevelEditor.asp.Data
{
	/// <summary>
	/// Shader provider
	/// </summary>
	public class ShaderService : IShaderService
	{
		private readonly HttpClient _httpClient;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpClient"></param>
		public ShaderService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		/// <summary>
		/// Get all shaders
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<ShaderDto>> GetShaders()
		{
			var res = await _httpClient.GetStreamAsync($"/Shaders");
			return await JsonSerializer.DeserializeAsync<IEnumerable<ShaderDto>>(res, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
		}
	}
}
