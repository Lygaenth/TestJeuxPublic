using Microsoft.AspNetCore.Mvc;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace GameWebApp.Controllers
{
	/// <summary>
	/// Shader controller
	/// </summary>
	[ApiController]
	[Route("Shaders")]
	public class ShaderController : ControllerBase
	{
		private readonly List<ShaderDto> _shaderTypes;

		/// <summary>
		/// constructor
		/// </summary>
		public ShaderController()
		{
			_shaderTypes = new List<ShaderDto>()
			{
				new ShaderDto(ShaderType.Natural),
				new ShaderDto(ShaderType.Cave),
				new ShaderDto(ShaderType.Evening)
			};
		}

		/// <summary>
		/// Get shaders
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetShaders()
		{
			return new OkObjectResult(_shaderTypes);
		}
	}
}
