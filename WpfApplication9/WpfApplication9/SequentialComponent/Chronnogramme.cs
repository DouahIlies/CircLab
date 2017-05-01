using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using CircLab.Component;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CircLab.SequentialComponent
{
    class Chronogramme : StandardComponent
    {
        ChronoWindow chronogramme;
        private DispatcherTimer t;
        public int nbrEntrée;
        public float Delay { get; set; }
      
        public Chronogramme(int nbrinputs,float Delay)
            : base(nbrinputs, 1, 0, "M0.5,0.5 L27,0.5 L27,27.5 L0.5,27.5 z", "chronogramme")
        {
            TypeLabel.Text = "Chronnogramme";
            this.OutputStack.Children.Clear();
            this.nbrEntrée = nbrinputs;
            this.MouseDoubleClick += DoubleClickEventHandler;
            this.Delay = Delay;
           
        }
        public void doubleClick()
        {
                      
            t.Tick += timer_Tick;
            t.Interval = TimeSpan.FromMilliseconds(100 * Delay);
            t.IsEnabled = true;
            for (int i = 0; i < nbrEntrée; i++)
            {
               chronogramme.chronogrammeStack.Children.Add(new Chart());
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            t.Interval = TimeSpan.FromMilliseconds(100 * Delay);
            update_input();
            if (chronogramme != null)
            {
                for (int k = 0; k < inputs_tab.Count; k++)
                {
                    double tmp;
                    if (((bool)inputs_tab[k]) == true)
                    {
                        tmp = 1;
                    }
                    else
                    {
                        tmp = 0;
                    }
                  
                    ((Chart)this.chronogramme.chronogrammeStack.Children[k]).drawchart(tmp);
                }
            }
        }

        public void pause_continue()
        {
            t.IsEnabled = !t.IsEnabled;

        }
        private void DoubleClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            t = new DispatcherTimer();
            if (chronogramme == null || !chronogramme.Activate())
            {
                chronogramme = new ChronoWindow(this.t);
            }
          
           
            chronogramme.Show();
            doubleClick();
        }
        public override void Run()
        {
          
        }


        public void remove()
        {
            if (this.chronogramme != null)
            {
                chronogramme.Close();
            }
        }
    }
}
