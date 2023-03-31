using System.Windows;
using System.Windows.Controls;
using TestJeux.Display.ViewModels.Display.Stats;

namespace TestJeux.Display.TemplateSelector
{
    public class StatsDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is StatWithMaxViewModel)
                return ((FrameworkElement)container).FindResource("StatWithMaxTemplate") as DataTemplate;
            else
                return ((FrameworkElement)container).FindResource("StatBaseTemplate") as DataTemplate; ;
        }
    }
}
