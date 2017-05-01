using CircLab.ComplexComponent;
using CircLab.Component;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication9.LogicGate
{
    class Hexadicimal : CircLab.Component.StandardComponent
    {

        public CircLab.LogicGate.Sept_Segments sept = new CircLab.LogicGate.Sept_Segments();
        public Hexadicimal()
            : base(7,1,0, "M 15,17 h 10 c 10,0 20,5 25,15 c -5,10 -15,15 -25,15 h -10 c 5,-10 5,-20 0,-30","7Segment")
        {
            
            this.OutputStack.Children.Clear();
            this.grid.Children.Remove(typeComponenet);

            this.grid.MinHeight = this.ActualHeight;
            this.inputStack.Children.RemoveAt(4);
            this.inputStack.Children.RemoveAt(4);
            this.inputStack.Children.RemoveAt(4);
          
            sept.Margin = new Thickness(14, 25, 0, 0);
            sept.HorizontalAlignment = HorizontalAlignment.Left;
            sept.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(sept);
            TypeLabel.Text = "Hexadecimal";

            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)inputStack.Children[2]).terminal_grid.ToolTip = "C";
            ((Terminal)inputStack.Children[3]).terminal_grid.ToolTip = "D";
         
        }

        public override void Run()
        {
            update_input();
            int nbr = ClassConverter.ConvertToInt(inputs_tab);
            switch(nbr)
            {
                case (0):
                   foreach(Line l in sept.sevenSegmentsGrid.Children)
                    {
                        l.Stroke = Brushes.Gray;
                    }
                    break;
                case (8):
                    {
                        foreach (Line l in sept.sevenSegmentsGrid.Children)
                        {
                            l.Stroke = Brushes.Red;
                        }
                        sept.lG.Stroke = Brushes.Gray;
                        break;
                    }
                case (1):
                    {
                        sept.lA.Stroke = Brushes.Gray;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Gray;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Gray;
                        sept.lG.Stroke = Brushes.Gray;
                        break;
                    }
                case (2):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Gray;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Gray;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (3):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Gray;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (4):
                    {
                        sept.lA.Stroke = Brushes.Gray;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Gray;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (5):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (6):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (7):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Gray;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Gray;
                        sept.lG.Stroke = Brushes.Gray;
                        break;
                    }
                case (9):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Gray;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (10):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Gray;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (11):
                    {
                        sept.lA.Stroke = Brushes.Gray;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (12):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Gray;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Gray;
                        break;
                    }
                case (13):
                    {
                        sept.lA.Stroke = Brushes.Gray;
                        sept.lB.Stroke = Brushes.Red;
                        sept.lC.Stroke = Brushes.Red;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Gray;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (14):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Gray;
                        sept.lD.Stroke = Brushes.Red;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }
                case (15):
                    {
                        sept.lA.Stroke = Brushes.Red;
                        sept.lB.Stroke = Brushes.Gray;
                        sept.lC.Stroke = Brushes.Gray;
                        sept.lD.Stroke = Brushes.Gray;
                        sept.lE.Stroke = Brushes.Red;
                        sept.lF.Stroke = Brushes.Red;
                        sept.lG.Stroke = Brushes.Red;
                        break;
                    }


            }
        }
    }
}
