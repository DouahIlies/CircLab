using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.ComplexComponent;
using CircLab.Component;


namespace CircLab.ComplexComponent
{
    class HalfAdder : StandardComponent
    {
        public HalfAdder(int nbrinput,int nbroutput)
            : base(2,2,0, "M0.5,0.5L54.378,0.5L54.378,88.036L0.5,88.036z", "HALFADD")
        {
            TypeLabel.Text = "HalfAdder";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "A";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "B";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "Cout";
        }

        public override void Run()
        {
            bool tmp = false ;
            update_input();
            outputs_tab.Clear();
          
            if(!inputs_tab[0].Equals(inputs_tab[1]))
            {
                tmp = true;
            } 
            //System.Windows.MessageBox.Show(tmp.ToString());
            outputs_tab.Add(tmp);
            outputs_tab.Add(((bool)inputs_tab[0]) && ((bool)inputs_tab[1]));
            update_output();
        }



    }
}
