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
            TypeLabel.Text = "7Segment";
          
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)inputStack.Children[2]).terminal_grid.ToolTip = "C";
            ((Terminal)inputStack.Children[3]).terminal_grid.ToolTip = "D";
            ((Terminal)inputStack.Children[4]).terminal_grid.ToolTip = "E";
            ((Terminal)inputStack.Children[5]).terminal_grid.ToolTip = "F";
            ((Terminal)inputStack.Children[6]).terminal_grid.ToolTip = "G";
        }

        
        public override void Run()
        {

            update_input();
            if ((bool)inputs_tab[0] == true)
            {
                sept.lA.Stroke = Brushes.Red;
            }
            else
            {
                sept.lA.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[1] == true)
            {
                sept.lB.Stroke = Brushes.Red;
            }
            else
            {
                sept.lB.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[2] == true)
            {
                sept.lC.Stroke = Brushes.Red;
            }
            else
            {
                sept.lC.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[3] == true)
            {
                sept.lD.Stroke = Brushes.Red;
            }
            else
            {
                sept.lD.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[4] == true)
            {
                sept.lE.Stroke = Brushes.Red;
            }
            else
            {
                sept.lE.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[5] == true)
            {
                sept.lF.Stroke = Brushes.Red;
            }
            else
            {
                sept.lF.Stroke = Brushes.Gray;
            }
            if ((bool)inputs_tab[6] == true)
            {
                sept.lG.Stroke = Brushes.Red;
            }
            else
            {
                sept.lG.Stroke = Brushes.Gray;
            }
        



        }
    }
}
