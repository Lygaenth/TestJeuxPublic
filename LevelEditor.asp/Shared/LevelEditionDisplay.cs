using LevelEditor.asp.Data;
using LevelEditor.asp.Data.Api;
using LevelEditor.asp.Extensions;
using LevelEditor.asp.Models;
using Microsoft.AspNetCore.Components;
using TestJeux.API.Models;
using TestJeux.Business.Managers.API;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Shared
{
	public partial class LevelEditionDisplay
	{
		#region Services
		[Inject]
		private ILevelService _levelService { get; set; }

		[Inject]
		private IShaderService _shaderService { get; set; }

		[Inject]
		private IMusicService _musicService { get; set; }

		[Inject]
		private IImageManager _imageService { get; set; }
		#endregion

		#region Parameters
		[Parameter]
		public List<int> Levels { get; set; }

		[Parameter]
		public EventCallback<List<int>> LevelsChanged { get; set; }

		[Parameter]
		public int SelectedLevel { get; set; }

		[Parameter]
		public EventCallback<int> SelectedLevelChanged { get; set; }

		[Parameter]
		public bool IsLoading { get; set; }

		[Parameter]
		public EventCallback<bool> IsLoadingChanged { get; set; }

		[Parameter]
		public LevelModel LevelModel { get; set; }

		[Parameter]
		public EventCallback<LevelModel> LevelModelChanged { get; set; }

		#endregion

		private bool DisplayDecoration { get; set; }

		private bool HasValidationError { get; set; }

		private string ValidationMessage { get; set; }
		
		private IEnumerable<ShaderDto> Shaders { get; set; }
		private IEnumerable<MusicDto> Musics { get; set; }

		public LevelEditionDisplay()
		{
			Levels = new List<int>();
		}

		/// <summary>
		/// On initialization
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			Shaders = (await _shaderService.GetShaders());
			Musics = (await _musicService.GetAllMusics());
			await ReloadLevelList();
			await ReloadLevel(1);
		}

		private async Task ReloadLevelList()
		{
			Levels.Clear();
			var levels = (await _levelService.GetLevelIds());
			Levels.AddRange(levels.Order());
			await LevelsChanged.InvokeAsync(Levels);
		}

		/// <summary>
		/// REturn style for rotated image
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		private string GetRotateClass(int angle)
		{
			return "transform: rotate(" + angle + "deg)";
		}

		/// <summary>
		/// Load selected level
		/// </summary>
		/// <param name="eventArgs"></param>
		private async void OnLevelChanged(ChangeEventArgs eventArgs)
		{
			if (eventArgs.Value == null)
				return;

			if (!Int32.TryParse(eventArgs.Value.ToString(), out int levelId))
				return;

			ReloadLevel(levelId);
		}

		private async Task OnDecorationDisplayChanged(ChangeEventArgs eventArgs)
		{
			if (eventArgs.Value == null || !bool.TryParse(eventArgs.Value.ToString(), out bool value))
				return;

			DisplayDecoration = value;

			await LoadDecorations();
			await LevelModelChanged.InvokeAsync(LevelModel);
		}

		/// <summary>
		/// Reload level
		/// </summary>
		/// <param name="levelId"></param>
		/// <returns></returns>
		private async Task ReloadLevel(int levelId)
		{
			IsLoading = true;
			var level = await _levelService.GetLevel(levelId);

			SelectedLevel = levelId;
			await SelectedLevelChanged.InvokeAsync(SelectedLevel);

			LevelModel = new LevelModel(levelId, _imageService);

			LevelModel.Tiles = new TileModel[16][];
			for (int i = 0; i < 16; i++)
				LevelModel.Tiles[i] = new TileModel[16];

			LevelModel.UpdateTiles((GroundSprite)level.DefaultTile, level.TilesZones);

			foreach (var item in level.Items)
				LevelModel.Items.Add(new Item() { ItemId = item.ID, Code = item.Code, StartPosition = item.StartPosition, Orientation = item.Orientation, DefaultState = item.DefaultState });

			foreach (var zone in level.Zones)
				LevelModel.Zones.Add(new Zone() { ZoneId = zone.ID, GroundType = zone.GroundType, TopLeft = zone.TopLeft, BottomRight = zone.BottomRight });

			await LoadDecorations();

			LevelModel.SelectedSprite = LevelModel.Tiles[0][0];
			await LevelModelChanged.InvokeAsync(LevelModel);

			IsLoading = false;
			await IsLoadingChanged.InvokeAsync(IsLoading);
		}

		/// <summary>
		/// Load or hide decorations
		/// </summary>
		/// <returns></returns>
		private async Task LoadDecorations(bool forceLoading = false)
		{
			LevelModel.Decorations.Clear();

			if (!DisplayDecoration && !forceLoading)
				return;

			foreach (var decoration in await _levelService.GetLevelDecorations(LevelModel.ID))
				LevelModel.Decorations.Add(new DecorationModel(decoration, _imageService.GetImage(decoration.Decoration.ToString())));
		}

		/// <summary>
		/// Load or hide decorations
		/// </summary>
		/// <returns></returns>
		private async Task LoadItems(bool forceLoading = false)
		{
			LevelModel.Items.Clear();

			if (!DisplayDecoration && !forceLoading)
				return;

			foreach (var decoration in await _levelService.GetLevelDecorations(LevelModel.ID))
				LevelModel.Decorations.Add(new DecorationModel(decoration, _imageService.GetImage(decoration.Decoration.ToString())));
		}

		/// <summary>
		/// Update selected tile sprite
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private async Task UpdateSelectedTileSprite(ChangeEventArgs args)
		{
			if (!Enum.TryParse(args.Value.ToString(), out GroundSprite sprite))
				return;

			LevelModel.SelectedSprite.GroundType = sprite;
			LevelModel.IsEdited = true;
			await LevelModelChanged.InvokeAsync(LevelModel);
		}

		/// <summary>
		/// Update selected tile sprite
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private async Task UpdateSelectedTileAngle(ChangeEventArgs args)
		{
			var angle = Convert.ToInt32(args.Value);
			LevelModel.SelectedSprite.Angle = angle;
			LevelModel.IsEdited = true;
			await LevelModelChanged.InvokeAsync(LevelModel);
		}

		/// <summary>
		/// Add new level to edit
		/// </summary>
		/// <returns></returns>
		private async Task AddNewLevel()
		{
			LevelModel = new LevelModel(-1, _imageService);
			LevelModel.UpdateTiles(GroundSprite.Grass, new List<TileZoneDto>());
			LevelModel.SelectedSprite = LevelModel.Tiles[0][0];
			await LevelModelChanged.InvokeAsync(LevelModel);
			LevelModel.IsEdited = true;
		}

		/// <summary>
		/// Pass into edition mode of current level
		/// </summary>
		/// <returns></returns>
		private void EditCurrentLevel()
		{
			LevelModel.IsEdited = true;
		}

		/// <summary>
		/// Save current changes
		/// </summary>
		/// <returns></returns>
		private async Task Save()
		{
			HasValidationError = false;
			try
			{
				if (LevelModel.ID < 1)
				{
					await LoadDecorations(true);
				}

				var result = await _levelService.PostLevel(LevelModel.Convert());
				if (result == null)
				{
					// Notify error
					return;
				}

				LevelModel.IsEdited = false;
				await ReloadLevelList();
				await ReloadLevel(result.ID);
				SelectedLevel = result.ID;
			}
			catch(Exception e)
			{
				HasValidationError = true;
				ValidationMessage = "Failed to save: "+ e.Message;		
			}
		}

		/// <summary>
		/// Canel current changes and reload level
		/// </summary>
		/// <returns></returns>
		private async Task Cancel()
		{
			HasValidationError = false;

			if (Levels.Any(id => id == LevelModel.ID))
				await ReloadLevel(LevelModel.ID);
			else
				await ReloadLevel(Levels.First());
		}
	}
}
