using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TestJeux.Business.Managers;
using TestJeux.Business.Managers.API;

namespace TestJeux.Display.Helper
{
	static public class ImageHelper
    {
        private static CachedBitmap _errorBitmap;
        private static IImageManager _imageManager;
        private static Dictionary<string, CachedBitmap> _images;

        public static CachedBitmap GetImage(string code)
        {
            if (_imageManager == null)
            {
                _images = new Dictionary<string, CachedBitmap>();
                _imageManager = new ImageManager();
                CreateBlackImage();
            }
    
            if (!_images.ContainsKey(code))
                _images[code] = CreateBitmap(Path.GetFullPath(_imageManager.GetImage(code)));

            return _images[code];
        }

        private static CachedBitmap CreateBitmap(string uri)
        {
            if (!File.Exists(uri))
                return _errorBitmap;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
             bitmap.UriSource = new Uri(uri);
            bitmap.EndInit();
            return new CachedBitmap(bitmap, BitmapCreateOptions.None, BitmapCacheOption.Default);
        }

        public static ObservableCollection<CachedBitmap> GetImages(ObservableCollection<string> codes)
        {
            var images = new ObservableCollection<CachedBitmap>();
            foreach (var sprite in GetImages(codes.ToList()))
                images.Add(sprite);
            return images;
        }

		public static List<CachedBitmap> GetImages(List<string> codes)
		{
			var images = new List<CachedBitmap>();
			foreach (var code in codes)
				images.Add(GetImage(code));
			return images;
		}

		private static void CreateBlackImage()
        {
            PixelFormat pf = PixelFormats.Bgr32;
            int width = 50;
            int height = 50;
            int rawStride = (width * pf.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];

            _errorBitmap = new CachedBitmap(BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, rawStride), BitmapCreateOptions.None, BitmapCacheOption.Default);
        }

    }
}
