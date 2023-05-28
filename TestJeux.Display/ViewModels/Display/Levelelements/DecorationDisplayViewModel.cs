using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using TestJeux.API.Models;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;
using TestJeux.Display.Helper;

namespace TestJeux.Display.ViewModels.Display.Levelelements
{
	public class DecorationDisplayViewModel : BindableBase
    {
        DecorationManager _decorationManager;
        IImageManager _imageManager;

        public ObservableCollection<DecorationViewModel> Decorations { get; set; }

        public DecorationDisplayViewModel(DecorationManager decorationManager, IImageManager imageManager)
        {
            Decorations = new ObservableCollection<DecorationViewModel>();
            _decorationManager = decorationManager;
            _imageManager = imageManager;
        }

        public void ReloadDecoration(List<DecorationDto> decorationsDto)
        {
            Decorations.Clear();
            foreach (var deco in decorationsDto)
                Decorations.Add(new DecorationViewModel(deco.ID) { Decoration = deco.Decoration, Sprite = GetImageBitmap(deco.Decoration.ToString()), X = deco.TopLeft.X, Y = deco.TopLeft.Y });
        }

        private CachedBitmap GetImageBitmap(string code)
        {
            return ImageHelper.GetImage(code);
        }
    }
}
