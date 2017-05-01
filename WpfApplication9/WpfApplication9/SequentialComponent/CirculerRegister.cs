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
    class CirculerRegister : StandardComponent
    {
        public enum TriggerType //les options pour l'horloge 
        {
            RisingEdge, FallingEdge, HighLevel, LowLevel
        }
        public enum Type //type du decalage 
        {
            Right, Left
        }
        private Type _type;
        public Type typeDec
        {
            get { return _type; }
            set { _type = value; }
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
        public CirculerRegister(TriggerType trigger, int nbrinput, Type ty) //constructeur de la Class 
            : base(nbrinput, nbrinput, 3, "M 0,0 L 30,0 L 30,30 L 0,30 z", "CirculaRegister")
        {
            _nbroutputs = nbrinput;
            _trigger = trigger;
            _type = ty;

            oldClockValue = false;//initialiser l'entrée de l'horloge 
            outputs_tab.Clear(); //initialiser les sorties 
            for (int i = 0; i < _nbroutputs; i++)
            {
                outputs_tab.Add(false);
            }
            TypeLabel.Text = "CReg";
            ((Terminal)selectionStack.Children[0]).terminal_grid.ToolTip = "Clock";
            ((Terminal)selectionStack.Children[1]).terminal_grid.ToolTip = "Clear";
            ((Terminal)selectionStack.Children[2]).terminal_grid.ToolTip = "Load";

        }

        public override void Run()//methode pour la mise à jour des resultats 
        {
            //selections_tab[0]==clock
            //selections_tab[1]==clear
            //selections_tab[2]==load

            bool _val;
            update_input(); //mise à jour des entrées 
            newClockValue = (bool)selections_tab[0]; //recuperer la valeur de l'horloge 
            if ((bool)selections_tab[1] == true) { for (int k = 0; k < _nbroutputs; k++) outputs_tab[k] = false; }//clear 
            else
            {
                if ((bool)selections_tab[2] == true) { for (int k = 0; k < _nbroutputs; k++) outputs_tab[k] = inputs_tab[k]; } //load
                else
                {

                    if ((_trigger == TriggerType.RisingEdge && newClockValue == true && oldClockValue == false) || //front montant 
                        (_trigger == TriggerType.FallingEdge && newClockValue == false && oldClockValue == true) ||//front descendant 
                        (_trigger == TriggerType.HighLevel && newClockValue == true) || //etat haut 
                        (_trigger == TriggerType.LowLevel && newClockValue == false)) //etat bas 

                    {
                        if (_type == Type.Left) //decalage gauche 
                        {
                            _val = (bool)outputs_tab[0];
                            for (int k = 0; k < _nbroutputs - 1; k++) { outputs_tab[k] = outputs_tab[k + 1]; }
                            outputs_tab[_nbroutputs - 1] = _val;
                        }
                        if (_type == Type.Right) //decalage droit 
                        {
                            _val = (bool)outputs_tab[_nbroutputs - 1];
                            for (int k = _nbroutputs - 1; k > 0; k--) { outputs_tab[k] = outputs_tab[k - 1]; }
                            outputs_tab[0] = _val;
                        }
                    }
                }

            }
            oldClockValue = newClockValue; //sauvegarde de l'etat de l'horloge 

            update_output(); //mise à jour des resultats 
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

            if (nbrInput != OutputStack.Children.Count)
            {
                while (OutputStack.Children.Count < nbrInput)
                {
                    terminal = new Terminal();
                    terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                    terminal.IsOutpt = true;
                    OutputStack.Children.Add(terminal);
                    outputs_tab.Add(false);
                }
                while (OutputStack.Children.Count > nbrInput)
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
            typeComponenet.Height = terminal.Height * nbrInput;
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

