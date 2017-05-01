using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CircLab.Component;

namespace CircLab.SequentialComponent
{
    class JK : StandardComponent
    {
        public enum TriggerType
        {
            RisingEdge, FallingEdge
        }

        private bool _val1, _val2, _out1, _out2;
        private TriggerType _trigger = TriggerType.RisingEdge;
        public TriggerType Trigger
        {
            get { return _trigger; }
            set { _trigger = value; }
        }
        private bool oldClockValue;
        public JK(TriggerType trigger)
            : base(3, 2,0, "M 0,0 L 30,0 L 30,30 L 0,30 z", "JK")
        {
            _trigger = trigger;
            outputs_tab.Clear();
            for (int i = 0; i < 2; i++)
            {
                outputs_tab.Add(false);
            }
            oldClockValue = false;

            TypeLabel.Text = "JK";
            ((Terminal)inputStack.Children[0]).terminal_grid.ToolTip = "J";
            ((Terminal)inputStack.Children[1]).terminal_grid.ToolTip = "K";
            ((Terminal)inputStack.Children[2]).terminal_grid.ToolTip = "Clock";
            ((Terminal)OutputStack.Children[0]).terminal_grid.ToolTip = "Q";
            ((Terminal)OutputStack.Children[1]).terminal_grid.ToolTip = "not Q";
        }

        public override void Run()
        {
            update_input();
            _val1 = (bool)inputs_tab[0]; _out1 = (bool)outputs_tab[0];
            _val2 = (bool)inputs_tab[1]; _out2 = (bool)outputs_tab[1];
            bool newClockValue = (bool)inputs_tab[2];
            if ((_trigger == TriggerType.RisingEdge && newClockValue == true && oldClockValue == false) ||
                (_trigger == TriggerType.FallingEdge && newClockValue == false && oldClockValue == true))
            {
                if (_val1 == true && _val2 == false) { outputs_tab[0] = true; outputs_tab[1] = false; }
                if (_val1 == false && _val2 == true) { outputs_tab[0] = false; outputs_tab[1] = true; }
                if (_val1 == true && _val2 == true) { outputs_tab[0] = _out2; outputs_tab[1] = _out1; }
                if (_val1 == false && _val2 == false) { outputs_tab[0] = _out1; outputs_tab[1] = !_out1; }
            }
            oldClockValue = newClockValue;
            update_output();
        }
    }

}