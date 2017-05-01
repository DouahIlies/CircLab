using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;
using System.Windows.Media;
using System.Windows;

namespace CircLab.ComplexComponent
{
    class Decodeur : StandardComponent
    {
        private int _nbroutput;
        public Decodeur(int nbrinput, int nbroutput)
            : base(nbrinput, nbroutput, 0, "M49.7560975609756,145.609756097561L98.2805836911085,123.890000372166 98.2805836911085,276.81536219976 49.2561883954712,255.596155901475z", "DECODEUR")
        {
            _nbroutput = nbroutput;

            TypeLabel.Text = "Dec";
        }

        public override void Run()
        {
            update_input();
            outputs_tab.Clear();
            for (int i = 0; i < _nbroutput; i++)
            {
                outputs_tab.Add(false);
            }

            int val = ClassConverter.ConvertToInt(inputs_tab);
            outputs_tab[val] = true;
            update_output();
        }

        public override void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            if(inputStack.Children.Count == 2 && OutputStack.Children.Count>4)
            {
                for (int i = OutputStack.Children.Count; i > 4; i--)
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
                    try {
                        outputs_tab.RemoveAt(1);
                    }
                    catch { }

                 }
            }

            else if(inputStack.Children.Count ==3 && OutputStack.Children.Count<8)
            {
                for (int i = OutputStack.Children.Count; i < 8; i++)
                {
                    terminal = new Terminal();
                    terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                    terminal.IsOutpt = true;
                    OutputStack.Children.Add(terminal);
                    outputs_tab.Add(false);
                }
            }

            _nbroutput = OutputStack.Children.Count;

            grid.Height = _nbroutput * 22 + 25;
            typeComponenet.Height =  terminal.Height * _nbroutput;
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
