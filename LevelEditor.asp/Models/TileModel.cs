using TestJeux.Business.Managers.API;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
    /// <summary>
    /// Tile model
    /// </summary>
	public class TileModel
    {
        private readonly IImageManager _imageService;

        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public int Angle { get; set; }
        public bool IsSelected { get; set; }

        private GroundSprite _groundType;
        public GroundSprite GroundType
        {
            get => _groundType;
            set { _groundType = value; Image = _imageService.GetImage(_groundType.ToString()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageService"></param>
        public TileModel(IImageManager imageService)
        {
            _imageService = imageService;
        }

    }
}
