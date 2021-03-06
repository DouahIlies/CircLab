﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication9.ComplexComponent;
using WpfApplication9.Component;


namespace WpfApplication9.ComplexComponent
{
    class Comparateur : StandardComponent
    {

        public Comparateur(int nbrinput, int nbroutput)
            : base(nbrinput, nbroutput,0, "M0.5,0.5L48.524,0.5L48.524,153.232L0.5,153.232z", "COMPARATOR")
        {
            TypeLabel.Text = "Comp";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "A < B";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "A == B";
            ((Terminal)OutputStack.Children[2]).terminal_grid.ToolTip = "A > B";
        }

        public override void Run()
        {
            outputs_tab.Clear ();
            update_input();

            ArrayList inputs_tab_1 = new ArrayList();
            ArrayList inputs_tab_2 = new ArrayList();
            int cpt = (inputs_tab.Count + 1) / 2; 
            for (int i = 0; i < cpt ; i++)
            {
                inputs_tab_1.Add(inputs_tab[i]);
            }

            for (int i = cpt ; i < inputs_tab.Count; i++)
            {
                inputs_tab_2.Add(inputs_tab[i]);
            }

            int val1 = ClassConverter.ConvertToInt(inputs_tab_1);
            int val2 = ClassConverter.ConvertToInt(inputs_tab_2);
            
            if (val1 < val2)
            {
                outputs_tab.Add(true);
                outputs_tab.Add(false);
                outputs_tab.Add(false);
            }
            else
            {
                if (val1 > val2)
                { 
                    outputs_tab.Add(false);
                    outputs_tab.Add(false);
                    outputs_tab.Add(true);
                }
            else // val1 == val 2 
                {
                    outputs_tab.Add(false);
                    outputs_tab.Add(true);
                    outputs_tab.Add(false);
                }
            }

            update_output();
        }
    }
}