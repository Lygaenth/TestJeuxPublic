using System;
using System.Windows;
using System.Windows.Controls;

namespace TestJeux.Display.CustomUc
{
    /// <summary>
    /// Logique d'interaction pour StatsDisplay.xaml
    /// </summary>
    public partial class StatsDisplay : UserControl
    {
        public StatsDisplay()
        {
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("TestJeux.Display;component/Styles/Style.xaml", UriKind.Relative) });
            InitializeComponent();
        }
    }
}
