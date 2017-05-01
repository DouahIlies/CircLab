using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;
using System.Windows.Forms;

namespace CircLab.SequentialComponent
{
    class RSLatche : StandardComponent
    {


        private bool _val1,_val2,_val;


        public RSLatche()
            :base(2,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "RSLatche")
        {
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }
            TypeLabel.Text = "RS";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "R";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "Q";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "not Q";


        }

        public override void Run()
        {
            update_input();//mettre à jour les entrées 
            _val = (bool)outputs_tab[0];//recuperer la valeur de Q-
            _val1 = (bool)inputs_tab[0];//recuperer la valeur de R
            _val2 = (bool)inputs_tab[1];//recuperer la valeur de s
            if (_val1 == false && _val2 == true) //R=0 S=1
            {

                outputs_tab[0] = true;
                outputs_tab[1] = false;
            }
            if (_val1 == true && _val2 == false) //R=1 S=0
            {
                outputs_tab[0] = false;
                outputs_tab[1] = true;
            }
            if (_val1 == false && _val2 == false) //R=0 S=0 :loading 
            {
                outputs_tab[0] = _val;
                outputs_tab[1] = !_val;
            }
            if (_val2 == true && _val1 == true) //R=1 S=1 cas interdit 
            {
                validateUserEntry(); //afficher le message d'erreur 
            }

            update_output();//mettre à jour les sorties 
        }
        private void validateUserEntry() //methode pour afficher le message d'erreur 
        {

            // Initializes the variables to pass to the MessageBox.Show method.

            string message = "les entrées R et S ne peuvent pas être à 1 en Même temps ";
            string caption = "Erreur !";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);

        }

    }
}
