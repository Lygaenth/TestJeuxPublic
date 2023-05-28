using LevelEditor.asp.Models;
using Microsoft.AspNetCore.Components;

namespace LevelEditor.asp.Shared
{
	/// <summary>
	/// Map display component
	/// </summary>
	public partial class MapDisplay
	{
		#region parameters
		[Parameter]
		public LevelModel LevelModel { get; set; }

		[Parameter]
		public EventCallback<LevelModel> LevelModelChanged { get; set; }
		#endregion

		public MapDisplay()
		{

		}

		private string GetCellClass(TileModel tile)
		{
			return tile != null && tile.IsSelected ? "selectedTd" : "compact";
		}

		private string GetRotateClass(int angle)
		{
			return "transform: rotate(" + angle + "deg)";
		}

		private string GetImgPosition(int X, int Y)
		{
			var style = "position: absolute; top: " + Y + "px; left: " + X + "px;";
			return style;
		}

		private async Task Select(TileModel tile)
		{
			if (LevelModel.SelectedSprite != null)
				LevelModel.SelectedSprite.IsSelected = false;

			LevelModel.SelectedSprite = tile;
			LevelModel.SelectedSprite.IsSelected = true;
			await LevelModelChanged.InvokeAsync(LevelModel);
		}
	}
}
