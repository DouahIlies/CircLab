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

namespace CircLab
{
    /// <summary>
    /// Interaction logic for SaveClose.xaml
    /// </summary>
    public partial class SaveClose : UserControl
    {
        private Result _r = Result.CANCEL;


        public enum Result
        {
            SAVE, DONT_SAVE, CANCEL
        }


        /// <summary>
        /// Result selected by the user.  Defaults to CANCEL
        /// </summary>
        public Result Selected
        {
            get
            {
                return _r;
            }
        }
        public SaveClose(string fileName)
        {
            InitializeComponent();
            FileName.Text = fileName;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _r = Result.CANCEL;
            ((Button)sender).Command.Execute(((Button)sender).CommandParameter);
        }

        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            _r = Result.DONT_SAVE;
            ((Button)sender).Command.Execute(((Button)sender).CommandParameter);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _r = Result.SAVE;
            ((Button)sender).Command.Execute(((Button)sender).CommandParameter);
        }
    }
}
