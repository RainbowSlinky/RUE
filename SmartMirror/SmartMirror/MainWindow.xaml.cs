using System.Windows;
using MahApps.Metro.Controls;
using SmartMirror.Common.Kinect;
using System.Windows.Media;

namespace SmartMirror
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }       

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
((MainWindow_ViewModel)this.DataContext).kinect.finializeInit();
        }
    }

}
