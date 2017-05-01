using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CircLab.Component;


namespace CircLab.LogicGate
{
   
    class Output : StandardComponent
    {

        public Output()
            : base(1,0,0, "M0,10a10,10 0 1,0 20,0a10,10 0 1,0 -20,0", "OUTPUT")
        {
            state = false;
            typeComponenet.Width = typeComponenet.Height;
            this.typeComponenet.Fill = Brushes.Red;
            OutputStack.Children.Clear();
            TypeLabel.Text = "";

        }


        private Boolean _state;
        public Boolean state
        {
            get { return _state; }
            set
            {
                _state = value;
                
                if (state==true)
                {
                    this.typeComponenet.Fill = Brushes.Green;
                }
                else
                {
                    this.typeComponenet.Fill = Brushes.Red;
                }
            }
        }

        public override void Run()
        {
           foreach(Terminal terminal in inputStack.Children)
            {
                foreach(Wireclass wire in terminal.wires)
                {
                    state = wire.state;
                }
            }
        }


    }
}
