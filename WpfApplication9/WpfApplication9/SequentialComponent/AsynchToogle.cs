using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class AsynchToogle : StandardComponent //heritage de standardComponent 
    {


        private bool _val; // recuperer la valeur de T


        public AsynchToogle()
            : base(1, 2, 0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "AsynchToogle") // constructeur 
        {
            outputs_tab.Clear(); //initialiser les sorties 
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }

            TypeLabel.Text = "T";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "T";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "Q";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "not Q";
        }

        public override void Run() //methode qui se charge des résultas 
        {
            update_input(); //mettre à jour les entrées 

            bool Tvalue = (bool)inputs_tab[0]; // recuperer la valeure de T
            _val = (bool)outputs_tab[1]; // recuperer la valeur de Q
            if (Tvalue == true) // T à 0 
            {

                outputs_tab[0] = _val;
                outputs_tab[1] = !_val;
            }
            else
            {
                outputs_tab[0] = !_val;
                outputs_tab[1] = _val;
            }
            update_output(); //mettre à jour les sorties 
        }



    }
}
