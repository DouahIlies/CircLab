using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;
using CircLab.ComplexComponent;
using System.Windows.Media;
using System.Windows;

namespace CircLab.SequentialComponent
{
    class DecompteurModN : StandardComponent
    {

        private int _nbroutputs;
        public int Nbroutputs
        {
            get { return _nbroutputs; }
            set { _nbroutputs = value; }
        }
        private int _val;
        public int Val
        {
            get { return _val; }
            set { _val = value; }
        }
        private bool oldClockValue;
        public DecompteurModN(int N, int nbr)
            :base(1,nbr,0, "M 0,0 L 30,0 L 30,30 L 0,30 z","frequencyDivider")
        {
            TypeLabel.Text = "Decompteur";
            _nbroutputs = nbr;
            outputs_tab = ClassConverter.ConvertToBinary(N-1,_nbroutputs);
            _val = N;
            oldClockValue = false;
            TypeLabel.Text = "DecModN";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "Clock";
        }

        public override void Run()
        {
            update_input();
            bool newClockValue = (bool)inputs_tab[0];
            if (newClockValue == true && oldClockValue == false)
            {
                int number = ClassConverter.ConvertToInt(outputs_tab);
                number--;
                if (number == -1) { number = _val-1; }
                outputs_tab = ClassConverter.ConvertToBinary(number,_nbroutputs);
            }
            oldClockValue = newClockValue;
            update_output();
        }

        public override void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            if (_nbroutputs > OutputStack.Children.Count)
            {
                for (int i = OutputStack.Children.Count; i < _nbroutputs; i++)
                {
                    terminal = new Terminal();
                    terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                    terminal.IsOutpt = true;
                    OutputStack.Children.Add(terminal);
                    outputs_tab.Add(false);
                }
            }
            else if (_nbroutputs < OutputStack.Children.Count)
            {
                for (int i = OutputStack.Children.Count; i > _nbroutputs; i--)
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
                    outputs_tab.RemoveAt(1);
                }
            }


            grid.Height = _nbroutputs * 22 + 25;
            typeComponenet.Height = terminal.Height * _nbroutputs;
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


