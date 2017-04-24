using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;

namespace CircLab.ComplexComponent
{
    class Demultiplexer : StandardComponent
    {
        public Demultiplexer(int nbrinput, int nbroutput, int nbrselection)
            : base(1,nbroutput,nbrselection, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "DEMUX")
        {
            TypeLabel.Text = "Demux";
        }

        public override void Run()
        {

            update_input();
            outputs_tab.Clear();
            
            for (int i = 0; i < nbrOutputs(); i++)
            {
                outputs_tab.Add(false);
            }
            int val = ClassConverter.ConvertToInt(selections_tab);
            outputs_tab[val] = inputs_tab[0];
            update_output();
        }
    }
}
