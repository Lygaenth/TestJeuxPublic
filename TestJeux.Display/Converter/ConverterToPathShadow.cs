using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TestJeux.Display.ViewModels;

namespace TestJeux.Display.Converter
{
	public class ConverterToPathShadow : IValueConverter
    {
        private const int HalfUnit = 25;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geometry = new PathGeometry();
            var sources = value as ObservableCollection<LightSourceViewModel>;
            Geometry currentGeometry = new RectangleGeometry(new System.Windows.Rect(0, 0, 0, 0));
            if (sources != null)
            {
                foreach (var source in sources)
                {
                    if (!source.IsLit)
                        continue;

                    var shadow = new CombinedGeometry() { GeometryCombineMode = GeometryCombineMode.Xor };
                    shadow.Geometry1 = new EllipseGeometry(new Point(source.Center.X, source.Center.Y), source.LightIntensity, source.LightIntensity);
                    shadow.Geometry2 = new EllipseGeometry(new Point(source.Center.X, source.Center.Y), source.LightIntensity * 3 / 4 + (DateTime.Now.Millisecond % 4), source.LightIntensity * 3 / 4);

                    var cg = new CombinedGeometry() { GeometryCombineMode = GeometryCombineMode.Union };
                    cg.Geometry1 = currentGeometry;
                    cg.Geometry2 = shadow;
                    currentGeometry = cg;
                }
            }
            return PathGeometry.CreateFromGeometry(currentGeometry);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
