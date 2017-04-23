﻿using Microsoft.Win32;
using System;
using System.Collections;
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
using WpfApplication9.SequentialComponent;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.Windows.Markup;
using System.Threading;
using System.ComponentModel;
using WpfApplication9.ComplexComponent;

namespace WpfApplication9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string APP_TITLE = "CircLab";
        public static List<StandardComponent> elementsSelected= new List<StandardComponent>();
        private string _filename = "";
        public static float Delay = 1;
        Stack<Canvas> undos = new Stack<Canvas>();
        Stack<Canvas> redos = new Stack<Canvas>();

        private bool _mute = false;
        private Brush oldBackground;
        public bool Mute
        {
            get {  return _mute; }
            set
            {
                if (value)
                {
                    oldBackground = canvas.Background;
                    canvas.Background = Brushes.White;


                }
                else
                {
                    canvas.Background = oldBackground;

                }
                _mute = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            gridActivatorCheckbox.DataContext = this;
            var ph = new PaletteHelper();
            ph.ReplacePrimaryColor("deeppurple");
            ph.ReplaceAccentColor("deeppurple");
            Wireclass.myCanvas = canvas;
            desactiveProp();
            StandardComponent.canvas = canvas;
            canvas.PreviewMouseMove += this.MouseMove2;
            canvas.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp2;
            this.Closing += new CancelEventHandler(WindowClosing);
            sourceEllipse = null;
        }

        private object movingObject;
        private double firstXPos, firstYPos;
     //   private Wireclass wire ;
        public static Ellipse sourceEllipse;
        public static Wireclass wire;
        public static bool SelectedTerminalIsSource;

   
        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            miseAJourPile();
            // In this event, we get the current mouse position on the control to use it in the MouseMove event.
            Control img = sender as Control;
            Canvas canvas = img.Parent as Canvas;

            firstXPos = e.GetPosition(canvas).X;
            firstYPos = e.GetPosition(canvas).Y;

            movingObject = sender;
           
            
            
            
            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            try
            {
                foreach (Control child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
            }
            catch (Exception) { }
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
            try
            {
                foreach (Control child in canvas.Children)
                    if (top > Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
            }
            catch (Exception) { }
            Canvas.SetZIndex(img, top + 1);

            Mouse.Capture(null);
        }


        public void modifieProperties()
        {
            if (elementsSelected!=null && elementsSelected.Count == 1)
            {       
                if (UserClass.IsInputChangeable(elementsSelected[0]))
                {
                    ClockFrequency.Visibility = Visibility.Collapsed;
                    Frequency.Visibility = Visibility.Collapsed;

                    if (elementsSelected[0].nbrInputs() != 8)
                        ComboBoxProperties.SelectedIndex = elementsSelected[0].nbrInputs() - 2;
                    else ComboBoxProperties.SelectedIndex = 3;

                    if(elementsSelected[0].nbrInputs()==2)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 2; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }

                    }
                    else if(elementsSelected[0].nbrInputs()==3)
                    {
                        
                        for(int i = 0;i< 3; i++){
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked= ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 3; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }
                       
                    }
                    else if(elementsSelected[0].nbrInputs()==4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 4; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }
                    }
                    else if (elementsSelected[0].nbrInputs() == 5)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 5; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }
                    }
                    else if (elementsSelected[0].nbrInputs() == 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 6; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }
                    }
                    else if (elementsSelected[0].nbrInputs() == 7)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                       
                        ((CheckBox)GridCheckBox.Children[7]).IsChecked = false;
                        ((CheckBox)GridCheckBox.Children[7]).Visibility = Visibility.Collapsed;
                        
                    }
                    else if (elementsSelected[0].nbrInputs() == 8)
                    {
                        for (int i = 0; i <8 ; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                    }
                    elementsSelected[0].recalculer_pos();
                }
                else if (elementsSelected[0] is SequentialComponent.Clock)
                {
                    ClockFrequency.Visibility = Visibility.Visible;
                    Frequency.Visibility = Visibility.Visible;
                    
                    
                }
            }
           
            
        }

        private void CreateNewGate(string name)
        {
            miseAJourPile();
            UIElement gate;
            switch (name)
            {
                case "AND":
                    gate = new AND(2); break;
                case "NAND":
                    gate = new NAND(2); break;
                case "NOR":
                    gate = new NOR(2); break;
                case "Not":
                    gate = new Not(); break;
                case "OR":
                    gate = new OR(2); break;
                case "XNOR":
                    gate = new XNOR(2); break;
                case "XOR":
                    gate = new XOR(2); break;
                case "Input":
                    gate = new Input(); break;
                case "Output":
                    gate = new Output(); break;
                case "Chronogram":
                    gate = new Chronogramme(2); break;
                case "Decoder":
                    gate = new Decodeur(2, 4); break;
                case "Encoder":
                    gate = new Encodeur(4, 2); break;
                case "FullAdder":
                    gate = new FullAdder(3, 2); break;
                case "HalfAdder":
                    gate = new HalfAdder(2, 2);  break;
                case "HalfSub":
                    gate = new HalfSub(2, 2); break;
                case "FullSub":
                    gate = new FullSub(3, 2); break;
                case "Comparator":
                    gate = new Comparateur(4, 3); break;
                case "Multiplexer":
                    gate = new Multiplexer(4, 1, 2); break;
                case "Demultiplexer":
                    gate = new Demultiplexer(1, 4, 2); break;
                case "Clock":
                    gate = new SequentialComponent.Clock(100, 100, MainWindow.Delay); break;
                case "Comment":
                    gate = new Comment("Label"); break;
                case "SToggle":
                    gate = new SynchToogle(); break;
                case "AToggle":
                    gate = new AsynchToogle(); break;
                case "RSLatche":
                    gate = new RSLatche(); break;
                case "RSTLatche":
                    gate = new RSHLatche(); break;
                case "JK":
                    gate = new JK(JK.TriggerType.RisingEdge); break;
                case "Register":
                    gate = new Registre(Registre.TriggerType.RisingEdge, 4); break;
                case "PRegister":
                    gate = new programmablRegister(programmablRegister.TriggerType.RisingEdge, 3); break;
                case "CRegister":
                    gate = new CirculerRegister(CirculerRegister.TriggerType.RisingEdge, 4, CirculerRegister.Type.Left); break;
                case "FDevider":
                    gate = new FrequencyDevider(); break;
                case "CounterN":
                    gate = new compteurN(6, 3); break;
                case "CounterModN":
                    gate = new CompteurModN(6, 3); break;
                case "CDownN":
                    gate = new DecompteurN(6, 3); break;
                case "CDownMN":
                    gate = new DecompteurModN(6, 3); break;
                default:
                    throw new ArgumentException("unknown gate");
            }
            canvas.Children.Add(gate);
            gate.AllowDrop = true;
            gate.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            gate.PreviewMouseMove += this.MouseMove;
            gate.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
        }
        private void add7segments(object sender, RoutedEventArgs e)
        {
            SeptSegmentsClass img = new SeptSegmentsClass();
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
        }

        private void addGate(object sender, RoutedEventArgs e)
        {
            CreateNewGate(((Button)sender).Content.ToString());
        }
        
        double differnceX;//Calculer la difference de deplacement des absices
        double differenceY;//meme chose :3

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                Control img = sender as Control;
                Canvas canvas = img.Parent as Canvas;
                differnceX = Mouse.GetPosition(canvas).X - firstXPos;
                differenceY = Mouse.GetPosition(canvas).Y - firstYPos;
                firstXPos = Mouse.GetPosition(canvas).X;
                firstYPos = Mouse.GetPosition(canvas).Y;






                foreach (StandardComponent component in elementsSelected)
                {



                    double newLeft = differnceX + ((StandardComponent)component).PosX - canvas.Margin.Left;
                    ((StandardComponent)component).PosX = differnceX + ((StandardComponent)component).PosX;
                    // newLeft inside canvas right-border?
                    if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                        newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                    // newLeft inside canvas left-border?
                    else if (newLeft < canvas.Margin.Left)
                        newLeft = canvas.Margin.Left;
                    component.SetValue(Canvas.LeftProperty, newLeft);

                    double newTop = differenceY + ((StandardComponent)component).PosY - canvas.Margin.Top;
                    ((StandardComponent)component).PosY = differenceY + ((StandardComponent)component).PosY;
                    // newTop inside canvas bottom-border?
                    if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                        newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                    // newTop inside canvas top-border?
                    else if (newTop < canvas.Margin.Top)
                        newTop = canvas.Margin.Top;
                    component.SetValue(Canvas.TopProperty, newTop);
                    component.recalculer_pos();
                    try
                    {
                        StandardComponent component1 = sender as StandardComponent;
                        component1.recalculer_pos();
                    }
                    catch { };
                }

            }
        }
        


      

        private void closeWindow(object sender, MouseButtonEventArgs e)
        {
            Close();

        }

        private async void WindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            SaveClose dlg = new SaveClose(CircuitName.Text);
            var result = await DialogHost.Show(dlg, "RootDialog");
            if (dlg.Selected == SaveClose.Result.DONT_SAVE)
            {
                this.Closing -= WindowClosing;
                Close();
            }
            if(dlg.Selected == SaveClose.Result.SAVE)
            {
                btnSave_Click(null, null);
                this.Closing -= WindowClosing;
                Close();
            }

        }

        private void RightDrawerHostOpen(object sender, RoutedEventArgs e)
        {
            drawerHost.IsRightDrawerOpen = true;
            drawerHost.IsLeftDrawerOpen = false;
        }
        private void HelpOpen(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Show();
        }

        private void ComboBoxProperties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(elementsSelected!=null)
            {
                if (elementsSelected.Count != 0)
                {
                    if (UserClass.IsInputChangeable(elementsSelected[0]))
                    {
                        int selecteVal = ComboBoxProperties.SelectedIndex + 2;
                        if (ComboBoxProperties.SelectedIndex == 3) selecteVal = 8;

                        if (selecteVal != elementsSelected[0].nbrInputs())
                        {
                            while (selecteVal > elementsSelected[0].nbrInputs())
                            {
                                elementsSelected[0].AddInputs();
                            }
                            while (selecteVal < elementsSelected[0].nbrInputs())
                            {
                                elementsSelected[0].RemoveInputs();
                            }

                            elementsSelected[0].redessiner(elementsSelected[0].path);
                            canvas.UpdateLayout();
                            elementsSelected[0].Run();
                        }
                    }
                    modifieProperties();

                }
            }
            
        }

        public void desactiveProp()
        {
            NbrEntreText.Visibility = Visibility.Collapsed;
            ComboBoxProperties.Visibility=Visibility.Collapsed;
            GridCheckBox.Visibility = Visibility.Collapsed;
        }

        public void activeProp()
        {
            NbrEntreText.Visibility = Visibility.Visible;
            ComboBoxProperties.Visibility = Visibility.Visible ;
            GridCheckBox.Visibility = Visibility.Visible;
        }


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Canvas && elementsSelected!=null)
            {
                foreach(StandardComponent elementSelected in elementsSelected)
                {
                    elementSelected.typeComponenet.Stroke = Brushes.RoyalBlue;
                }
                elementsSelected.Clear();
                
            }
            if (elementsSelected.Count() > 1 || elementsSelected.Count()==0) desactiveProp();
        }

               
         private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[1];
            terminal.IsInversed = checkBox2.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox3_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[2];
            terminal.IsInversed = checkBox3.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox4_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[3];
            terminal.IsInversed = checkBox4.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox5_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[4];
            terminal.IsInversed = checkBox5.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox6_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[5];
            terminal.IsInversed = checkBox6.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox7_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[6];
            terminal.IsInversed = checkBox7.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox8_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[7];
            terminal.IsInversed = checkBox8.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[0];
            terminal.IsInversed = checkBox1.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        

        private void BottomDrawerHostOpen(object sender, RoutedEventArgs e)
        {
            drawerHost.IsBottomDrawerOpen = true;
        }



        private  void PreviewMouseLeftButtonUp2(object sender, MouseButtonEventArgs e)
        {

            if (sourceEllipse != null)
            {

                Wireclass.selection1 = sourceEllipse;
                // (int)canvas.Children.Capacity;
                //MessageBox.Show(i.ToString);


                ArrayList arraylist = UserClass.FiltreSrandardComponent(canvas);

               

                foreach(StandardComponent standardcomponent in arraylist)
                {

                
                    foreach (Terminal terminal in standardcomponent.inputStack.Children)
                    { 
                      
                        
                        if (Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).X - e.GetPosition(canvas).X) < 20 && Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).Y - e.GetPosition(canvas).Y) < 20)
                        {
                            //MessageBox.Show("911");
                            Wireclass.selection2 = terminal.elSelector;
                        }
                    }
                    foreach (Terminal terminal in standardcomponent.OutputStack.Children)
                    {


                        if (Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).X - e.GetPosition(canvas).X) < 20 && Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).Y - e.GetPosition(canvas).Y) < 20)
                        {
                            //MessageBox.Show("911");
                            Wireclass.selection2 = terminal.elSelector;
                        }
                    }

                }
                
                 wire.Destroy();
                if (Wireclass.selection2!=null)
                {
                    wire.relier();
                }
                
            }

          
            Mouse.Capture(null);
            sourceEllipse = null;
        }

        public void refaire(object sender, RoutedEventArgs e)
        {
            //remplir undos
            undos.Push(instancier(canvas));
            retourBouton.IsEnabled = true;

            //Faire la modif
            Canvas nouveau = redos.Pop();
            if (redos.Count == 0)
            {
                refaireBouton.IsEnabled = false;
            }

            canvas.Children.RemoveRange(0, canvas.Children.Count);
            UIElement[] tableau = new UIElement[1000];
            int length = nouveau.Children.Count;
            nouveau.Children.CopyTo(tableau, 0);
            for (int i = 0; i < length; i++)
            {
                nouveau.Children.Remove(tableau[i]);
                canvas.Children.Add(tableau[i]);
                
            }
        }
        public void retour(object sender, RoutedEventArgs e)
        {
            //remplir redos
            redos.Push(instancier(canvas));
            refaireBouton.IsEnabled = true;

            //Faire la modif
            Canvas nouveau = undos.Pop();
            if (undos.Count == 0)
            {
                retourBouton.IsEnabled = false;
            }

            /*foreach(UIElement u in canvas.Children)
            {
                if (typeof(Ellipse) != u.GetType())
                {
                    canvas.Children.Remove(u);
                }
            }*/
            canvas.Children.RemoveRange(0, canvas.Children.Count);
            UIElement[] tableau = new UIElement[1000];
            int length = nouveau.Children.Count;
            nouveau.Children.CopyTo(tableau, 0);
            for (int i = 0; i < length; i++)
            {
                nouveau.Children.Remove(tableau[i]);
                canvas.Children.Add(tableau[i]);
                if (tableau[i] is Ellipse) MessageBox.Show("ok");
            }
            canvas.UpdateLayout();
            Wireclass.myCanvas = canvas;
        }

        public void miseAJourPile()
        {
            Canvas canvasPrecedent = instancier(canvas);
            undos.Push(canvasPrecedent);
            retourBouton.IsEnabled = true;
            //Console.Out.Write("*********"+undos.Count+"\n");
        }
        public Canvas instancier(Canvas canvas)
        {
            Canvas retour = new Canvas();
            UIElement[] tableau = new UIElement[1000];
            List<Wireclass> wireA = new List<Wireclass>();
            List<Wireclass> wireN = new List<Wireclass>();

            List<Wireclass> wires = new List<Wireclass>();

            int length = canvas.Children.Count;
            //Console.Out.WriteLine("**" + length + "**");
            canvas.Children.CopyTo(tableau, 0);


            for (int i = 0; i < length; i++)
            {
                UIElement newChild = null;

                Console.Out.Write("test" + tableau[i].GetType());

                if (typeof(AND) == tableau[i].GetType())
                {
                    // newChild = new AND((tableau[i] as AND).nbrinput);
                    newChild = new AND((tableau[i] as AND).nbrInputs());
                    for (int j = 0; j < (tableau[i] as AND).nbrInputs(); j++)
                    {

                        Terminal terminal_1 = (Terminal)((tableau[i] as AND).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as AND).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }

                }
                else if (typeof(OR) == tableau[i].GetType())
                {
                    newChild = new OR((tableau[i] as OR).nbrInputs());
                    for (int j = 0; j < (tableau[i] as OR).nbrInputs(); j++)
                    {

                        Terminal terminal_1 = (Terminal)((tableau[i] as OR).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as OR).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }
                }

                else if (typeof(NAND) == tableau[i].GetType())
                {
                    newChild = new NAND((tableau[i] as NAND).nbrInputs());
                    for (int j = 0; j < (tableau[i] as NAND).nbrInputs(); j++)
                    {

                        Terminal terminal_1 = (Terminal)((tableau[i] as NAND).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as NAND).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }
                }
                else if (typeof(NOR) == tableau[i].GetType())
                {
                    newChild = new NOR((tableau[i] as NOR).nbrInputs());
                    for (int j = 0; j < (tableau[i] as NOR).nbrInputs(); j++)
                    {
                        Terminal terminal_1 = (Terminal)((tableau[i] as NOR).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as NOR).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }
                }

                else if (typeof(Not) == tableau[i].GetType())
                {
                    newChild = new Not();

                    Terminal terminal_1 = (Terminal)((tableau[i] as Not).inputStack.Children[0]);
                    Terminal terminal_2 = (Terminal)((newChild as Not).inputStack.Children[0]);

                    if (terminal_1.IsInversed)
                    {
                        terminal_2.IsInversed = true;
                        terminal_2.input_inversed();
                    }
                }

                else if (typeof(Output) == tableau[i].GetType())
                {
                    newChild = new Output();
                }

                else if (typeof(Input) == tableau[i].GetType())
                {
                    newChild = new Input();
                    (newChild as Input).state = (tableau[i] as Input).state;

                }

                else if (typeof(XNOR) == tableau[i].GetType())
                {
                    newChild = new XNOR((tableau[i] as XNOR).nbrInputs());
                    for (int j = 0; j < (tableau[i] as XNOR).nbrInputs(); j++)
                    {

                        Terminal terminal_1 = (Terminal)((tableau[i] as XNOR).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as XNOR).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }
                }
                else if (typeof(XOR) == tableau[i].GetType())
                {
                    newChild = new XOR((tableau[i] as XOR).nbrInputs());
                    for (int j = 0; j < (tableau[i] as XOR).nbrInputs(); j++)
                    {

                        Terminal terminal_1 = (Terminal)((tableau[i] as XOR).inputStack.Children[j]);
                        Terminal terminal_2 = (Terminal)((newChild as XOR).inputStack.Children[j]);

                        if (terminal_1.IsInversed)
                        {
                            terminal_2.IsInversed = true;
                            terminal_2.input_inversed();
                        }

                    }
                }

                else if (typeof(Comparateur) == tableau[i].GetType())
                {
                    newChild = new Comparateur((tableau[i] as Comparateur).nbrInputs(), (tableau[i] as Comparateur).nbrOutputs());

                }

                else if (typeof(Decodeur) == tableau[i].GetType())
                {
                    newChild = new Decodeur((tableau[i] as Decodeur).nbrInputs(), (tableau[i] as Decodeur).nbrOutputs());
                }

                else if (typeof(Demultiplexer) == tableau[i].GetType())
                {
                    newChild = new Demultiplexer((tableau[i] as Demultiplexer).nbrInputs(), (tableau[i] as Demultiplexer).nbrOutputs(), (tableau[i] as Demultiplexer).nbrSelections());
                }

                else if (typeof(Encodeur) == tableau[i].GetType())
                {
                    newChild = new Encodeur((tableau[i] as Encodeur).nbrInputs(), (tableau[i] as Encodeur).nbrOutputs());
                }

                else if (typeof(FullAdder) == tableau[i].GetType())
                {
                    newChild = new FullAdder((tableau[i] as FullAdder).nbrInputs(), (tableau[i] as FullAdder).nbrOutputs());
                }

                else if (typeof(FullSub) == tableau[i].GetType())
                {
                    newChild = new FullSub((tableau[i] as FullSub).nbrInputs(), (tableau[i] as FullSub).nbrOutputs());
                }

                else if (typeof(HalfAdder) == tableau[i].GetType())
                {
                    newChild = new HalfAdder((tableau[i] as HalfAdder).nbrInputs(), (tableau[i] as HalfAdder).nbrOutputs());
                }

                else if (typeof(HalfSub) == tableau[i].GetType())
                {
                    newChild = new HalfSub((tableau[i] as HalfSub).nbrInputs(), (tableau[i] as HalfSub).nbrOutputs());
                }

                else if (typeof(Multiplexer) == tableau[i].GetType())
                {
                    newChild = new Multiplexer((tableau[i] as Multiplexer).nbrInputs(), (tableau[i] as Multiplexer).nbrOutputs(), (tableau[i] as Multiplexer).nbrSelections());
                }

                else if (typeof(SequentialComponent.Clock) == tableau[i].GetType())
                {
                    newChild = new SequentialComponent.Clock((tableau[i] as SequentialComponent.Clock).LowLevelms, (tableau[i] as SequentialComponent.Clock).HighLevelms, MainWindow.Delay);
                }
                
                else if (typeof(TextBlock) == tableau[i].GetType())
                {
                    newChild = new TextBlock();
                }

                if (newChild != null)
                {
                    Terminal[] tabTerminauxA = new Terminal[100];
                    Terminal[] tabTerminauxN = new Terminal[100];
                    (tableau[i] as StandardComponent).inputStack.Children.CopyTo(tabTerminauxA, 0);
                    (newChild as StandardComponent).inputStack.Children.CopyTo(tabTerminauxN, 0);
                    int len = (tableau[i] as StandardComponent).inputStack.Children.Count;

                    for (int jj = 0; jj < len; jj++)
                    {
                        if (tabTerminauxA[jj].wires.Count > 0)
                        {
                            if (!wireA.Contains((tabTerminauxA[jj].wires)[0] as Wireclass))
                            {
                                Wireclass wire = new Wireclass(retour);
                                tabTerminauxN[jj].wires.Add(wire);
                                wire.destination = tabTerminauxN[jj];

                                wire.btn111 = new Ellipse();
                                wire.btn222 = new Ellipse();
                                wire.state = ((tabTerminauxA[jj].wires)[0] as Wireclass).state;

                                if (wire.noued != null)
                                {
                                    wire.noued = new Ellipse();
                                    retour.Children.Add(wire.noued);
                                }
                                retour.Children.Add(wire.btn111);
                                retour.Children.Add(wire.btn222);

                                wireA.Add((tabTerminauxA[jj].wires)[0] as Wireclass);
                                wireN.Add(wire);
                            }
                            else
                            {
                                int index = wireA.IndexOf((tabTerminauxA[jj].wires)[0] as Wireclass);
                                tabTerminauxN[jj].wires.Add(wireN[index]);
                                wireN[index].destination = tabTerminauxN[jj];
                            }

                        }
                    }

                    (tableau[i] as StandardComponent).OutputStack.Children.CopyTo(tabTerminauxA, 0);
                    (newChild as StandardComponent).OutputStack.Children.CopyTo(tabTerminauxN, 0);
                    len = (tableau[i] as StandardComponent).OutputStack.Children.Count;

                    for (int jj = 0; jj < len; jj++)
                    {
                        foreach (Wireclass w in tabTerminauxA[jj].wires)
                        {
                            if (!wireA.Contains(w))
                            {
                                Wireclass wire = new Wireclass(retour);
                                tabTerminauxN[jj].wires.Add(wire);
                                wireA.Add(w);
                                wireN.Add(wire);
                                wire.source = tabTerminauxN[jj];
                                wire.btn111 = w.btn111;
                                wire.btn222 = w.btn222;
                                wire.state = w.state;
                                wire.noued = w.noued;

                                wire.btn111 = new Ellipse();
                                wire.btn222 = new Ellipse();
                                wire.state = w.state;

                                if (wire.noued != null)
                                {
                                    wire.noued = new Ellipse();
                                    retour.Children.Add(wire.noued);
                                }
                                retour.Children.Add(wire.btn111);
                                retour.Children.Add(wire.btn222);
                                //indiceLiaison++;
                            }
                            else
                            {
                                int index = wireA.IndexOf(w);
                                tabTerminauxN[jj].wires.Add(wireN[index]);
                                wireN[index].source = tabTerminauxN[jj];
                            }
                        }
                    }




                    retour.Children.Add(newChild);
                    newChild.AllowDrop = true;
                    newChild.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    newChild.PreviewMouseMove += this.MouseMove;
                    newChild.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;

                    newChild.SetValue(Canvas.LeftProperty, tableau[i].GetValue(Canvas.LeftProperty));
                    newChild.SetValue(Canvas.TopProperty, tableau[i].GetValue(Canvas.TopProperty));

                    try
                    {
                        StandardComponent component = newChild as StandardComponent;
                        component.recalculer_pos();
                    }
                    catch { };
                }

            }

            afficherLignes(retour, wireA, wireN);

            return retour;
        }

        public void afficherLignes(Canvas retour, List<Wireclass> wireA, List<Wireclass> wireN)
        {
            Line[] ligne = new Line[3];
            Line[] ligneN = new Line[3];
            for (int i = 0; i < wireA.Count; i++)
            {
                ligne[0] = wireA[i].l1;
                ligne[1] = wireA[i].l2;
                ligne[2] = wireA[i].l3;

                ligneN[0] = wireN[i].l1;
                ligneN[1] = wireN[i].l2;
                ligneN[2] = wireN[i].l3;

                for (int j = 0; j < 3; j++)
                {
                    Line l1 = ligneN[j];
                    Line line = ligne[j];

                    l1.ContextMenu = line.ContextMenu;

                    l1.Stroke = new SolidColorBrush(Colors.Black);
                    l1.StrokeThickness = 2.0;
                    l1.X1 = line.X1;
                    l1.X2 = line.X2;
                    l1.Y1 = line.Y1;
                    l1.Y2 = line.Y2;
                }
            }
        }

        private void SimulationStart_ButtonClick(object sender, RoutedEventArgs e)
        {
            if(simIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Play)
            {
                simIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                foreach (object component in canvas.Children)
                {
                    if (component is ISequential)
                    {
                        var tmp = component as ISequential;
                        tmp.Start();
                    }
                }
            }
            else
            {
                simIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                foreach (object component in canvas.Children)
                {
                    if (component is ISequential)
                    {
                        var tmp = component as ISequential;
                        tmp.Stop();
                    }
                }
            }
        }

        private void delay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (canvas != null) {
                foreach (object component in canvas.Children)
                {
                    if (component is ISequential)
                    {
                        var tmp = component as ISequential;
                        tmp.Delay = (float)delay.Value;
                    }
                }
                MainWindow.Delay = (float)delay.Value;
            }
            
        }

        private void ChangeTheme(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var ph = new PaletteHelper();
            switch (cb.SelectedIndex)
            {
                case 0:
                    {
                        ph.ReplacePrimaryColor("deeppurple");
                        ph.ReplaceAccentColor("deeppurple");
                    } break;
                case 1:
                    {
                        ph.ReplacePrimaryColor("indigo");
                        ph.ReplaceAccentColor("indigo");
                    }break;
                case 2:
                    {
                        ph.ReplacePrimaryColor("deeporange");
                        ph.ReplaceAccentColor("deeporange");
                    }break;
                case 3:
                    {
                        ph.ReplacePrimaryColor("red");
                        ph.ReplaceAccentColor("red");
                    }
                    break;
            }
        }
        
        private void MouseMove2(object sender, MouseEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed && sourceEllipse!=null)
            {
              //  wire = new Wireclass();
                Wireclass.btn2Point.X = e.GetPosition(canvas).X;
                Wireclass.btn2Point.Y = e.GetPosition(canvas).Y;
                Wireclass.selection1 = sourceEllipse;
                Wireclass.btn1Point = sourceEllipse.TransformToAncestor(canvas).Transform(new Point(0, 0));
                wire.Dessiner();
         

                //On dessine 
            }
        }

        private void btnOpen_click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".clc";
            dlg.Filter = "CircLab Circuit (.clc)|*.clc";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    CircuitXML.Load(dlg.FileName, ref canvas);
                    foreach (FrameworkElement gate in canvas.Children)
                    {
                        if (gate is StandardComponent)
                        {
                            gate.AllowDrop = true;
                            gate.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                            gate.PreviewMouseMove += this.MouseMove;
                            gate.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
                        }
                    }
                    btnSave.IsEnabled = true;
                    _filename = dlg.FileName;
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load circuit as requested: " + ex.ToString());
                }

            }
        }

        private void btnSaveAs_click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".clc";
            dlg.Filter = "CircLab Circuit (.clc)|*.clc";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    CircuitXML.Save(dlg.FileName, canvas);
                    _filename = dlg.FileName;
                    btnSave.IsEnabled = true;
                    UpdateTitle();
                    MainSnackbar.MessageQueue.Enqueue("Saved Circuit Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save circuit as requested: " + ex.ToString());
                }
            }
        }
        
        private async void btnNew_click(object sender, MouseButtonEventArgs e)
        {
            NewFileDialog dlg = new NewFileDialog();
            var result = await DialogHost.Show(dlg, "RootDialog");
            if(dlg.Selected == NewFileDialog.Result.CREATE)
            {
                _filename = "";
                canvas.Children.Clear();
                int temp;
                if (!int.TryParse(dlg.CircuitWidth.Text, out temp)) temp = 2000;
                canvas.Width = Math.Min(10000,Math.Max(100, temp));
                if (!int.TryParse(dlg.CircuitHeight.Text, out temp)) temp = 2000;
                canvas.Height = Math.Min(10000, Math.Max(100, temp));
                UpdateTitle();
            }

        }

        private void About_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            aboutBtn.Command.Execute(aboutBtn.CommandParameter);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(_filename == "")
            {
                btnSaveAs_click(null, null);
            }
            else
            {
                try
                {
                    CircuitXML.Save(_filename, canvas);
                    btnSave.IsEnabled = true;
                    UpdateTitle();
                    MainSnackbar.MessageQueue.Enqueue("Saved Circuit Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save circuit as requested: " + ex.ToString());
                }
            }
        }

        private void mwindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                if(e.Key == Key.S)
                {
                    btnSave_Click(null, null);
                }
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Image (.png)|*.png|JPEG Image (.jpg)|*.jpg|Bitmap Image (.bmp)|*.bmp";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                BitmapEncoder be = null;
                switch (dlg.FilterIndex)
                {
                    case 0:
                        be = new PngBitmapEncoder();
                        break;
                    case 1:
                        be = new JpegBitmapEncoder();
                        break;
                    case 2:
                        be = new BmpBitmapEncoder();
                        break;
                }
                Mute = true;
                canvas.UpdateLayout();
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)(canvas.ActualWidth), (int)(canvas.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                rtb.Render(canvas);
                Mute = false;
                be.Frames.Add(BitmapFrame.Create(rtb));
                try
                {
                    System.IO.FileStream fs = System.IO.File.Create(dlg.FileName);
                    be.Save(fs);
                    fs.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save image as requested: " + ex.ToString());
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new System.Windows.Controls.PrintDialog();

            if (printDlg.ShowDialog() == true)
            {
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                
                Mute = true;
                canvas.UpdateLayout();
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)(canvas.ActualWidth), (int)(canvas.ActualHeight), 96, 96, PixelFormats.Pbgra32);
                rtb.Render(canvas);
                BitmapSource bs = rtb;
                Image i = new Image();
                i.Source = bs;
                i.Stretch = Stretch.Uniform;
                i.Width = capabilities.PageImageableArea.ExtentWidth;
                i.Height = capabilities.PageImageableArea.ExtentHeight;

                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.

                i.Measure(sz);

                i.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));


                //now print the visual to printer to fit on the one page.

                printDlg.PrintVisual(i, "Circuit");

                Mute = false;

            }
        }
        private void addChrono(object sender, RoutedEventArgs e)
        {
            Chronogramme img = new Chronogramme(2);
            canvas.Children.Add(img);
            img.AllowDrop = true;
            img.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            img.PreviewMouseMove += this.MouseMove;
            img.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;


        }
        private void UpdateTitle()
        {
            StringBuilder ttl = new StringBuilder();

            if (String.IsNullOrEmpty(_filename))
            {
                ttl.Append("[Untitled]");
            }
            else
            {
                ttl.Append(_filename.Substring(_filename.LastIndexOf(@"\") + 1, _filename.LastIndexOf(".") - _filename.LastIndexOf(@"\") - 1));
            }

            ttl.Append(" - ");

            ttl.Append(APP_TITLE);

            Title = ttl.ToString();
            CircuitName.Text = ttl.ToString();
        }
    }


}


