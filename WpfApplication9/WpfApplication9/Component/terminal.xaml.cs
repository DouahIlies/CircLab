using System;
using System.Collections;
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

namespace CircLab.Component
{
    /// <summary>
    /// Interaction logic for terminal.xaml
    /// </summary>
    public partial class Terminal : UserControl
    {
        public Boolean IsOutpt;//si vrai => ouput ,si false => intput;
        public ArrayList wires; //Un terminal de sortie peut être brancher à plusieurs entrés 
        public Wireclass logestWire;
        public Boolean IsInversed;//
        public bool etat;
        public static int idTotal = 0;
        public int id;


        public Terminal()
        {
            this.id = idTotal;
            id++;
            
            InitializeComponent();
            IsInversed = false;
            wires = new ArrayList();
            elSelector.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
        }

        public void relier(object sender, MouseButtonEventArgs e)
        {
            Terminal terminal = UserClass.TryFindParent<Terminal>((Ellipse)sender);
            StandardComponent componentPere = UserClass.TryFindParent<StandardComponent>((Ellipse)sender);
            Canvas canvas = UserClass.TryFindParent<Canvas>((terminal));
  
            Wireclass wire = new Wireclass();
     
            wire.relier();
        }

        //Recalcule de position de chaque terminal en recaluculant la pos de tout les fils
        public void recalculer(int rotation)
        {
            foreach(Wireclass wire in wires)
            {
                
                wire.recalculer(rotation,this.IsOutpt);
            }
        }

        public void input_inversed()
        {
            if(IsInversed)
            {
                inverse_input.Visibility = Visibility.Visible;
            }
            else
            {
                inverse_input.Visibility = Visibility.Collapsed;
            }
        }

      

        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Ellipse img = sender as Ellipse;
            Canvas canvas = UserClass.TryFindParent<Canvas>(img);
            MainWindow.sourceEllipse = sender as Ellipse;
            MainWindow.wire = new Wireclass();
            MainWindow.SelectedTerminalIsSource = this.IsOutpt;
            Mouse.Capture(canvas);
            
        }

        
    }
}
