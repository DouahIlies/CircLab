﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication9.Component;


namespace WpfApplication9.LogicGate
{
    class Not : StandardComponent
    {
        public Not()
            : base(1,1,0, "M 17,17 v 30 h 15 a 2,2 1 0 0 0,-30 h -15", "AND")
        {

        }

        public override void Run()
        {

            update_input();
            outputs_tab.Add(false);
            outputs_tab[0] = !(Boolean)inputs_tab[0];
            update_output();
        }

    }
}
