using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.ComplexComponent
{
    class Encodeur : StandardComponent
    {
        public Encodeur(int nbrinput, int nbroutput)
      
            : base(nbrinput,nbroutput,0," M49.7560975609756, 123.658536585366L98.2805836911085, 145.10929491592 98.2805836911085, 255.595622936389 49.2561883954712, 276.082695793093z", "ENCODEUR")
        {
            TypeLabel.Text = "Coder";
        }

        public override void Run()
        {
            
            update_input();
            outputs_tab.Clear();
            int val = 0;
            while ((val < inputs_tab.Count) && ((bool)inputs_tab[val]==false))  { val ++; }
            if (val == inputs_tab.Count) val = 0 ;
            outputs_tab = ClassConverter.ConvertToBinary(val,nbrOutputs());
            update_output();
        }
    }
}
