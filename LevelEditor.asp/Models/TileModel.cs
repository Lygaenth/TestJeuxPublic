using TestJeux.Business.Managers;
using TestJeux.SharedKernel.Enums;

namespace LevelEditor.asp.Models
{
	public class TileModel
    {
        private readonly ImageManager _imageService;

        public int X { get; set; }
        public int Y { get; set; }
        public string Image { get; set; }
        public int Angle { get; set; }
        public bool IsSelected { get; set; }

        private GroundSprite _groundType;
        public GroundSprite GroundType { get => _groundType; set { _groundType = value; Image = _imageService.GetImage(_groundType.ToString()); } }

        public TileModel(ImageManager imageService)
        {
            _imageService = imageService;
        }

    }
}
