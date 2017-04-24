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

        public Chronogramme(int nbrinputs)
            : base(nbrinputs, 1, 0, "M0.5,0.5 L27,0.5 L27,27.5 L0.5,27.5 z", "chronogramme")
        {
            this.OutputStack.Children.Clear();
            this.MouseDoubleClick += DoubleClickEventHandler;
        }
        public void doubleClick()
        {
                      
            t.Tick += timer_Tick;
            t.Interval = TimeSpan.FromMilliseconds(1000);
            t.IsEnabled = true;
            for (int i = 0; i < inputs_tab.Count; i++)
            {
                
                chronogramme.chronogrammeStack.Children.Add(new Chart());
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
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
    }
}
