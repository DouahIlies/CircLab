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
using CircLab.Component;
using CircLab.LogicGate;
using CircLab.SequentialComponent;

namespace CircLab.ComplexComponent
{
    class Multiplexer : StandardComponent
    {
        public Multiplexer(int nbrinput, int nbroutput ,int nbrselection)

            : base(nbrinput,1,nbrselection, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "MUX")
        {
            TypeLabel.Text = "Mux";

         
        }
        public override void Run()
        {
            update_input();
            outputs_tab.Clear();
            
            int val = ClassConverter.ConvertToInt(selections_tab);
            outputs_tab.Add(inputs_tab[val]);
            update_output();
        }

        public override void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            if (inputStack.Children.Count == 4 && selectionStack.Children.Count > 2)
            {
                for (int i = selectionStack.Children.Count; i > 2; i--)
                {
                    terminal = null;
                    Wireclass wire = null;

                    foreach (Terminal tmp in selectionStack.Children)
                    {
                        terminal = tmp;
                    }
                    foreach (Wireclass tmp in terminal.wires)
                    {
                        wire = tmp;
                    }
                    if (wire != null) wire.Destroy();
                    selectionStack.Children.Remove(terminal);
                    try
                    {
                        selections_tab.RemoveAt(1);
                    }
                    catch { }

                }
            }

            else if (inputStack.Children.Count == 8 && selectionStack.Children.Count < 3)
            {
                selectionStack.Margin = new Thickness(terminal.Width, 0, terminal.Width, 0);
                for (int i = selectionStack.Children.Count; i < 3; i++)
                {
                    
                    terminal = new Terminal();
                  
                    terminal.LayoutTransform = new RotateTransform(90);
                    terminal.IsOutpt = false;
                    terminal.Margin = new Thickness(-terminal.Width / Math.Pow(2, 3) - terminal.Width + 3, 0, 0, 2);
                    selectionStack.Children.Add(terminal);
                    
                    selections_tab.Add(false);
                }
            }

             

            grid.Height = inputStack.Children.Count * 22 + 25;
            typeComponenet.Height = terminal.Height * inputStack.Children.Count;
            typeComponenet.Width = terminal.Width * 4;

            typeComponenet.Data = StreamGeometry.Parse(path);
            typeComponenet.Stretch = Stretch.Fill;
            typeComponenet.StrokeThickness = 0;
            typeComponenet.Fill = Brushes.RoyalBlue;
            typeComponenet.Margin = new Thickness(14, 25, 0, 0);
            typeComponenet.HorizontalAlignment = HorizontalAlignment.Left;
            typeComponenet.VerticalAlignment = VerticalAlignment.Top;
            recalculer_pos();
            if (IsSelect) selectElement(this);
            canvas.UpdateLayout();
        }
    }
}
