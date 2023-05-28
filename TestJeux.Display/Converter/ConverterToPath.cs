using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TestJeux.Display.ViewModels;

namespace TestJeux.Display.Converter
{
	public class ConverterToPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var geometry = new PathGeometry();
            var sources = value as ObservableCollection<LightSourceViewModel>;
            Geometry currentGeometry = new RectangleGeometry(new System.Windows.Rect(0, 0, 800, 800));
            if (sources != null)
            {
                foreach (var source in sources)
                {
                    if (!source.IsLit)
                        continue;

                    var cg = new CombinedGeometry() { GeometryCombineMode = GeometryCombineMode.Exclude };
                    cg.Geometry1 = currentGeometry;
                    cg.Geometry2 = new EllipseGeometry(new Point(source.Center.X, source.Center.Y), source.LightIntensity + (DateTime.Now.Millisecond / 200 % 3), source.LightIntensity + (DateTime.Now.Millisecond / 200  % 3));
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
