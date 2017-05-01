using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class RSLatche : StandardComponent
    {


        private bool _val1,_val2,_val;


        public RSLatche()
            :base(2,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "RSLatche")
        {
            TypeLabel.Text = "RSHLatche";
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }
            TypeLabel.Text = "RS";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "R";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "S";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "Q";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "not Q";


        }

        public override void Run()
        {
            update_input(); 
            _val = (bool)outputs_tab[0];
            _val1 = (bool)inputs_tab[0];
            _val2 = (bool)inputs_tab[1];
            if (_val1 == false && _val2 == true)
            {
                
                outputs_tab[0] = true;
                outputs_tab[1] = false;
            }
            if(_val1 == true && _val2 == false)
            {
                outputs_tab[0] = false;
                outputs_tab[1] = true;
            }
            if (_val1 == false && _val2 == false)
            {
                outputs_tab[0] =_val ;
                outputs_tab[1] = !_val;
            }
            if (_val2 == true && _val1 == true) throw new ArgumentException("cas interdit !");

            update_output();
        }
    
    }
}
