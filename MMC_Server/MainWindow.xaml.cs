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
using MMC_Server.ViewModel;
using MMC_Server.Model;
using System.Drawing;

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
            //this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            this.DataContext = new MainWindowViewModel();

#if DEBUG

            var c = new Character
            {
                Name = "Miku [1]",
                ModelCode = "SpqhgyzipV",
                MotionCode = "erdT4oRZ3s",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/accessory/532686332fe7d/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [2]",
                ModelCode = "bQAZAFe0Et",
                MotionCode = "DV1xHJP1SY",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/model/53325ac36de9a/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [1]",
                ModelCode = "SpqhgyzipV",
                MotionCode = "erdT4oRZ3s",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/accessory/532686332fe7d/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [2]",
                ModelCode = "bQAZAFe0Et",
                MotionCode = "DV1xHJP1SY",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/model/53325ac36de9a/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [1]",
                ModelCode = "SpqhgyzipV",
                MotionCode = "erdT4oRZ3s",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/accessory/532686332fe7d/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [2]",
                ModelCode = "bQAZAFe0Et",
                MotionCode = "DV1xHJP1SY",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/model/53325ac36de9a/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [1]",
                ModelCode = "SpqhgyzipV",
                MotionCode = "erdT4oRZ3s",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/accessory/532686332fe7d/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [2]",
                ModelCode = "bQAZAFe0Et",
                MotionCode = "DV1xHJP1SY",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/model/53325ac36de9a/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [1]",
                ModelCode = "SpqhgyzipV",
                MotionCode = "erdT4oRZ3s",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/accessory/532686332fe7d/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            c = new Character
            {
                Name = "Miku [2]",
                ModelCode = "bQAZAFe0Et",
                MotionCode = "DV1xHJP1SY",
                Preview = new BitmapImage(new Uri("http://motiondex.com/resources/model/53325ac36de9a/preview.png"))
            };
            lboxCharCharacters.Items.Add(c);

            /*
            lboxCharCharacters.Items.Add(new Character
            {
                Name = "Miku [2]"//,
                //ModelCode = "HI7dNej7UI",
                //MotionCode = "WMHIWpY8eS"//,
                //Preview = new BitmapImage(new Uri("preview1.png", UriKind.RelativeOrAbsolute))
            });
            */
#endif

        }
        private void ToggleFlyout(int index)
        {
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
