using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;


namespace CircLab.ComplexComponent
{
    class FullAdder : StandardComponent
    {

        public FullAdder(int nbrinput, int nbroutput)
            : base(3, 2, 0, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "FULLADD")
        {
            TypeLabel.Text = "FullAdder";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)inputStack.Children[2]).terminal_grid.ToolTip = "Cin";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "Cout";
        }

        public override void Run()
        {
            update_input();
            int val = 0;
            for(int i=0; i <= 2; i++)
            {
                if ((bool)inputs_tab[i]) val ++ ;
            }
            outputs_tab = ClassConverter.ConvertToBinary(val,2);
            update_output();

        }



    }
}