using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;

namespace CircLab.ComplexComponent
{
    class Decodeur:StandardComponent
    {
        private int _nbroutput;
        public Decodeur(int nbrinput,int nbroutput)
            :base(nbrinput,nbroutput,0, "M49.7560975609756,145.609756097561L98.2805836911085,123.890000372166 98.2805836911085,276.81536219976 49.2561883954712,255.596155901475z", "DECODEUR") 
        {    
            _nbroutput = nbroutput;

            TypeLabel.Text = "Dec";
        }

        public override void Run()
        {
            update_input();
            outputs_tab.Clear();
            for (int i = 0; i<_nbroutput; i++)
            {
                outputs_tab.Add(false);
            }

            int val = ClassConverter.ConvertToInt(inputs_tab);
            outputs_tab[val] = true;
            update_output();
        }
    }
}
