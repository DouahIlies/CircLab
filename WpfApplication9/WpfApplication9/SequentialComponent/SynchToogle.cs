using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class SynchToogle : StandardComponent
    {
      

        private bool _val;
        
        private bool oldClockValue;
        public SynchToogle()
            :base(2,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "SynchToogle")
        {
            TypeLabel.Text = "SynchToggle";
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }

            oldClockValue = false;

            TypeLabel.Text = "ST";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "T";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "Clock";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "Q";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "not Q";
        }

        public override void Run()
        {
            
            update_input();
            bool newClockValue = (bool)inputs_tab[1];
            _val = (bool)outputs_tab[1];
            bool Tvalue = (bool)inputs_tab[0];
            if (( newClockValue == true && oldClockValue == false && Tvalue==true))
            {
                
                outputs_tab[0] = _val;
                outputs_tab[1] = !_val;
            }
            else
            {
                outputs_tab[0] = !_val;
                outputs_tab[1] = _val;
            }
            oldClockValue = newClockValue;
            update_output();
        }


    }
}


