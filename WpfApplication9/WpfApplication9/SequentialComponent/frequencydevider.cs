﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication9.Component;

namespace WpfApplication9.SequentialComponent
{
    class FrequencyDevider : StandardComponent
    {
        

        
        private bool oldClockValue,_val;
        public FrequencyDevider()
            :base(1,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z","frequencyDivider")
        {
            outputs_tab.Clear();
            outputs_tab.Add(false);
            outputs_tab.Add(true);
            oldClockValue = false;
        }

        public override void Run()
        {
            update_input();
            _val = (bool)outputs_tab[0];
            bool newClockValue = (bool)inputs_tab[0];
            if ( newClockValue == true && oldClockValue == false)
            {

                outputs_tab[0] = !_val;
                outputs_tab[1] = _val;

            }
            oldClockValue = newClockValue;
            update_output();
        }
    }
}
   