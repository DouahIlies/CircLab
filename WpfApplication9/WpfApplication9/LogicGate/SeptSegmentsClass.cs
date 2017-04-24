using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CircLab.Component;

namespace CircLab.LogicGate
{
    class SeptSegmentsClass : StandardComponent
    {
        public Sept_Segments sept = new Sept_Segments();
        public SeptSegmentsClass()
            : base(7,1,0, "M 15,17 h 10 c 10,0 20,5 25,15 c -5,10 -15,15 -25,15 h -10 c 5,-10 5,-20 0,-30","7Segment")
        {
            this.OutputStack.Children.Clear();
             this.grid.Children.Remove(typeComponenet);
            sept.Margin = new Thickness(14, 25, 0, 0);
            sept.HorizontalAlignment = HorizontalAlignment.Left;
            sept.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(sept);
        }

        
        public override void Run()
        {

            update_input();
            if ((bool)inputs_tab[0] == true)
            {
                sept.l1.Stroke = Brushes.Red;
            }
            else
            {
                sept.l1.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[1] == true)
            {
                sept.l2.Stroke = Brushes.Red;
            }
            else
            {
                sept.l2.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[2] == true)
            {
                sept.l3.Stroke = Brushes.Red;
            }
            else
            {
                sept.l3.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[3] == true)
            {
                sept.l4.Stroke = Brushes.Red;
            }
            else
            {
                sept.l4.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[4] == true)
            {
                sept.l5.Stroke = Brushes.Red;
            }
            else
            {
                sept.l5.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[5] == true)
            {
                sept.l6.Stroke = Brushes.Red;
            }
            else
            {
                sept.l6.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[6] == true)
            {
                sept.l7.Stroke = Brushes.Red;
            }
            else
            {
                sept.l7.Stroke = Brushes.Gray;
            }
        



        }
    }
}
