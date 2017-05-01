using CircLab.Component;
using System;
using System.Windows;
using System.Windows.Media;

namespace CircLab.SequentialComponent
{
    class Registre : StandardComponent
    {
        public enum TriggerType //type du changement 
        {
            RisingEdge, FallingEdge, HighLevel, LowLevel
        }

        private int _nbroutput; //contenir le nombre de sorties 
        public TriggerType _trigger;//contenir le type du changement 
        public TriggerType Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }
        private bool oldClockValue;//contenir l'ancienne valeur de l'horloge 
        private bool newClockValue;//contenir la nouvelle valeur de l'horloge 

        public Registre(TriggerType trigger, int nbrinput)//constructeur de la calss 
            : base(nbrinput, nbrinput, 2, "M 0,0 L 30,0 L 30,30 L 0,30 z", "Register")
        {
            _nbroutput = nbrinput;
            _trigger = trigger;
            TypeLabel.Text = "Registre";
            oldClockValue = false;//initialiser l'horloge 
            outputs_tab.Clear();//initialiser les sorties
            for (int i = 0; i < _nbroutput; i++)
            {
                outputs_tab.Add(false);
            }
            TypeLabel.Text = "Reg";
            ((Terminal)selectionStack.Children[0]).terminal_grid.ToolTip = "Clear";
            ((Terminal)selectionStack.Children[1]).terminal_grid.ToolTip = "Load";
        }

        public override void Run()
        {
            //selections_tab[0]==clear
            //selections_tab[1]==clock

            update_input();//mettre à jour les entrées
            newClockValue = (bool)selections_tab[1]; //recuperer la nouvelle valeur de l'horloge 
            if ((bool)selections_tab[0] == true) { for (int k = 0; k < _nbroutput; k++) outputs_tab[k] = false; }//clear
            else
            {

                if ((_trigger == TriggerType.RisingEdge && newClockValue == true && oldClockValue == false) || //front montant 
                    (_trigger == TriggerType.FallingEdge && newClockValue == false && oldClockValue == true) || //front descendant 
                    (_trigger == TriggerType.HighLevel && newClockValue == true) || //etat haut 
                    (_trigger == TriggerType.LowLevel && newClockValue == false)) //etat bas 
                {
                    for (int i = 0; i < _nbroutput; i++) //load
                    { outputs_tab[i] = inputs_tab[i]; }
                }

            }
            oldClockValue = newClockValue; //sauvegarde de l'etat de l'horloge
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

            if(nbrInput!=OutputStack.Children.Count)
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

    








