using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class AsynchToogle : StandardComponent
    {


        private bool _val;

       
        public AsynchToogle()
            :base(1,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "AsynchToogle")
        {
            TypeLabel.Text = "AsynchToggle";
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }
        }

        public override void Run()
        {
            update_input();
         
            bool Tvalue = (bool)inputs_tab[0];
            _val = (bool)outputs_tab[1];
            if (Tvalue == true)
            {
               
                outputs_tab[0] = _val;
                outputs_tab[1] = !_val;
            }
            else
            {
                outputs_tab[0] = !_val;
                outputs_tab[1] = _val;
            }
            update_output();
        }


    
    }
}
