using LevelEditor.asp.Data;
using LevelEditor.asp.Data.Api;
using LevelEditor.asp.Models;
using LevelEditor.asp.Shared;
using Microsoft.AspNetCore.Components;
using TestJeux.API.Models;
using TestJeux.Business.Managers.API;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Pages
{
	public partial class LevelEdition
	{
		[Inject]
		private ILevelService LevelService { get; set; }

		[Inject]
		private IImageManager ImageService { get; set; }

		private MapDisplay? Map { get; set; }

		private LevelEditionDisplay LevelEditionMenu { get; set; }

		private List<int> Levels { get; set; }

		public LevelModel SelectedLevel { get; set; }

		private bool IsLoading { get; set; }

		public LevelEdition()
		{
			Levels = new List<int>();
			IsLoading = true;
		}
	}
}
