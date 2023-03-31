using System.Windows;
using System.Windows.Input;
using TestJeux.Display.ViewModels;

namespace TestJeux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameViewModel _vm;

        public MainWindow()
        {
            _vm = new GameViewModel();
            InitializeComponent();
            DataContext = _vm;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _vm.KeyBoardVm.KeyDown(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _vm.KeyBoardVm.KeyUp(e.Key);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _vm.ActionStart();
        }

        private void QuitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
