using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class FlipFlop : StandardComponent
    {
        public enum TriggerType
        {
            RisingEdge, FallingEdge, HighLevel, LowLevel
        }
        
        private bool _val;
        public TriggerType _trigger = TriggerType.RisingEdge;
        public TriggerType Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }
        private bool oldClockValue;
        public FlipFlop(TriggerType trigger)
            :base(2,2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z","FlipFlop")
        {
            TypeLabel.Text = "FlipFlop";
            _trigger = trigger;
            oldClockValue = false;
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }
        }

        public override void Run()
        {
            update_input();
            bool newClockValue = (bool)inputs_tab[1];
            _val = (bool)inputs_tab[0];
            if ((_trigger == TriggerType.RisingEdge && newClockValue == true && oldClockValue == false) ||
                (_trigger == TriggerType.FallingEdge && newClockValue == false && oldClockValue == true) ||
                (_trigger == TriggerType.HighLevel && newClockValue == true) ||
                (_trigger == TriggerType.LowLevel && newClockValue == false))
            {
                
                outputs_tab[0] =  _val;
                outputs_tab[1] = !_val;
                
            }
            else
            {
                outputs_tab[0] =  _val;
                outputs_tab[1] = !_val;
            }
            oldClockValue = newClockValue;
            update_output();
        }
    }
}
