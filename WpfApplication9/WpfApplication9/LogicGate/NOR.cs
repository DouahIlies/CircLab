using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;


namespace CircLab.LogicGate
{
    class NOR : StandardComponent
    {

        public NOR(int nbrinput)
            : base(nbrinput,1,0, "M 15,17 h 5 c 10,0 20,5 25,15 c -5,10 -15,15 -25,15 h -5 c 5,-10 5,-20 0,-30 M 46,33.5 a 3,3 1 1 1 0.1,0.1","NOR")
        {
            TypeLabel.Text = "Nor";
        }

        public override void Run()
        {

            update_input();
            outputs_tab[0] = true;
            foreach (bool tmp in inputs_tab)
            {
                if (tmp == true)
                {
                    outputs_tab[0] = false;
                }

            }
            update_output();
        }
    }
}
