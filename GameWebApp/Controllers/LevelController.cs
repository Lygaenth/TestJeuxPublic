using Microsoft.AspNetCore.Mvc;
using Test.Jeux.Data.Xml;
using TestJeux.API.Models;
using TestJeux.Data.Api;

namespace GameWebApp.Controllers
{
	[ApiController]
	[Route("Levels")]
	public class LevelController : ControllerBase
	{
		private readonly IDALLevels _dalLevels;

		public LevelController()
		{
			_dalLevels = new DALLevels();
		}

		[HttpGet]
		public IActionResult GetLevel()
		{
			return new OkObjectResult(_dalLevels.LoadAllLevels());
		}

		[HttpGet("ids")]
		public IActionResult GetLevelIds()
		{
			return new OkObjectResult(_dalLevels.LoadAllLevels().Select(l => l.ID));
		}

		[HttpGet("{id}")]
		public IActionResult GetLevel(int id)
		{
			return new OkObjectResult(_dalLevels.GetDataById(id));
		}

		[HttpGet("{id}/Decorations")]
		public IActionResult GetLevelDecorations(int id)
		{
			return new OkObjectResult(_dalLevels.GetDataById(id).Decorations);
		}

		[HttpGet("{id}/Items")]
		public IActionResult GetLevelItems(int id)
		{
			return new OkObjectResult(_dalLevels.GetDataById(id).Items);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<LevelDto> CreateLevel(LevelDto level)
		{
			_dalLevels.SaveLevel(level);

			return CreatedAtAction(nameof(GetLevel), new { id = level.ID }, level);
		}
	}
}