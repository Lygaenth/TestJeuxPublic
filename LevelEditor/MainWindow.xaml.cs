using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TestJeux.Display.ViewModels.Display.Levelelements;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LevelEditorViewModel _levelEditor;

        public MainWindow()
        {
            InitializeComponent();

            _levelEditor = new LevelEditorViewModel();
            DataContext = _levelEditor;
        }


        private void Thumb_DragDeltaCenter(object sender, DragDeltaEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone;
            zone.X += (int)e.HorizontalChange;
            zone.Y += (int)e.VerticalChange;
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            var x = (zone.X +25) / 50;
            var y = (zone.Y + 25) / 50;
            var w = (zone.Width + 25) / 50;
            var h = (zone.Heigth + 25) / 50;

            zone.X = x * 50;
            zone.Y = y * 50;
            zone.Width = w * 50;
            zone.Heigth = h * 50;
        }

        private void Thumb_DragDeltaLeft(object sender, DragDeltaEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone;
            zone.X += (int)e.HorizontalChange;
            zone.Width -= (int)e.HorizontalChange;
        }

        private void Thumb_DragDeltaRight(object sender, DragDeltaEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone;
            zone.Width += (int)e.HorizontalChange;
        }

        private void Thumb_DragDeltaTop(object sender, DragDeltaEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone;
            zone.Y += (int)e.VerticalChange;
            zone.Heigth -= (int)e.VerticalChange;
        }

        private void Thumb_DragDeltaBottom(object sender, DragDeltaEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone;
            zone.Heigth += (int)e.VerticalChange;
        }

        private void Thumb_Click(object sender, MouseButtonEventArgs e)
        {
            var thumb = (sender as Thumb);
            if (thumb == null)
                return;

            var zone = thumb.DataContext as ZoneViewModel;
            if (zone == null)
                return;

            _levelEditor.SelectedZone = zone; 
        }
    }
}
