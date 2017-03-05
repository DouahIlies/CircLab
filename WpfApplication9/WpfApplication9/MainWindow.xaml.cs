using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication9.Component;
using WpfApplication9.LogicGate;

namespace WpfApplication9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Wireclass.mwindow = mwindow;
            Wireclass.myCanvas = canvas;
            StandardComponent.canvas = canvas;
            //Input input = new Input(2);
           //gg.Children.Add(input.create(2));
           //StandardComponent standard = new StandardComponent(3);
            //gg.Children.Add(standard);
            // gg.Children.Add(standard.create(2, "M 17,17 v 30 h 15 a 2,2 1 0 0 0,-30 h -15"));
            //gg.Children.Add(standard);
            /*img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;*/

        }
        private object movingObject;
        private double firstXPos, firstYPos;
        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            Control img = sender as Control;
            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(img).X;
            firstYPos = e.GetPosition(img).Y;

            movingObject = sender;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            try { foreach (Control child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
            }
            catch (Exception){ }
            Canvas.SetZIndex(img, top + 1);
            Mouse.Capture(img);
        }
        private new void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Control img = sender as Control;
            Canvas canvas = img.Parent as Canvas;

            movingObject = null;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            try { foreach (Control child in canvas.Children)
                    if (top > Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child); }
            catch (Exception) { }
            Canvas.SetZIndex(img, top + 1);
         
            Mouse.Capture(null);
        }

  

        private void addAND(object sender, RoutedEventArgs e)
        {
            //canvas.Children.Clear();
            AND img = new AND(4);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
        }

        private void addOR(object sender, RoutedEventArgs e)
        {
            OR img = new OR(2);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
        }

        private void addXOR(object sender, RoutedEventArgs e)
        {
            XOR img = new XOR(2);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;

        }

        private void addNAND(object sender, RoutedEventArgs e)
        {
            NAND img = new NAND(4);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
        }

        private void addNOR(object sender, RoutedEventArgs e)
        {
            NOR img = new NOR(2);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;

        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                Control img = sender as Control;
                Canvas canvas = img.Parent as Canvas;

                double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                    newLeft = canvas.Margin.Left;
                img.SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                    newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                    newTop = canvas.Margin.Top;
                img.SetValue(Canvas.TopProperty, newTop);
                try
                {
                    StandardComponent component = sender as StandardComponent;
                    component.recalculer_pos();
                }
                catch { };


            }
        }


        private void addXNOR(object sender, RoutedEventArgs e)
        {
            XNOR img = new XNOR(2);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;

        }

        private void addInput(object sender, RoutedEventArgs e)
        {
            Input img = new Input();
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
            

        }
        private void addOutput(object sender, RoutedEventArgs e)
        {
            Output img = new Output();
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;


        }

        private void annuler(object sender, MouseButtonEventArgs e)
        {
            Wireclass.selected = false;
        }

      
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void closeWindow(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void RightDrawerHostOpen(object sender, RoutedEventArgs e)
        {
            drawerHost.IsRightDrawerOpen = true;
            drawerHost.IsLeftDrawerOpen = false;
        }

        private void BottomDrawerHostOpen(object sender, RoutedEventArgs e)
        {
            drawerHost.IsBottomDrawerOpen = true;
            MessageBox.Show("hi");
        }
    }


}


