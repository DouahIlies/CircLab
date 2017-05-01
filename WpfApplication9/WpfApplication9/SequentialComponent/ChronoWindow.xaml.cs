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
            for (int i=0;i< tmp; i++)
            {
                chronogrammeStack.Children.Add(new Chart());
            }
          
        }

        private void Pause_Continu(object sender, RoutedEventArgs e)
        {
            Pause.Content = ((string)Pause.Content == "Pause")? "Continuer" : "Pause";
            this.t.IsEnabled = !this.t.IsEnabled;
        }

    }
}
