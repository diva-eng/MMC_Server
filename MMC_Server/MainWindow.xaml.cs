using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;

namespace MMC_Server
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
        private void ToggleFlyout(int index)
        {
            CloseAllFlyout();
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        private void Toggle_Settings(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }
		private void Toggle_DeviceList(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(1);
        }
        private void CloseAllFlyout()
        {
            foreach (Flyout f in this.Flyouts.Items)
            {
                if (f != null)
                    f.IsOpen = false;
            }
        }
    }
}
