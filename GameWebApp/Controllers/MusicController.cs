using Microsoft.AspNetCore.Mvc;
using TestJeux.API.Models;
using TestJeux.SharedKernel.Enums;

namespace GameWebApp.Controllers
{
	[ApiController]
	[Route("Musics")]
	public class MusicController : ControllerBase
	{
		private readonly List<MusicDto> _musicTypes;

		/// <summary>
		/// Constructor
		/// </summary>
		public MusicController()
		{
			_musicTypes = new List<MusicDto>()
			{
				new MusicDto{ Id = Musics.None.GetHashCode(), Name = Musics.None.ToString() },
				new MusicDto{ Id = Musics.Menu.GetHashCode(), Name = Musics.Menu.ToString() },
				new MusicDto{ Id = Musics.LevelEasy.GetHashCode(), Name = Musics.LevelEasy.ToString() },
				new MusicDto{ Id = Musics.Cave.GetHashCode(), Name = Musics.Cave.ToString() }
			};
		}

		/// <summary>
		/// Get musics
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult GetMusics()
		{
			return new OkObjectResult(_musicTypes);
		}
	}
}
