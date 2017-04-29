using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CircLab.Component;


namespace CircLab.LogicGate
{
    class XOR : StandardComponent
    {

        public XOR(int nbrinput)
            : base(nbrinput,1,0, "M 13,47 c 5,-10 5,-20 0,-30 M 13,17 c 5,10 5,20 0,30 M 18,17 h 7 c 10,0 20,5 25,15 c -5,10 -15,15 -25,15 h -7 c 5,-10 5,-20 0,-30","XNOR")
        {
            typeComponenet.StrokeThickness = 2;
            TypeLabel.Text = "Xor";
        }

        public override void Run()
        {

            update_input();
            outputs_tab[0]=(false);
            if ((bool)inputs_tab[0] != (bool)inputs_tab[1]) outputs_tab[0] = true;
            else outputs_tab[0] = false;
          
            update_output();
        }

        
    }
}

