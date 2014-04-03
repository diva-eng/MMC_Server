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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro;

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
            this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
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

        private async void Toggle_DialogSongName(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowInputAsync("please input song title.", "song title");
        }

        private void Finalize(object sender, RoutedEventArgs e)
        {
            CalibrateAndTransfer();
        }


        private async void CalibrateAndTransfer()
        {
            await this.ShowMessageAsync("calibration enabled on all devices.", "please calibrate screens and click ok to finish.");
            var controller = await this.ShowProgressAsync("transferring data to clients", "please wait.");

            await Task.Delay(2000);

            double i = 0.0;
            while (i < 6.0)
            {
                double val = (i / 100.0) * 20.0;
                controller.SetProgress(val);
                controller.SetMessage("sending songs: " + i + "...");

                i += 1.0;

                await Task.Delay(1000);
            }

            await controller.CloseAsync();

            if (!controller.IsCanceled)
            {
                await this.ShowMessageAsync("done", "\"" + tboxConcertTitle.Text + "\" is ready to start!");
            }
        }

        private async void Toggle_DialogCharacterName(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowInputAsync("please input character title.", "character title");
        }
    }
}
