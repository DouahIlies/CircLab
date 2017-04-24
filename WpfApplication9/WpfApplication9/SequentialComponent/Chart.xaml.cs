using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CircLab.SequentialComponent
{
    /// <summary>
    /// Interaction logic for chronogramme.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        private double x = 0;
        private double y = 0;

        private ObservableCollection<KeyValuePair<String, double>> Power = new ObservableCollection<KeyValuePair<String, double>>();
        public Chart()
        {
            InitializeComponent();
            
            chart.DataContext = Power;

        }
        public void drawchart(double yr)
        {
            
            Power.Add(new KeyValuePair<String, double>(x.ToString(), y));
            Power.Add(new KeyValuePair<String, double>(x.ToString(), yr));
            x++;
            y = yr;
            
        }
     

        public void ResetChronohramme()
        {
            Power.Clear();
            
            //chart.Series=new   
            //chart.Series.Clear();
            
           // ls = new System.Windows.Controls.DataVisualization.Charting.LineSeries();
            //ls.ItemsSource = null;
            //ls.IndependentValueBinding = new Binding("Key");
            //ls.DependentValueBinding = new Binding("Value");
            //chart.Series.Add(ls);


            
            //ls.ItemsSource = Power;
            //chart.DataContext = Power;
        }


    }

}
