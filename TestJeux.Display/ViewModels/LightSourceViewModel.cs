using System.Drawing;
using TestJeux.Display.ViewModels.Base;

namespace TestJeux.Display.ViewModels
{
	public class LightSourceViewModel : BaseViewModel
	{
		private bool _isLit;
		private int _lightIntensity;
		private Point _center;

		public int ID { get; private set; }

		/// <summary>
		/// Is lit
		/// </summary>
		public bool IsLit { get => _isLit; set => SetProperty(ref _isLit, value); }

		/// <summary>
		/// Light intensity
		/// </summary>
		public int LightIntensity { get => _lightIntensity; set => SetProperty(ref _lightIntensity, value); }

		/// <summary>
		/// Light source center
		/// </summary>
		public Point Center { get => _center; set => SetProperty(ref _center, value); }


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="isLit"></param>
		/// <param name="lightIntesity"></param>
		/// <param name="center"></param>
		public LightSourceViewModel(int id, bool isLit, int lightIntesity, Point center)
		{
			ID = id;
			_isLit = isLit;
			_lightIntensity = lightIntesity;
			_center = center;
		}
	}
}
