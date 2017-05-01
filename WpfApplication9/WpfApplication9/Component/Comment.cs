using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CircLab.Component
{
    class Comment : StandardComponent
    {
        public Comment(string comment) : base(0, 0, 0, "", "Comment")
        {
            Label.Text = comment;
            
        }

        public override void Run() { }
    }
}
