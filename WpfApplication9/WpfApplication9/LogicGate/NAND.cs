using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;


namespace CircLab.LogicGate
{
    class NAND : StandardComponent
    {

        public NAND(int nbrinput)
            : base(nbrinput,1,0, "M 15,17 v 30 h 15 a 2,2 1 0 0 0,-30 h -15 M 46,33.5 a 3,3 1 1 1 0.1,0.1","NAND")
        {
            TypeLabel.Text = "Nand";
        }
        


        public override void Run()
        {
            update_input();
           // outputs_tab.Add(false);
            outputs_tab[0] = false;
            foreach (bool tmp in inputs_tab)
            {
                if (tmp == false)
                {
                    outputs_tab[0] = true;
                }
                
            }
            update_output();
        }
    }
}
