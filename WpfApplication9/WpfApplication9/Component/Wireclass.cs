using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication9.Component
{

    public class Wireclass
    {
        public Line l1 { get; set; }
        public Line l2 { get; set; }
        public Line l3 { get; set; }

        public static Boolean selected = false;
        public static Point btn1Point;
        public static Point btn2Point;
        public static Ellipse btn11;
        public static Ellipse btn22;
        public static Canvas myCanvas;
        public static Window mwindow;

        public static Terminal source;
        public static Terminal destination;

       
        public ArrayList destinations;
        public double x1;
        public double x2;
        public double y1;
        public double y2;

       

        public Ellipse btn111;
        public Ellipse btn222;


        private  Boolean _state;
        public Boolean state
        {
            get { return _state; }
            set
            {
                if (_state !=value)
                {
                    _state = value;
                    foreach(StandardComponent standardcomponent in destinations)
                    {
                        standardcomponent.Run();
                        //destination.etat = value;
                    }

                   // Class1.TryFindParent<StandardComponent>(destination).Run();
                }
                _state = value;
            }
        }


        public Wireclass()
        {
            destinations = new ArrayList();
        }

        public void relier(Ellipse btn)
        {

            if (!selected)
            {
                btn11 = btn;   
                selected = true;
            }
            else
            {   

                btn22 = btn;
                destination = Class1.TryFindParent<Terminal>(btn22);
                source = Class1.TryFindParent<Terminal>(btn11);

                if (source.source==false)
                {
                    if (destination.source==true)
                    {
                        btn22 = btn11;
                        btn11 = btn;
                        Terminal temp = destination;
                        destination = source;
                        source = temp;
                    }
                    else
                    {
                        selected = false;
                        return;
                    }
                }
                else
                {
                    if (destination.source==true)
                    {
                        selected = false;
                        return;
                    }
                }

                if (destination.wires.Count>=1)
                {
                    selected = false;
                    return;
                }

                btn111 = btn11;
                btn222 = btn22;
                destination.wires.Add(this);
                source.wires.Add(this);
             
                
                //state = source.etat;
                btn2Point = btn22.TransformToAncestor(myCanvas).Transform(new Point(0, 0));
                btn1Point = btn11.TransformToAncestor(myCanvas).Transform(new Point(0, 0));
                l1 = new Line();
                l1.Stroke = new SolidColorBrush(Colors.White);
                l1.StrokeThickness = 2.0;
                l1.X1 = btn1Point.X + 0/*btn11.ActualWidth*/;
                l1.X2 = (btn1Point.X + btn2Point.X + 2*btn11.ActualWidth) / 2;
                l1.Y1 = btn1Point.Y+ btn11.ActualHeight / 2;
                l1.Y2 = btn1Point.Y + btn11.ActualHeight / 2;
                myCanvas.Children.Add(l1);
                l2 = new Line();
                l2.Stroke = new SolidColorBrush(Colors.White);
                l2.StrokeThickness = 2.0;
                l2.X1 = l1.X2;
                l2.X2 = l1.X2;
                l2.Y1 = l1.Y1;
                l2.Y2 = btn2Point.Y -btn22.ActualHeight/2;
                myCanvas.Children.Add(l2);

                l3 = new Line();
                l3.Stroke = new SolidColorBrush(Colors.White);
                l3.StrokeThickness = 2.0;
                l3.X1 = l2.X2;
                l3.X2 = (btn2Point.X);//btn2Point.X;
                l3.Y1 = l2.Y2;
                l3.Y2 = l2.Y2;
                
                myCanvas.Children.Add(l3);

                Class1.TryFindParent<StandardComponent>(source).Run(); 
                //partie noued
               /* foreach(Wireclass in Wire)


                if (source.wires.Count>1)
                {
                    Ellipse noued = new Ellipse();
                    Canvas.SetTop(noued, l2.Y1 - btn22.ActualHeight / 2);
                    Canvas.SetLeft(noued, l2.X1 - btn22.ActualHeight / 2);
                    noued.Fill = Brushes.White;
                    noued.Height = 10;
                    noued.Width = 10;
                    
                    myCanvas.Children.Add(noued); 
                }*/
                
                selected = false;
                
                Class1.TryFindParent<StandardComponent>(destination).Run();
                destinations.Add(Class1.TryFindParent<StandardComponent>(destination));
            }
        }


        public void recalculer(Boolean source)
        {
           
          //  if (!source)
          //  {
                btn1Point = btn111.TransformToAncestor(myCanvas).Transform(new Point(0, 0));
                x1 = btn1Point.X;
                y1 = btn1Point.Y;
         /*   }
            else
            {*/
                btn2Point = btn222.TransformToAncestor(myCanvas).Transform(new Point(0, 0));
                x2 = btn2Point.X;
                y2 = btn2Point.Y;
            // }
            l1.Stroke = new SolidColorBrush(Colors.Black);
            l1.X1 = x1 + 0/*btn11.ActualWidth*/;
            l1.X2 = (x1 + x2 + 2 * btn11.ActualWidth) / 2;
            l1.Y1 = y1 + btn11.ActualHeight / 2;
            l1.Y2 = y1 + btn11.ActualHeight / 2;
            //myCanvas.Children.Add(l1);
            //l2 = new Line();
            l2.Stroke = new SolidColorBrush(Colors.Black);
           // l2.StrokeThickness = 2.0;
            l2.X1 = l1.X2;
            l2.X2 = l1.X2;
            l2.Y1 = l1.Y1;
            l2.Y2 = y2 - btn22.ActualHeight / 2;
            //myCanvas.Children.Add(l2);

            //l3 = new Line();
            l3.Stroke = new SolidColorBrush(Colors.Black);
            //l3.StrokeThickness = 2.0;
            l3.X1 = l2.X2;
            l3.X2 = (x2);//btn2Point.X;
            l3.Y1 = l2.Y2;
            l3.Y2 = l2.Y2;
            //myCanvas.Children.Add(l3);
           // selected = false;
        }

       


    }


}