using Prism.Mvvm;
using System.Windows.Input;

namespace TestJeux.Display.ViewModels.Display.Menu
{
    public class MenuItemViewModel : BindableBase
    {
        private string _text;
        private bool _isSelected;
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        public ICommand Command { get; set; }

        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }

        public MenuItemViewModel(string text, ICommand cmd)
        {
            Text = text;
            Command = cmd;
        }
    }
}
