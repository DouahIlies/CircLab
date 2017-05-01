using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CircLab.SequentialComponent
{
    /// <summary>
    /// Interaction logic for ChronoWindow.xaml
    /// </summary>
    public partial class ChronoWindow : Window
    {
        private DispatcherTimer t;
        public ChronoWindow(DispatcherTimer x)
        {

            t = x;
            InitializeComponent();
        }

        private void ResetChronogrammes(object sender, RoutedEventArgs e)
        {
            int tmp = chronogrammeStack.Children.Count;
            chronogrammeStack.Children.Clear();
            for (int i = 0; i < tmp; i++)
            {
                chronogrammeStack.Children.Add(new Chart());
            }

        }

        private void Pause_Continu(object sender, RoutedEventArgs e)
        {
            Pause.Content = ((string)Pause.Content == "Pause")? "Continuer" : "Pause";
            this.t.IsEnabled = !this.t.IsEnabled;
        }



        public void Save(object sender, RoutedEventArgs e)
        {
            //   this.ConvertToBitmapSource(chronogrammeStack,);
        }

        public void ConvertToBitmapSource(UIElement element, String fileName)
        {
            var target = new RenderTargetBitmap(
                (int)element.RenderSize.Width, (int)element.RenderSize.Height,
                96, 96, PixelFormats.Pbgra32);
            target.Render(element);

            var encoder = new PngBitmapEncoder();
            var outputFrame = BitmapFrame.Create(target);
            encoder.Frames.Add(outputFrame);

            using (var file = File.OpenWrite(fileName))
            {
                encoder.Save(file);
            }
        }


        private string _filename = "";

        private void btnSaveAs_click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "CircLab Circuit (.png)|*.png";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    _filename = dlg.FileName;
                    this.ConvertToBitmapSource(chronogrammeStack, _filename);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save circuit as requested: " + ex.ToString());
                }
            }





        }


        private void SaveMeEvent(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "CircLab Circuit (.png)|*.png";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {

                    _filename = dlg.FileName;
                    this.ConvertToBitmapSource(chronogrammeStack, _filename);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save circuit as requested: " + ex.ToString());
                }
            }



        }

        private void SaveEvent(object sender,RoutedEventArgs e)
        {
            if (_filename == "")
            {
                this.SaveMeEvent(sender, e);
            }
            else
            {
                this.ConvertToBitmapSource(chronogrammeStack,_filename);
            }
        }
    }
}
