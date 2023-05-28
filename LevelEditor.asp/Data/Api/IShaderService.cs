using TestJeux.API.Models;

namespace LevelEditor.asp.Data.Api
{
	/// <summary>
	/// Shader service
	/// </summary>
	public interface IShaderService
	{
		/// <summary>
		/// Get all shaders
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<ShaderDto>> GetShaders();
	}
}
