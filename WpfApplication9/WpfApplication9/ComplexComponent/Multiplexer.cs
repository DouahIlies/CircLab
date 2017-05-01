using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CircLab.Component;
using CircLab.LogicGate;
using CircLab.SequentialComponent;

namespace CircLab.ComplexComponent
{
    class Multiplexer : StandardComponent
    {
        public Multiplexer(int nbrinput, int nbroutput ,int nbrselection)

            : base(nbrinput,1,nbrselection, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "MUX")
        {
            TypeLabel.Text = "Mux";

         
        }
        public override void Run()
        {
            update_input();
            outputs_tab.Clear();
            
            int val = ClassConverter.ConvertToInt(selections_tab);
            outputs_tab.Add(inputs_tab[val]);
            update_output();
        }
    }
}
