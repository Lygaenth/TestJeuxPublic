using System.Windows;
using System.Windows.Controls;
using TestJeux.Core.Entities.Items;

namespace TestJeux.TemplateSelector
{
	public class DisplayDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CreatureModel)
                return ((FrameworkElement)container).FindResource("CreatureDataTemplate") as DataTemplate;
            else
                return ((FrameworkElement)container).FindResource("ItemDataTemplate") as DataTemplate; ;
        }
    }
}
