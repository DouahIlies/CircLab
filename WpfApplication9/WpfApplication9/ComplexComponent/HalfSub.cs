using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;


namespace CircLab.ComplexComponent
{
    class HalfSub : StandardComponent
    {
        public HalfSub(int nbrinput, int nbroutput)
            : base(nbrinput,nbroutput,0, "M0.5,0.5 L100.189,0.5 L100.189,145.017 L0.5,145.017 z","HALFSUB")
        {
            TypeLabel.Text = "HalfSub";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "Cout";
        }
        public override void Run()
        {
            update_input();
            outputs_tab.Clear();
            
            bool tmp = false ;

            if (!inputs_tab[0].Equals(inputs_tab[1])) { tmp = true; }
            outputs_tab.Add(tmp);
            outputs_tab.Add((!(bool)inputs_tab[0]) && ((bool)inputs_tab[1]));
            update_output () ;
        }


    }
}
