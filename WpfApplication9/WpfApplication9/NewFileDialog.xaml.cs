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
    /// Interaction logic for NewFileDialog.xaml
    /// </summary>
    public partial class NewFileDialog : UserControl
    {
        private Result _r = Result.CANCEL;
        public enum Result
        {
            CANCEL, CREATE
        }
        public Result Selected
        {
            get { return _r; }
        }
        public NewFileDialog()
        {
            InitializeComponent();
        }
        
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            _r = Result.CREATE;
            ((Button)sender).Command.Execute(((Button)sender).CommandParameter);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _r = Result.CANCEL;
            ((Button)sender).Command.Execute(((Button)sender).CommandParameter);
        }
    }
}
