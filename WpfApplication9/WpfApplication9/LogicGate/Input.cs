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
    class Input : StandardComponent
    {
    
        public Input()
            : base(0,1,0, "M0.5,0.5 L27,0.5 L27,27.5 L0.5,27.5 z","INPUT")
        {
            state = false;
            this.typeComponenet.Fill = Brushes.Red;
            typeComponenet.MouseLeftButtonUp += this.MouseClick;
            TypeLabel.Text = "Input";
        }


        private Boolean _state;
        public Boolean state
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Terminal terminal = (Terminal)OutputStack.Children[0];
                        foreach(Wireclass wire in terminal.wires)
                        {
                            wire.state = _state ;
                        }
                    
                }
                _state = value;
            }
        }

        private void MouseClick(object sender, RoutedEventArgs e)
        {
            state = !state;
            if (state==true)
            {
                this.typeComponenet.Fill = Brushes.Green;
            }
            else
            {
                this.typeComponenet.Fill = Brushes.Red;
            }
        }

        public override void Run()
        {
          
            foreach(Wireclass wire in output.wires)
            {
                wire.state = this.state;
            }

        }




    }
}
