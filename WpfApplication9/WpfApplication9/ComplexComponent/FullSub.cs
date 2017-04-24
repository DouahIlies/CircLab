using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;

namespace CircLab.ComplexComponent
{
    class FullSub : StandardComponent
    {

        public FullSub(int nbrinput, int nbroutput)
            : base(3, 2, 0, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "FULLSUB")
        {
            TypeLabel.Text = "FullSub";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "Cout";
        }

        public override void Run()
        {
            outputs_tab.Clear();
            update_input();
            outputs_tab.Add((bool)inputs_tab[0] ^ ((bool)inputs_tab[1] ^ (bool)inputs_tab[2]));
            bool tmp1 = (!(bool)inputs_tab[0]) && ((bool)inputs_tab[1]);
            bool tmp2 = (!((bool)inputs_tab[0] ^ (bool)inputs_tab[1])) && ((bool)inputs_tab[2]);
            bool tmp = ((tmp1) || (tmp2)); 
        
            outputs_tab.Add(tmp);
            update_output();

        }



    }
}
