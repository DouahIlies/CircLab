
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;
using System.Windows.Media;
using System.Windows;

namespace CircLab.SequentialComponent
{
    class programmablRegister : StandardComponent
    {
        public enum TriggerType //type de changement 
        {
            RisingEdge, FallingEdge, HighLevel, LowLevel
        }

        private int _nbroutputs;
        public TriggerType _trigger = TriggerType.RisingEdge;
        public TriggerType Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }
        private bool oldClockValue;
        private bool newClockValue;

        public programmablRegister(TriggerType trigger, int nbrinput)
            : base(nbrinput + 2, nbrinput, 4, "M 0,0 L 30,0 L 30,30 L 0,30 z", "ProgrammablRegister")
        {
            _trigger = trigger;
            _nbroutputs = nbrinput;
            path= "M 0,0 L 30,0 L 30,30 L 0,30 z";
            TypeLabel.Text = "ProgrammableRegister";
            oldClockValue = false;//initialiser l'horloge 
            outputs_tab.Clear();//initialiser les sorties 
            for (int i = 0; i < _nbroutputs; i++)
            {
                outputs_tab.Add(false);
            }
            TypeLabel.Text = "PReg";
            ((Terminal)selectionStack.Children[0]).terminal_grid.ToolTip = "Clear";
            ((Terminal)selectionStack.Children[1]).terminal_grid.ToolTip = "Clock";
            ((Terminal)selectionStack.Children[2]).terminal_grid.ToolTip = "Cmd1";
            ((Terminal)selectionStack.Children[3]).terminal_grid.ToolTip = "Cmd2";
        }

        public override void Run()
        {
            //selections_tab[0]==clear
            //selections_tab[1]==clock 
            //selections_tab[2]==so
            //selections_tab[3]==s1
            //inputs_tab[0]==eg
            //inputs_tab[count-1]==ed

            update_input();//mettre à jour les entrées 
            newClockValue = (bool)selections_tab[1];//contenir la nouvelle valeur de l'horloge
            if ((bool)selections_tab[0] == true) { for (int k = 0; k < _nbroutputs; k++) outputs_tab[k] = false; }//clear
            else
            {

                if ((_trigger == TriggerType.RisingEdge && newClockValue == true && oldClockValue == false) || //front montant
                    (_trigger == TriggerType.FallingEdge && newClockValue == false && oldClockValue == true) || //front descendant
                    (_trigger == TriggerType.HighLevel && newClockValue == true) || //etat haut
                    (_trigger == TriggerType.LowLevel && newClockValue == false)) //etat bas 
                {
                    if ((bool)selections_tab[2] == true && (bool)selections_tab[3] == true) //load
                    {
                        for (int i = 0; i < _nbroutputs; i++)
                        { outputs_tab[i] = inputs_tab[i + 1]; }
                    }
                    if ((bool)selections_tab[2] == true && (bool)selections_tab[3] == false) ///decalage gauche 
                    {
                        for (int i = 0; i < _nbroutputs - 1; i++) { outputs_tab[i] = outputs_tab[i + 1]; }
                        outputs_tab[_nbroutputs - 1] = inputs_tab[inputs_tab.Count - 1];
                    }
                    if ((bool)selections_tab[2] == false && (bool)selections_tab[3] == true)//decalage droit 
                    {
                        for (int i = _nbroutputs - 1; i > 0; i--) { outputs_tab[i] = outputs_tab[i - 1]; }
                        outputs_tab[0] = inputs_tab[0];
                    }
                }

            }
            oldClockValue = newClockValue;
            update_output();//mettre à jour les sorties 
        }

        public override void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            int nbrInput;
            foreach (Terminal tmp in inputStack.Children)
            {
                terminal = tmp;
            }

            if (this.nbrInputs() == 0)
            {
                nbrInput = 1;
            }
            else nbrInput = this.nbrInputs();

            if (nbrInput-2 != OutputStack.Children.Count)
            {
                while (OutputStack.Children.Count < nbrInput-2)
                {
                    terminal = new Terminal();
                    terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                    terminal.IsOutpt = true;
                    OutputStack.Children.Add(terminal);
                    outputs_tab.Add(false);
                }
                while (OutputStack.Children.Count > nbrInput-2)
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

            output.Margin = new Thickness(4.5, 0, 4.5, 0);
            grid.Height = nbrInput * 22 + 25;
            typeComponenet.Height = terminal.Height * (nbrInput+1);
            typeComponenet.Width = terminal.Width * 6;

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
