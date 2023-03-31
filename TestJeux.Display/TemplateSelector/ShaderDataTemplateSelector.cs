using System.Windows;
using System.Windows.Controls;
using TestJeux.Display.ViewModels;
using TestJeux.SharedKernel.Enums;

namespace TestJeux.Display.TemplateSelector
{
	public class ShaderDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var template = "Natural";
            if (item is ShaderViewModel vm)
            {
                switch(vm.ShaderType)
                {
                    case ShaderType.Cave:
                        template = "Cave";
                        break;
                    case ShaderType.Evening:
                        template = "Evening";
                        break;
                }
            }
            return ((FrameworkElement)container).FindResource(template) as DataTemplate;
        }
    }
}
