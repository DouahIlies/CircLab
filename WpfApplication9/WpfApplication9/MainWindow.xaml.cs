using Microsoft.Win32;
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
using CircLab.Component;
using CircLab.LogicGate;
using CircLab.SequentialComponent;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.Windows.Markup;
using System.Threading;
using System.ComponentModel;
using CircLab.ComplexComponent;
using WpfApplication9.LogicGate;
using System.Xml.Linq;

namespace CircLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string APP_TITLE = "CircLab";
        public Canvas canvastest;
        public static List<StandardComponent> elementsSelected= new List<StandardComponent>();
        private string _filename = "";
        public static float Delay = 1;
        Stack<XElement> undos = new Stack<XElement>();
        Stack<XElement> redos = new Stack<XElement>();
        List<StandardComponent> liste_copier = new List<StandardComponent>();

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
            StandardComponent.liste_copier = liste_copier;
            gridActivatorCheckbox.DataContext = this;
            var ph = new PaletteHelper();
            ph.ReplacePrimaryColor("deeppurple");
            ph.ReplaceAccentColor("deeppurple");
            Wireclass.myCanvas = canvas;
            desactiveProp();
            StandardComponent.canvas = canvas;
            StandardComponent.fenetre = this;
            canvas.PreviewMouseMove += this.MouseMove2;
            canvas.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp2;
            this.PreviewKeyDown += new KeyEventHandler(Window1_EditFull_KeyDown);
            this.Closing += new CancelEventHandler(WindowClosing);
            sourceEllipse = null;
           canvastest = this.canvas;
        }

        private object movingObject;
        private double firstXPos, firstYPos;
     //   private Wireclass wire ;
        public static Ellipse sourceEllipse;
        public static Wireclass wire;
        public static bool SelectedTerminalIsSource;

   
        public new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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


        public new void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                    Type.Visibility = Visibility.Collapsed;
                    Compteur.Visibility = Visibility.Collapsed;
                    NbrEnreComparateur.Visibility = Visibility.Collapsed;
                    TypeDec.Visibility = Visibility.Collapsed;
                    TypeEnc.Visibility = Visibility.Collapsed;
                    TypeReg.Visibility = Visibility.Collapsed;

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

                    if (UserClass.IsFrontChangeable(elementsSelected[0]) && UserClass.IsNiveauChangeable(elementsSelected[0]))
                    {
                        NiveauBas.Visibility = Visibility.Visible;
                        NiveauHaut.Visibility = Visibility.Visible;
                        Type.Visibility = Visibility.Visible;

                       
                        if ((elementsSelected[0] is SequentialComponent.Registre))
                        {
                            comboBoxtype(((SequentialComponent.Registre)elementsSelected[0]).Trigger.ToString());
                        }
                        else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                        {
                            comboBoxtype(((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger.ToString());
                            TypeReg.Visibility = Visibility.Visible;
                            if (((CirculerRegister)elementsSelected[0]).typeDec == CirculerRegister.Type.Right)
                            {
                                ComboBoxPropertiesReg.SelectedIndex = 0;
                            }
                            else
                            {
                                ComboBoxPropertiesReg.SelectedIndex = 1;
                            }
                        }
                        else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        comboBoxtype(((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger.ToString());
                    }
                        Type.Visibility = Visibility.Visible;
                    }
                    elementsSelected[0].recalculer_pos();
                }
                else if ((elementsSelected[0] is SequentialComponent.Clock) ||(elementsSelected[0] is SequentialComponent.Chronogramme))
                {
                    Type.Visibility = Visibility.Collapsed;
                    if((elementsSelected[0] is SequentialComponent.Clock))
                    {
                        Frequency.Text = ((SequentialComponent.Clock)elementsSelected[0]).Delay.ToString();
                    }
                    else
                    {
                        Frequency.Text = ((SequentialComponent.Chronogramme)elementsSelected[0]).Delay.ToString();
                    }
                       
                    ClockFrequency.Visibility = Visibility.Visible;
                    Frequency.Visibility = Visibility.Visible;
                               
                }
                else if(UserClass.IsFrontChangeable(elementsSelected[0]) && UserClass.IsNiveauChangeable(elementsSelected[0]))
                {
                    NiveauBas.Visibility = Visibility.Visible;
                    NiveauHaut.Visibility = Visibility.Visible;
                    Type.Visibility = Visibility.Visible;

                    if (elementsSelected[0] is SequentialComponent.FlipFlop)
                    {
                        comboBoxtype(((SequentialComponent.FlipFlop)elementsSelected[0]).Trigger.ToString());
                    }
                    else if((elementsSelected[0] is SequentialComponent.Registre))
                    {
                        comboBoxtype(((SequentialComponent.Registre)elementsSelected[0]).Trigger.ToString());
                    }
                    else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                    {
                        comboBoxtype(((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger.ToString());
                        TypeReg.Visibility = Visibility.Visible;
                        if (((CirculerRegister)elementsSelected[0]).typeDec == CirculerRegister.Type.Right)
                        {
                            ComboBoxPropertiesReg.SelectedIndex = 0;
                        }
                        else
                        {
                            ComboBoxPropertiesReg.SelectedIndex = 1;
                        }
                    }
                    else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        comboBoxtype(((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger.ToString());
                    }
                    Type.Visibility = Visibility.Visible;
                }
                else if (UserClass.IsFrontChangeable(elementsSelected[0]))
                {
                    Type.Visibility = Visibility.Visible;
                    Compteur.Visibility = Visibility.Collapsed;
                    comboBoxtype(((SequentialComponent.JK)elementsSelected[0]).Trigger.ToString());
                    NiveauBas.Visibility = Visibility.Collapsed;
                    NiveauHaut.Visibility = Visibility.Collapsed;
                }
                else if (UserClass.IsCompteur(elementsSelected[0]))
                {
                    Compteur.Visibility = Visibility.Visible;
                    if(elementsSelected[0] is SequentialComponent.compteurN)
                    {
                        ComboBoxCompteur.SelectedIndex = ((SequentialComponent.compteurN)elementsSelected[0]).Val - 2;
                        TextCompteur.Text = "Le nombre max";
                    }
                    else if (elementsSelected[0] is SequentialComponent.CompteurModN)
                    {
                        ComboBoxCompteur.SelectedIndex = ((SequentialComponent.CompteurModN)elementsSelected[0]).Val - 2;
                       
                        TextCompteur.Text = "Modulo";
                    }
                    else if (elementsSelected[0] is SequentialComponent.DecompteurN)
                    {
                        ComboBoxCompteur.SelectedIndex = ((SequentialComponent.DecompteurN)elementsSelected[0]).Val - 2;
                        TextCompteur.Text = "Le nombre max";
                    }
                    else if (elementsSelected[0] is SequentialComponent.DecompteurModN)
                    {
                        ComboBoxCompteur.SelectedIndex = ((SequentialComponent.DecompteurModN)elementsSelected[0]).Val - 2;
                        TextCompteur.Text = "Modulo";
                    }
                }
                else if(elementsSelected[0] is Decodeur)
                {
                    TypeDec.Visibility = Visibility.Visible;
                    GridCheckBox.Visibility = Visibility.Visible;
                   
                    if (elementsSelected[0].nbrInputs() == 2)
                        ComboBoxPropertiesDec.SelectedIndex = 0;
                    else ComboBoxPropertiesDec.SelectedIndex = 1;

                    if (elementsSelected[0].nbrInputs() == 2)
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
                    else if (elementsSelected[0].nbrInputs() == 3)
                    {

                        for (int i = 0; i < 3; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        for (int i = 3; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = false;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Collapsed;
                        }

                    }

                }
                else if (elementsSelected[0] is Encodeur)
                {
                    TypeEnc.Visibility = Visibility.Visible;
                    GridCheckBox.Visibility = Visibility.Visible;
                    if (elementsSelected[0].nbrInputs() == 4)
                        ComboBoxPropertiesEnc.SelectedIndex = 0;
                    else ComboBoxPropertiesEnc.SelectedIndex = 1;

                    if (elementsSelected[0].nbrInputs() == 4)
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
                    else if (elementsSelected[0].nbrInputs() == 8)
                    {

                        for (int i = 0; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                        

                    }

                }else if(elementsSelected[0] is Multiplexer)
                {
                    TypeDec.Visibility = Visibility.Visible;
                    GridCheckBox.Visibility = Visibility.Visible;

                    if (elementsSelected[0].nbrInputs() == 4)
                        ComboBoxPropertiesDec.SelectedIndex = 0;
                    else ComboBoxPropertiesDec.SelectedIndex = 1;

                    if (elementsSelected[0].nbrInputs() == 4)
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
                    else if (elementsSelected[0].nbrInputs() == 8)
                    {

                        for (int i = 0; i < 8; i++)
                        {
                            ((CheckBox)GridCheckBox.Children[i]).IsChecked = ((Terminal)elementsSelected[0].inputStack.Children[i]).IsInversed;
                            ((CheckBox)GridCheckBox.Children[i]).Visibility = Visibility.Visible;
                        }
                    }
                }
                else if(elementsSelected[0] is Demultiplexer)
                {

                }
                else if(elementsSelected[0] is Comparateur)
                {
                    NbrEnreComparateur.Visibility = Visibility.Visible;
                    ComparatuerText.Text = (elementsSelected[0].nbrInputs()/2).ToString();

                }
             
            }
          
            
        }
        //RisingEdge, FallingEdge, HighLevel, LowLevel
        private void comboBoxtype(string type)
        {
            if (type == "RisingEdge") ComboBoxFrontNiveau.SelectedIndex = 0;
            else if(type== "FallingEdge") ComboBoxFrontNiveau.SelectedIndex = 1;
            else if (type == "HighLevel") ComboBoxFrontNiveau.SelectedIndex = 2;
            else if (type == "LowLevel") ComboBoxFrontNiveau.SelectedIndex = 3;
        }
        private void CreateNewGate(string name)
        {
            //miseAJourPile();
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
                    gate = new Chronogramme(2,MainWindow.Delay); break;
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
                    gate = new programmablRegister(programmablRegister.TriggerType.RisingEdge, 2); break;
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
                case "Hexa Segments":
                    gate = new Hexadicimal(); break;
                case "Sept Segments":
                    gate = new SeptSegmentsClass(); break;
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
        private void addHexa(object sender, RoutedEventArgs e)
        {
            Hexadicimal img = new Hexadicimal();
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

        public new void MouseMove(object sender, MouseEventArgs e)
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
                foreach (Window w in Application.Current.Windows)
                {
                    if (w != this)
                        w.Close();
                }
                Close();
            }
            if(dlg.Selected == SaveClose.Result.SAVE)
            {
                btnSave_Click(null, null);
                this.Closing -= WindowClosing;
                foreach (Window w in Application.Current.Windows)
                {
                    if (w != this)
                        w.Close();
                }
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
            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\help\\index.html");
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
                       // if(elementsSelected[0] is programmablRegister) selecteVal= ComboBoxProperties.SelectedIndex + 4;
                        if (elementsSelected[0] is Chronogramme)
                        {
                            ((Chronogramme)elementsSelected[0]).nbrEntrée=selecteVal;
                            MessageBox.Show(selecteVal.ToString());
                        }
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
            ClockFrequency.Visibility = Visibility.Collapsed;
            Frequency.Visibility = Visibility.Collapsed;
            Type.Visibility = Visibility.Collapsed;
            Compteur.Visibility = Visibility.Collapsed;
            TypeDec.Visibility = Visibility.Collapsed;
            TypeEnc.Visibility = Visibility.Collapsed;
            NbrEnreComparateur.Visibility = Visibility.Collapsed;
            TypeReg.Visibility = Visibility.Collapsed;
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
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[1];
            terminal.IsInversed = checkBox2.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox3_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[2];
            terminal.IsInversed = checkBox3.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox4_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[3];
            terminal.IsInversed = checkBox4.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox5_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[4];
            terminal.IsInversed = checkBox5.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox6_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[5];
            terminal.IsInversed = checkBox6.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox7_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[6];
            terminal.IsInversed = checkBox7.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox8_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
            Terminal terminal = (Terminal)elementsSelected[0].inputStack.Children[7];
            terminal.IsInversed = checkBox8.IsChecked.Value;
            terminal.input_inversed();
            elementsSelected[0].Run();
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            //miseAJourPile();
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
           // MessageBox.Show("krahna");
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
                      
                        
                        if (Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).X - e.GetPosition(canvas).X) < 15 && Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).Y - e.GetPosition(canvas).Y) < 15)
                        {
                            
                            Wireclass.selection2 = terminal.elSelector;
                        }
                    }
                    foreach (Terminal terminal in standardcomponent.OutputStack.Children)
                    {


                        if (Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).X - e.GetPosition(canvas).X) < 15 && Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).Y - e.GetPosition(canvas).Y) < 15)
                        {
                            
                            Wireclass.selection2 = terminal.elSelector;
                        }
                    }
                    foreach (Terminal terminal in standardcomponent.selectionStack.Children)
                    {


                        if (Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).X - e.GetPosition(canvas).X) < 15 && Math.Abs(terminal.elSelector.TransformToAncestor(canvas).Transform(new Point(0, 0)).Y - e.GetPosition(canvas).Y) < 10)
                        {
                          
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
            XElement circuit = redos.Pop();
            if (redos.Count == 0)
            {
                refaireBouton.IsEnabled = false;
            }

            canvas.Children.Clear();
            canvas.Width = int.Parse(circuit.Attribute("Width").Value);
            canvas.Height = int.Parse(circuit.Attribute("Height").Value);
            var gid = new Dictionary<int, StandardComponent>();
            foreach (XElement gate in circuit.Element("Gates").Elements())
            {
                StandardComponent shape = CreateGate(gate);
                shape.SetValue(Canvas.LeftProperty, double.Parse(gate.Attribute("X").Value));
                shape.SetValue(Canvas.TopProperty, double.Parse(gate.Attribute("Y").Value));
                shape.RotateComponent(int.Parse(gate.Attribute("Rotation").Value));
                shape.PosX = (double)shape.GetValue(Canvas.LeftProperty);
                shape.PosY = (double)shape.GetValue(Canvas.TopProperty);

                gid[int.Parse(gate.Attribute("ID").Value)] = shape;
                canvas.Children.Add(shape);

            }
            foreach (XElement wire in circuit.Element("Wires").Elements())
            {
                canvas.UpdateLayout();
                MainWindow.wire = new Wireclass();
                int temp;
                if (!int.TryParse(wire.Element("From").Attribute("ID").Value, out temp)) continue;
                StandardComponent gateSrc = gid[temp];
                if (!int.TryParse(wire.Element("To").Attribute("ID").Value, out temp)) continue;
                StandardComponent gateDest = gid[temp];
                if (!int.TryParse(wire.Element("From").Attribute("Port").Value, out temp)) continue;
                int portSrc = temp;
                if (!int.TryParse(wire.Element("To").Attribute("Port").Value, out temp)) continue;
                int portDest = temp;
                Wireclass.selection1 = ((Terminal)gateSrc.OutputStack.Children[portSrc]).elSelector;
                Wireclass.selection2 = ((Terminal)gateDest.inputStack.Children[portDest]).elSelector;
                MainWindow.wire.relier();
                canvas.UpdateLayout();
                MainWindow.wire.btn111 = Wireclass.selection1;
                MainWindow.wire.btn222 = Wireclass.selection2;
                Wireclass.myCanvas = canvas;

                canvas.UpdateLayout();
            }

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

        }
        public void retour(object sender, RoutedEventArgs e)
        {
            //remplir redos
            redos.Push(instancier(canvas));
            refaireBouton.IsEnabled = true;

            //Faire la modif
            XElement circuit = undos.Pop();
            if (undos.Count == 0)
            {
                retourBouton.IsEnabled = false;
            }

            canvas.Children.Clear();
            canvas.Width = int.Parse(circuit.Attribute("Width").Value);
            canvas.Height = int.Parse(circuit.Attribute("Height").Value);
            var gid = new Dictionary<int, StandardComponent>();
            foreach (XElement gate in circuit.Element("Gates").Elements())
            {
                StandardComponent shape = CreateGate(gate);
                shape.SetValue(Canvas.LeftProperty, double.Parse(gate.Attribute("X").Value));
                shape.SetValue(Canvas.TopProperty, double.Parse(gate.Attribute("Y").Value));
                shape.RotateComponent(int.Parse(gate.Attribute("Rotation").Value));
                shape.PosX = (double)shape.GetValue(Canvas.LeftProperty);
                shape.PosY = (double)shape.GetValue(Canvas.TopProperty);

                gid[int.Parse(gate.Attribute("ID").Value)] = shape;
                canvas.Children.Add(shape);

            }
            foreach (XElement wire in circuit.Element("Wires").Elements())
            {
                canvas.UpdateLayout();
                MainWindow.wire = new Wireclass();
                int temp;
                if (!int.TryParse(wire.Element("From").Attribute("ID").Value, out temp)) continue;
                StandardComponent gateSrc = gid[temp];
                if (!int.TryParse(wire.Element("To").Attribute("ID").Value, out temp)) continue;
                StandardComponent gateDest = gid[temp];
                if (!int.TryParse(wire.Element("From").Attribute("Port").Value, out temp)) continue;
                int portSrc = temp;
                if (!int.TryParse(wire.Element("To").Attribute("Port").Value, out temp)) continue;
                int portDest = temp;
                Wireclass.selection1 = ((Terminal)gateSrc.OutputStack.Children[portSrc]).elSelector;
                Wireclass.selection2 = ((Terminal)gateDest.inputStack.Children[portDest]).elSelector;
                MainWindow.wire.relier();
                canvas.UpdateLayout();
                MainWindow.wire.btn111 = Wireclass.selection1;
                MainWindow.wire.btn222 = Wireclass.selection2;
                Wireclass.myCanvas = canvas;

                canvas.UpdateLayout();
            }

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

            foreach (StandardComponent elementSelected in elementsSelected)
            {
                elementSelected.typeComponenet.Stroke = Brushes.RoyalBlue;
            }
            elementsSelected.Clear();

        }

        public void miseAJourPile()
        {
            XElement canvasPrecedent = instancier(canvas);
            undos.Push(canvasPrecedent);
            retourBouton.IsEnabled = true;
        }
        public XElement instancier(Canvas canvas)
        {
            XElement circuit = new XElement("Circuit");
            circuit.SetAttributeValue("Width", canvas.Width);
            circuit.SetAttributeValue("Height", canvas.Height);

            XElement gates = new XElement("Gates");
            var gid = new Dictionary<StandardComponent, int>();
            int id = 1;
            foreach (object shape in canvas.Children)
            {
                if (shape is StandardComponent)
                {
                    var g = shape as StandardComponent;
                    XElement gt = new XElement("Gate");
                    gt.SetAttributeValue("Type", g.GetType().Name);
                    gt.SetAttributeValue("ID", id);
                    gt.SetAttributeValue("X", g.PosX);
                    gt.SetAttributeValue("Y", g.PosY);
                    gt.SetAttributeValue("NumInputs", g.nbrInputs());
                    gt.SetAttributeValue("Rotation", g.rotation);

                    if (shape is SequentialComponent.Clock)
                    {
                        gt.SetAttributeValue("HighLevelms", ((SequentialComponent.Clock)g).HighLevelms);
                        gt.SetAttributeValue("LowLevelms", ((SequentialComponent.Clock)g).LowLevelms);
                    }
                    if (shape is JK)
                    {
                        if (((JK)shape).Trigger == JK.TriggerType.FallingEdge)
                            gt.SetAttributeValue("TriggerType", 0);
                        else gt.SetAttributeValue("TriggerType", 1);
                    }
                    if (shape is Registre)
                    {
                        if (((Registre)shape).Trigger == Registre.TriggerType.FallingEdge)
                            gt.SetAttributeValue("TriggerType", 0);
                        else if (((Registre)shape).Trigger == Registre.TriggerType.RisingEdge)
                            gt.SetAttributeValue("TriggerType", 1);
                        else if (((Registre)shape).Trigger == Registre.TriggerType.LowLevel)
                            gt.SetAttributeValue("TriggerType", 2);
                        else if (((Registre)shape).Trigger == Registre.TriggerType.HighLevel)
                            gt.SetAttributeValue("TriggerType", 3);
                    }
                    if (shape is programmablRegister)
                    {
                        if (((programmablRegister)shape).Trigger == programmablRegister.TriggerType.FallingEdge)
                            gt.SetAttributeValue("TriggerType", 0);
                        else if (((programmablRegister)shape).Trigger == programmablRegister.TriggerType.RisingEdge)
                            gt.SetAttributeValue("TriggerType", 1);
                        else if (((programmablRegister)shape).Trigger == programmablRegister.TriggerType.LowLevel)
                            gt.SetAttributeValue("TriggerType", 2);
                        else if (((programmablRegister)shape).Trigger == programmablRegister.TriggerType.HighLevel)
                            gt.SetAttributeValue("TriggerType", 3);
                    }
                    if (shape is CirculerRegister)
                    {
                        if (((CirculerRegister)shape).Trigger == CirculerRegister.TriggerType.FallingEdge)
                            gt.SetAttributeValue("TriggerType", 0);
                        else if (((CirculerRegister)shape).Trigger == CirculerRegister.TriggerType.RisingEdge)
                            gt.SetAttributeValue("TriggerType", 1);
                        else if (((CirculerRegister)shape).Trigger == CirculerRegister.TriggerType.LowLevel)
                            gt.SetAttributeValue("TriggerType", 2);
                        else if (((CirculerRegister)shape).Trigger == CirculerRegister.TriggerType.HighLevel)
                            gt.SetAttributeValue("TriggerType", 3);
                        if (((CirculerRegister)shape).typeDec == CirculerRegister.Type.Left)
                            gt.SetAttributeValue("CircularType", 0);
                        else if (((CirculerRegister)shape).typeDec == CirculerRegister.Type.Right)
                            gt.SetAttributeValue("CircularType", 1);
                    }
                    if (shape is compteurN)
                    {
                        gt.SetAttributeValue("NumOutputs", ((compteurN)g).Nbroutputs);
                        gt.SetAttributeValue("N", ((compteurN)g).Val);
                    }
                    if (shape is CompteurModN)
                    {
                        gt.SetAttributeValue("NumOutputs", ((CompteurModN)g).Nbroutputs);
                        gt.SetAttributeValue("N", ((CompteurModN)g).Val);
                    }
                    if (shape is DecompteurN)
                    {
                        gt.SetAttributeValue("NumOutputs", ((DecompteurN)g).Nbroutputs);
                        gt.SetAttributeValue("N", ((DecompteurN)g).Val);
                    }
                    if (shape is DecompteurModN)
                    {
                        gt.SetAttributeValue("NumOutputs", ((DecompteurModN)g).Nbroutputs);
                        gt.SetAttributeValue("N", ((DecompteurModN)g).Val);
                    }
                    if (shape is Demultiplexer)
                    {
                        gt.SetAttributeValue("NumOutputs", ((Demultiplexer)g).nbrOutputs());
                    }
                    gates.Add(gt);
                    gid.Add(g, id);
                    id++;
                }
            }

            XElement wires = new XElement("Wires");
            foreach (object shape in canvas.Children)
            {
                if (shape is StandardComponent)
                {
                    var g = shape as StandardComponent;
                    for (int i = 0; i < g.inputStack.Children.Count; i++)
                    {
                        var input = g.inputStack.Children[i] as Terminal;
                        if (input.wires.Count > 0)
                        {
                            XElement wire = new XElement("Wire",
                                    new XElement("From"), new XElement("To"));
                            Terminal sourceTerminal = ((Wireclass)input.wires[0]).source;
                            StandardComponent sourceComponent = UserClass.TryFindParent<StandardComponent>(sourceTerminal);
                            wire.Element("From").SetAttributeValue("ID", gid[sourceComponent]);
                            wire.Element("From").SetAttributeValue("Port", sourceComponent.OutputStack.Children.IndexOf(sourceTerminal));
                            wire.Element("To").SetAttributeValue("ID", gid[g]);
                            wire.Element("To").SetAttributeValue("Port", i);
                            wires.Add(wire);
                        }
                    }
                }
            }

            circuit.Add(gates);
            circuit.Add(wires);
            return circuit;
        }

        public StandardComponent CreateGate(XElement gate)
        {
            int temp, temp2;
            int numInputs = int.Parse(gate.Attribute("NumInputs").Value);
            switch (gate.Attribute("Type").Value)
            {

                case "AND":
                    return new AND(numInputs);
                case "NAND":
                    return new NAND(numInputs);
                case "NOR":
                    return new NOR(numInputs);
                case "Not":
                    return new Not();
                case "OR":
                    return new OR(numInputs);
                case "XNOR":
                    return new XNOR(numInputs);
                case "XOR":
                    return new XOR(numInputs);
                case "Input":
                    return new Input();
                case "Output":
                    return new Output();
                case "Clock":
                    temp = int.Parse(gate.Attribute("HighLevelms").Value);
                    temp2 = int.Parse(gate.Attribute("LowLevelms").Value);
                    return new SequentialComponent.Clock(temp, temp2, MainWindow.Delay);
                case "JK":
                    temp = int.Parse(gate.Attribute("TriggerType").Value);
                    return new JK((temp == 0) ? JK.TriggerType.FallingEdge : JK.TriggerType.RisingEdge);
                case "SynchToogle":
                    return new SynchToogle();
                case "AsynchToogle":
                    return new AsynchToogle();
                case "RSLatche":
                    return new RSLatche();
                case "RSHLatche":
                    return new RSHLatche();
                case "Chronogramme":
                    return new Chronogramme(2, MainWindow.Delay);
                case "SeptSegmentsClass":
                    return new SeptSegmentsClass();
                case "Hexadicimal":
                    return new WpfApplication9.LogicGate.Hexadicimal();
                case "Registre":
                    temp = int.Parse(gate.Attribute("TriggerType").Value);
                    switch (temp)
                    {
                        case 0: return new Registre(Registre.TriggerType.FallingEdge, numInputs);
                        case 1: return new Registre(Registre.TriggerType.RisingEdge, numInputs);
                        case 2: return new Registre(Registre.TriggerType.LowLevel, numInputs);
                        default: return new Registre(Registre.TriggerType.HighLevel, numInputs);
                    }
                case "programmablRegister":
                    temp = int.Parse(gate.Attribute("TriggerType").Value);
                    switch (temp)
                    {
                        case 0: return new programmablRegister(programmablRegister.TriggerType.FallingEdge, numInputs);
                        case 1: return new programmablRegister(programmablRegister.TriggerType.RisingEdge, numInputs);
                        case 2: return new programmablRegister(programmablRegister.TriggerType.LowLevel, numInputs);
                        default: return new programmablRegister(programmablRegister.TriggerType.HighLevel, numInputs);
                    }
                case "CirculerRegister":
                    temp = int.Parse(gate.Attribute("TriggerType").Value);
                    switch (temp)
                    {
                        case 0: return new CirculerRegister(CirculerRegister.TriggerType.FallingEdge, numInputs, CirculerRegister.Type.Left);
                        case 1: return new CirculerRegister(CirculerRegister.TriggerType.RisingEdge, numInputs, CirculerRegister.Type.Left);
                        case 2: return new CirculerRegister(CirculerRegister.TriggerType.LowLevel, numInputs, CirculerRegister.Type.Left);
                        default: return new CirculerRegister(CirculerRegister.TriggerType.HighLevel, numInputs, CirculerRegister.Type.Left);
                    }
                case "FrequencyDevider":
                    return new FrequencyDevider();
                case "compteurN":
                    temp = int.Parse(gate.Attribute("NumOutputs").Value);
                    temp2 = int.Parse(gate.Attribute("N").Value);
                    return new compteurN(temp2, temp);
                case "CompteurModN":
                    temp = int.Parse(gate.Attribute("NumOutputs").Value);
                    temp2 = int.Parse(gate.Attribute("N").Value);
                    return new CompteurModN(temp2, temp);
                case "DecompteurN":
                    temp = int.Parse(gate.Attribute("NumOutputs").Value);
                    temp2 = int.Parse(gate.Attribute("N").Value);
                    return new DecompteurN(temp2, temp);
                case "DecompteurModN":
                    temp = int.Parse(gate.Attribute("NumOutputs").Value);
                    temp2 = int.Parse(gate.Attribute("N").Value);
                    return new DecompteurModN(temp2, temp);
                case "Multiplexer":
                    return new Multiplexer(numInputs, 1, int.Parse(((Double)(Math.Log(numInputs, 2))).ToString()));
                case "Decodeur":
                    return new CircLab.ComplexComponent.Decodeur(numInputs, int.Parse(((Double)(Math.Pow(2, numInputs))).ToString()));
                case "Encodeur":
                    return new CircLab.ComplexComponent.Encodeur(numInputs, int.Parse(((Double)(Math.Log(numInputs, 2))).ToString()));
                case "FullAdder":
                    return new CircLab.ComplexComponent.FullAdder(numInputs, 2);
                case "HalfAdder":
                    return new CircLab.ComplexComponent.HalfAdder(numInputs, 2);
                case "HalfSub":
                    return new CircLab.ComplexComponent.HalfSub(numInputs, 2);
                case "FullSub":
                    return new CircLab.ComplexComponent.FullSub(numInputs, 2);
                case "Comparateur":
                    return new CircLab.ComplexComponent.Comparateur(numInputs, 3);
                case "Demultiplexer":
                    temp = int.Parse(gate.Attribute("NumOutputs").Value);
                    return new CircLab.ComplexComponent.Demultiplexer(numInputs, temp, int.Parse(((Double)(Math.Log(temp, 2))).ToString()));

            }
            throw new ArgumentException("unknown gate");
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
        private void Frequency_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(elementsSelected[0] is Chronogramme)
                {
                    ((SequentialComponent.Chronogramme)elementsSelected[0]).Delay = float.Parse(Frequency.Text);
                }
                else
                {
                    if(elementsSelected[0] is SequentialComponent.Clock)
                    {
                        ((SequentialComponent.Clock)elementsSelected[0]).Delay = float.Parse(Frequency.Text);
                    }
                }
               
            }
            catch
            { }
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

        private void ComboBoxFrontNiveau_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboBoxFrontNiveau.SelectedIndex == 0) //Front montant 
                {
                    if (elementsSelected[0] is SequentialComponent.FlipFlop)
                    {
                        ((SequentialComponent.FlipFlop)elementsSelected[0]).Trigger = FlipFlop.TriggerType.RisingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.Registre))
                    {
                        ((SequentialComponent.Registre)elementsSelected[0]).Trigger = Registre.TriggerType.RisingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                    {
                        ((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger = CirculerRegister.TriggerType.RisingEdge; ;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        ((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger = programmablRegister.TriggerType.RisingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.JK))
                    {
                        ((SequentialComponent.JK)elementsSelected[0]).Trigger = JK.TriggerType.RisingEdge;
                    }
                }
                else if (ComboBoxFrontNiveau.SelectedIndex == 1) //front descandant
                {
                    if (elementsSelected[0] is SequentialComponent.FlipFlop)
                    {
                        ((SequentialComponent.FlipFlop)elementsSelected[0]).Trigger = FlipFlop.TriggerType.FallingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.Registre))
                    {
                        ((SequentialComponent.Registre)elementsSelected[0]).Trigger = Registre.TriggerType.FallingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                    {
                        ((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger = CirculerRegister.TriggerType.FallingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        ((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger = programmablRegister.TriggerType.FallingEdge;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.JK))
                    {
                        ((SequentialComponent.JK)elementsSelected[0]).Trigger = JK.TriggerType.FallingEdge;
                    }
                }
                else if (ComboBoxFrontNiveau.SelectedIndex == 2) //Niveau haut
                {
                    if (elementsSelected[0] is SequentialComponent.FlipFlop)
                    {
                        ((SequentialComponent.FlipFlop)elementsSelected[0]).Trigger = FlipFlop.TriggerType.HighLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.Registre))
                    {
                        ((SequentialComponent.Registre)elementsSelected[0]).Trigger = Registre.TriggerType.HighLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                    {
                        ((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger = CirculerRegister.TriggerType.HighLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        ((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger = programmablRegister.TriggerType.HighLevel;
                    }

                }
                else
                {
                    if (elementsSelected[0] is SequentialComponent.FlipFlop)
                    {
                        ((SequentialComponent.FlipFlop)elementsSelected[0]).Trigger = FlipFlop.TriggerType.LowLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.Registre))
                    {
                        ((SequentialComponent.Registre)elementsSelected[0]).Trigger = Registre.TriggerType.LowLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.CirculerRegister))
                    {
                        ((SequentialComponent.CirculerRegister)elementsSelected[0]).Trigger = CirculerRegister.TriggerType.LowLevel;
                    }
                    else if ((elementsSelected[0] is SequentialComponent.programmablRegister))
                    {
                        ((SequentialComponent.programmablRegister)elementsSelected[0]).Trigger = programmablRegister.TriggerType.LowLevel;
                    }
                }
            }
            catch { }
            
        }

        private void ComboBoxCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (elementsSelected[0] is SequentialComponent.compteurN)
                {
                    ((SequentialComponent.compteurN)elementsSelected[0]).Val = ComboBoxCompteur.SelectedIndex + 2;
                    ((SequentialComponent.compteurN)elementsSelected[0]).Nbroutputs = (int)Math.Floor(Math.Log(ComboBoxCompteur.SelectedIndex + 2, 2))+1;
                    ((SequentialComponent.compteurN)elementsSelected[0]).redessiner("M 0,0 L 30,0 L 30,30 L 0,30 z");

                }
                else if (elementsSelected[0] is SequentialComponent.CompteurModN)
                {
                    ((SequentialComponent.CompteurModN)elementsSelected[0]).Val = ComboBoxCompteur.SelectedIndex + 2;
                    ((SequentialComponent.CompteurModN)elementsSelected[0]).Nbroutputs = (int)Math.Floor(Math.Log(ComboBoxCompteur.SelectedIndex + 2, 2))+1;
                    ((SequentialComponent.CompteurModN)elementsSelected[0]).redessiner("M 0,0 L 30,0 L 30,30 L 0,30 z");
                }
                else if (elementsSelected[0] is SequentialComponent.DecompteurN)
                {
                    ((SequentialComponent.DecompteurN)elementsSelected[0]).Val = ComboBoxCompteur.SelectedIndex + 2;
                    ((SequentialComponent.DecompteurN)elementsSelected[0]).Nbroutputs = (int)Math.Floor(Math.Log(ComboBoxCompteur.SelectedIndex + 2, 2))+1;
                    ((SequentialComponent.DecompteurN)elementsSelected[0]).redessiner("M 0,0 L 30,0 L 30,30 L 0,30 z");
                }
                else if (elementsSelected[0] is SequentialComponent.DecompteurModN)
                {
                    ((SequentialComponent.DecompteurModN)elementsSelected[0]).Val = ComboBoxCompteur.SelectedIndex + 2;
                    ((SequentialComponent.DecompteurModN)elementsSelected[0]).Nbroutputs = (int)Math.Floor(Math.Log(ComboBoxCompteur.SelectedIndex + 2, 2))+1;
                    ((SequentialComponent.DecompteurModN)elementsSelected[0]).redessiner("M 0,0 L 30,0 L 30,30 L 0,30 z");
                }
            }
            catch
            {

            }
           
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


        public List<StandardComponent> parcourir_coupier()
        {
            UIElement[] tableau = new UIElement[1000];
            List<StandardComponent> liste = new List<StandardComponent>();
            int length = canvas.Children.Count;
            canvas.Children.CopyTo(tableau, 0);
            for (int i = 0; i < length; i++)
            {
                StandardComponent newChild = null;
                if ((tableau[i] is StandardComponent) && ((tableau[i] as StandardComponent).IsSelect))
                {

                    if (typeof(AND) == tableau[i].GetType())
                    {
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

                    else if (typeof(AsynchToogle) == tableau[i].GetType())
                    {
                        newChild = new AsynchToogle();
                    }

                    else if (typeof(Chronogramme) == tableau[i].GetType())
                    {
                        newChild = new Chronogramme((tableau[i] as Chronogramme).nbrInputs(),MainWindow.Delay);
                    }
                    else if (typeof(CirculerRegister) == tableau[i].GetType())
                    {
                        newChild = new CirculerRegister((tableau[i] as CirculerRegister)._trigger, (tableau[i] as CirculerRegister).nbrInputs(), (tableau[i] as CirculerRegister).typeDec);
                    }
                    else if (typeof(CompteurModN) == tableau[i].GetType())
                    {
                        newChild = new CompteurModN(1, (tableau[i] as CompteurModN).nbrOutputs());
                    }
                    else if (typeof(compteurN) == tableau[i].GetType())
                    {
                        newChild = new compteurN(1, (tableau[i] as compteurN).nbrOutputs());
                    }
                    else if (typeof(DecompteurN) == tableau[i].GetType())
                    {
                        newChild = new DecompteurN(1, (tableau[i] as DecompteurN).nbrOutputs());
                    }
                    else if (typeof(FlipFlop) == tableau[i].GetType())
                    {
                        newChild = new FlipFlop((tableau[i] as FlipFlop)._trigger);
                    }
                    else if (typeof(FrequencyDevider) == tableau[i].GetType())
                    {
                        newChild = new FrequencyDevider();
                    }

                    else if (typeof(JK) == tableau[i].GetType())
                    {
                        newChild = new JK((tableau[i] as JK).Trigger);
                    }

                    else if (typeof(programmablRegister) == tableau[i].GetType())
                    {
                        newChild = new programmablRegister((tableau[i] as programmablRegister)._trigger, (tableau[i] as programmablRegister).nbrInputs());
                    }

                    else if (typeof(Registre) == tableau[i].GetType())
                    {
                        newChild = new Registre((tableau[i] as Registre)._trigger, (tableau[i] as Registre).nbrInputs());
                    }

                    else if (typeof(RSHLatche) == tableau[i].GetType())
                    {
                        newChild = new RSHLatche();
                    }

                    else if (typeof(RSLatche) == tableau[i].GetType())
                    {
                        newChild = new RSLatche();
                    }

                    else if (typeof(SynchToogle) == tableau[i].GetType())
                    {
                        newChild = new SynchToogle();
                    }

                    liste.Add(newChild);
                    newChild.AllowDrop = true;
                    newChild.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    newChild.PreviewMouseMove += this.MouseMove;
                    newChild.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;

                    newChild.SetValue(Canvas.LeftProperty, tableau[i].GetValue(Canvas.LeftProperty));
                    newChild.SetValue(Canvas.TopProperty, tableau[i].GetValue(Canvas.TopProperty));

                    (newChild as StandardComponent).PosX = (tableau[i] as StandardComponent).PosX;

                    (newChild as StandardComponent).PosY = (tableau[i] as StandardComponent).PosY;

                    try
                    {
                        StandardComponent component = newChild as StandardComponent;
                        component.recalculer_pos();
                    }
                    catch { };

                    /**/
                    //miseAJourPile();
                    foreach (Terminal terminal in (tableau[i] as StandardComponent).inputStack.Children)
                    {
                        try
                        {
                            for (int j = 0; j < terminal.wires.Count; j++)
                            {
                                ((Wireclass)terminal.wires[j]).Destroy();

                            }
                        }
                        catch (ArgumentOutOfRangeException) { }

                    }
                    foreach (Terminal terminal in (tableau[i] as StandardComponent).OutputStack.Children)
                    {

                        for (int k = terminal.wires.Count - 1; k >= 0; k--)
                        {

                            ((Wireclass)terminal.wires[k]).Destroy();
                        }
                    }
                    canvas.Children.Remove(tableau[i]);
                }
            }
            return liste;
        }


        public List<StandardComponent> parcourir_copier()
        {
            UIElement[] tableau = new UIElement[1000];
            List<StandardComponent> liste = new List<StandardComponent>();
            int length = canvas.Children.Count;
            canvas.Children.CopyTo(tableau, 0);
            for (int i = 0; i < length; i++)
            {

                StandardComponent newChild = null;
                if ((tableau[i] is StandardComponent) && ((tableau[i] as StandardComponent).IsSelect))
                {

                    if (typeof(AND) == tableau[i].GetType())
                    {
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

                    else if (typeof(AsynchToogle) == tableau[i].GetType())
                    {
                        newChild = new AsynchToogle();
                    }

                    else if (typeof(Chronogramme) == tableau[i].GetType())
                    {
                        newChild = new Chronogramme((tableau[i] as Chronogramme).nbrInputs(),MainWindow.Delay);
                    }
                    else if (typeof(CirculerRegister) == tableau[i].GetType())
                    {
                        newChild = new CirculerRegister((tableau[i] as CirculerRegister)._trigger, (tableau[i] as CirculerRegister).nbrInputs(), (tableau[i] as CirculerRegister).typeDec);
                    }
                    else if (typeof(CompteurModN) == tableau[i].GetType())
                    {
                        newChild = new CompteurModN(1, (tableau[i] as CompteurModN).nbrOutputs());
                    }
                    else if (typeof(compteurN) == tableau[i].GetType())
                    {
                        newChild = new compteurN(1, (tableau[i] as compteurN).nbrOutputs());
                    }
                    else if (typeof(DecompteurN) == tableau[i].GetType())
                    {
                        newChild = new DecompteurN(1, (tableau[i] as DecompteurN).nbrOutputs());
                    }
                    else if (typeof(FlipFlop) == tableau[i].GetType())
                    {
                        newChild = new FlipFlop((tableau[i] as FlipFlop)._trigger);
                    }
                    else if (typeof(FrequencyDevider) == tableau[i].GetType())
                    {
                        newChild = new FrequencyDevider();
                    }

                    else if (typeof(JK) == tableau[i].GetType())
                    {
                        newChild = new JK((tableau[i] as JK).Trigger);
                    }

                    else if (typeof(programmablRegister) == tableau[i].GetType())
                    {
                        newChild = new programmablRegister((tableau[i] as programmablRegister)._trigger, (tableau[i] as programmablRegister).nbrInputs());
                    }

                    else if (typeof(Registre) == tableau[i].GetType())
                    {
                        newChild = new Registre((tableau[i] as Registre)._trigger, (tableau[i] as Registre).nbrInputs());
                    }

                    else if (typeof(RSHLatche) == tableau[i].GetType())
                    {
                        newChild = new RSHLatche();
                    }

                    else if (typeof(RSLatche) == tableau[i].GetType())
                    {
                        newChild = new RSLatche();
                    }

                    else if (typeof(SynchToogle) == tableau[i].GetType())
                    {
                        newChild = new SynchToogle();
                    }

                    liste.Add(newChild);
                    newChild.AllowDrop = true;
                    newChild.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    newChild.PreviewMouseMove += this.MouseMove;
                    newChild.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
                    newChild.SetValue(Canvas.LeftProperty, tableau[i].GetValue(Canvas.LeftProperty));
                    newChild.SetValue(Canvas.TopProperty, tableau[i].GetValue(Canvas.TopProperty));
                    (newChild as StandardComponent).PosX = (tableau[i] as StandardComponent).PosX;
                    (newChild as StandardComponent).PosY = (tableau[i] as StandardComponent).PosY;

                    try
                    {
                        StandardComponent component = newChild as StandardComponent;
                        component.recalculer_pos();
                    }
                    catch { };
                }
            }
            return liste;
        }

        public void copier(object sender, RoutedEventArgs e)
        {
            liste_copier.Clear();
            List<StandardComponent> liste = parcourir_copier();

            for (int i = 0; i < liste.Count; i++)
            {
                liste_copier.Add(liste[i]);
            }

            
        }

        public void couper(object sender, RoutedEventArgs e)
        {
            List<StandardComponent> liste = parcourir_coupier();
            liste_copier.Clear();

            for (int i = 0; i < liste.Count; i++)
            {
                liste_copier.Add(liste[i]);
            }

        }


        public void coller(object sender, RoutedEventArgs e)
        {
            StandardComponent[] tmp = new StandardComponent[1000];
            int size = liste_copier.Count;
            liste_copier.CopyTo(tmp);
            //remplir(liste_tmp, tmp, size);

            for (int j = 0; j < liste_copier.Count; j++)
            {
                canvas.Children.Add(liste_copier[j]);
            }
            liste_copier.Clear();
            remplir(liste_copier, tmp, size);
        }

        public void remplir(List<StandardComponent> liste, StandardComponent[] tableau, int size)
        {
            //liste.Clear();
            //System.Windows.MessageBox.Show(tableau.Length.ToString());
            for (int i = 0; i < size; i++)
            {

                StandardComponent newChild = null;
                if (typeof(AND) == tableau[i].GetType())
                {
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

                else if (typeof(AsynchToogle) == tableau[i].GetType())
                {
                    newChild = new AsynchToogle();
                }

                else if (typeof(Chronogramme) == tableau[i].GetType())
                {
                    newChild = new Chronogramme((tableau[i] as Chronogramme).nbrInputs(),MainWindow.Delay);
                }
                else if (typeof(CirculerRegister) == tableau[i].GetType())
                {
                    newChild = new CirculerRegister((tableau[i] as CirculerRegister)._trigger, (tableau[i] as CirculerRegister).nbrInputs(), (tableau[i] as CirculerRegister).typeDec);
                }
                else if (typeof(CompteurModN) == tableau[i].GetType())
                {
                    newChild = new CompteurModN(1, (tableau[i] as CompteurModN).nbrOutputs());
                }
                else if (typeof(compteurN) == tableau[i].GetType())
                {
                    newChild = new compteurN(1, (tableau[i] as compteurN).nbrOutputs());
                }
                else if (typeof(DecompteurN) == tableau[i].GetType())
                {
                    newChild = new DecompteurN(1, (tableau[i] as DecompteurN).nbrOutputs());
                }
                else if (typeof(FlipFlop) == tableau[i].GetType())
                {
                    newChild = new FlipFlop((tableau[i] as FlipFlop)._trigger);
                }
                else if (typeof(FrequencyDevider) == tableau[i].GetType())
                {
                    newChild = new FrequencyDevider();
                }

                else if (typeof(JK) == tableau[i].GetType())
                {
                    newChild = new JK((tableau[i] as JK).Trigger);
                }

                else if (typeof(programmablRegister) == tableau[i].GetType())
                {
                    newChild = new programmablRegister((tableau[i] as programmablRegister)._trigger, (tableau[i] as programmablRegister).nbrInputs());
                }

                else if (typeof(Registre) == tableau[i].GetType())
                {
                    newChild = new Registre((tableau[i] as Registre)._trigger, (tableau[i] as Registre).nbrInputs());
                }

                else if (typeof(RSHLatche) == tableau[i].GetType())
                {
                    newChild = new RSHLatche();
                }

                else if (typeof(RSLatche) == tableau[i].GetType())
                {
                    newChild = new RSLatche();
                }

                else if (typeof(SynchToogle) == tableau[i].GetType())
                {
                    newChild = new SynchToogle();
                }

                liste.Add(newChild);
                newChild.AllowDrop = true;
                newChild.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                newChild.PreviewMouseMove += this.MouseMove;
                newChild.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
                newChild.SetValue(Canvas.LeftProperty, tableau[i].GetValue(Canvas.LeftProperty));
                newChild.SetValue(Canvas.TopProperty, tableau[i].GetValue(Canvas.TopProperty));
                (newChild as StandardComponent).PosX = (tableau[i] as StandardComponent).PosX;
                (newChild as StandardComponent).PosY = (tableau[i] as StandardComponent).PosY;

                try
                {
                    StandardComponent component = newChild as StandardComponent;
                    component.recalculer_pos();
                }
                catch { };
            }
        }

        private void ComboBoxPropertiesDec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (elementsSelected != null)
            {
                if (elementsSelected.Count != 0)
                {
                    int selecteVal;
                    if(elementsSelected[0] is Decodeur) selecteVal = ComboBoxPropertiesDec.SelectedIndex + 2;
        
                    else
                    {
                        if (ComboBoxPropertiesDec.SelectedIndex == 0) selecteVal = 4;
                        else selecteVal = 8;
                    }
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
                   modifieProperties();

                }
            }
        }

        private void ComboBoxPropertiesEnc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (elementsSelected != null)
            {
                if (elementsSelected.Count != 0)
                {
                    int selecteVal;
                    if (ComboBoxPropertiesEnc.SelectedIndex == 0) selecteVal = 4;
                    else selecteVal = 8;

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

                    modifieProperties();

                }
            }
        }

        private void ComparatuerText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (elementsSelected != null)
            {
                if (elementsSelected.Count != 0)
                {
                    int selecteVal=0;
                  
                    try
                    {
                        selecteVal = Int32.Parse(ComparatuerText.Text)*2;
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show("Entrez un nombre s'il vous plait ! ");
                        selecteVal = elementsSelected[0].nbrInputs();
                        Console.WriteLine(ex.Message);
                    }

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

                    modifieProperties();

                }
            }
        }

        private void ComboBoxPropertiesReg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxPropertiesReg.SelectedIndex == 0)
            {
                try
                {
                    ((CirculerRegister)elementsSelected[0]).typeDec = CirculerRegister.Type.Right;
                }
                catch
                {

                }



            }
            else
            {
                try
                {
                    ((CirculerRegister)elementsSelected[0]).typeDec = CirculerRegister.Type.Left;
                }
                catch
                {

                }
            }
        }


        // Rac Clavier 

        private void Window1_EditFull_KeyDown(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.Z && retourBouton.IsEnabled)
                {
                    retour(sender, e);
                }
                if (e.Key == Key.Y && refaireBouton.IsEnabled)
                {
                    refaire(sender, e);
                }

                if (e.Key == Key.C)
                {
                    copier(sender, e);
                }

                if (e.Key == Key.X)
                {
                    couper(sender, e);
                }

                if (e.Key == Key.V)
                {
                    coller(sender, e);
                }
                if (e.Key == Key.Delete)
                {
                    if (elementsSelected.Count != 0)
                    {
                        elementsSelected[0].Delete_elements();
                    }
                }
                if (e.Key == Key.A)
                {
                    elementsSelected.Clear();
                    foreach(Object component in canvas.Children)
                    {
                        if(component is StandardComponent)
                        {
                            ((StandardComponent)component).IsSelect = true;
                            StandardComponent.selectElement((StandardComponent)component);
                            elementsSelected.Add((StandardComponent)component);
                        }
                        
                    }
                }
                if (e.Key == Key.R)
                {
                    foreach(Object componenet in elementsSelected)
                    {
                        if(componenet is StandardComponent)
                        {
                            ((StandardComponent)componenet).RotateRight();
                        }
                       
                    }
                }
                if (e.Key == Key.L)
                {
                    foreach (Object componenet in elementsSelected)
                    {
                        if (componenet is StandardComponent)
                        {
                            ((StandardComponent)componenet).RotateLeft();
                        }

                    }
                }

            }
        }


    }


}


