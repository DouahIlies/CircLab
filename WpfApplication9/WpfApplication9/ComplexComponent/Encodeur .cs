using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;
using System.Windows.Media;
using System.Windows;

namespace CircLab.ComplexComponent
{
    class Encodeur : StandardComponent
    {
        private int _nbroutput;
        
        public Encodeur(int nbrinput, int nbroutput)
      
            : base(nbrinput,nbroutput,0," M49.7560975609756, 123.658536585366L98.2805836911085, 145.10929491592 98.2805836911085, 255.595622936389 49.2561883954712, 276.082695793093z", "ENCODEUR")
        {
            _nbroutput= nbroutput;
            TypeLabel.Text = "Coder";
        }

        public override void Run()
        {
            
            update_input();
            outputs_tab.Clear();
            int val = 0;
            while ((val < inputs_tab.Count) && ((bool)inputs_tab[val]==false))  { val ++; }
            if (val == inputs_tab.Count) val = 0 ;
            outputs_tab = ClassConverter.ConvertToBinary(val,nbrOutputs());
            update_output();
        }

        public override void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            if (inputStack.Children.Count == 4 && OutputStack.Children.Count > 2)
            {
                for (int i = OutputStack.Children.Count; i > 2; i--)
                {
                    terminal = null;
                    Wireclass wire = null;

                    foreach (Terminal tmp in OutputStack.Children)
                    {
                        terminal = tmp;
                    }
                    foreach (Wireclass tmp in terminal.wires)
                    {
                        wire = tmp;
                    }
                    if (wire != null) wire.Destroy();
                    OutputStack.Children.Remove(terminal);
                    try
                    {
                        outputs_tab.RemoveAt(1);
                    }
                    catch { }

                }
            }

            else if (inputStack.Children.Count == 8 && OutputStack.Children.Count < 3)
            {
                for (int i = OutputStack.Children.Count; i < 3; i++)
                {
                    terminal = new Terminal();
                    terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                    terminal.IsOutpt = true;
                    OutputStack.Children.Add(terminal);
                    outputs_tab.Add(false);
                }
            }

           _nbroutput = OutputStack.Children.Count;

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

