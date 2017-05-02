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
using CircLab.ComplexComponent;
using CircLab.LogicGate;
using CircLab.SequentialComponent;

namespace CircLab.Component
{
    /// <summary>
    /// Interaction logic for StandardComponent.xaml
    /// </summary>
    public abstract partial class StandardComponent : UserControl
    {
        public static Canvas canvas;//le canvas de l'interface 
        public Path typeComponenet; //Le path pour dessiner le composant concerné (And,Or,...)
        public static List<StandardComponent> liste_copier = new List<StandardComponent>();
        public String path;
        public Boolean IsSelect;
        public string type;
        public double PosX;//Position x dans le canvas
        public double PosY;//Position y dans le canvas 
        public int rotation = 0;


        protected ArrayList inputs_tab;
        protected ArrayList outputs_tab;
        protected ArrayList selections_tab;
        public static MainWindow fenetre;

        //Constructeur de tout les composonts
        public StandardComponent(int nbrinput, int nbrOutput, int nbrSelection, string path, string type)
        {
            inputs_tab = new ArrayList();
            outputs_tab = new ArrayList();
            if (nbrSelection != 0)
            {
                selections_tab = new ArrayList();
                for (int i = 0; i < nbrSelection; i++)
                {
                    selections_tab.Add(false);
                }
            }
            for (int i = 0; i < nbrinput; i++)
            {
                inputs_tab.Add(false);
            }
            for (int i = 0; i < nbrOutput; i++)
            {
                outputs_tab.Add(false);
            }

            selections_tab = new ArrayList();
            InitializeComponent();
            this.type = type;
            this.path = path;
            Terminal terminal = new Terminal();//on crée un terminal 
            typeComponenet = new Path();//le nombre d'input ;

            for (int i = 0; i < nbrinput; i++)
            {
                terminal = new Terminal();
                terminal.IsOutpt = false;
                inputStack.Children.Add(terminal);
                if (nbrOutput != 1)
                    terminal.Margin = new Thickness(0, ((Math.Max(nbrinput, nbrOutput) * terminal.Height)) / (Math.Pow(2, nbrinput)) - terminal.Height / 2, 0, terminal.Height / (nbrinput + 10));
            }

            OutputStack.Children.Remove(output);
            if (nbrSelection > 2)//here 
            {

                OutputStack.Margin = new Thickness(nbrSelection * 1.5 * 12, 25, 0, 0);

            }

            for (int i = 0; i < nbrOutput; i++)
            {

                terminal = new Terminal();
                terminal.Margin = new Thickness(0, ((Math.Max(nbrinput, nbrOutput) * terminal.Height)) / (Math.Pow(2, nbrOutput)) - terminal.Height / 2, 0, terminal.Height / (nbrinput + 10));

                terminal.terminal_grid.LayoutTransform = new RotateTransform(180);
                terminal.IsOutpt = true;
                OutputStack.Children.Add(terminal);


            }



            //output.Margin = new Thickness(15, 15, 15, 15);

            output.IsOutpt = true;//defini que c'est une sortie ; 
            if (nbrOutput != 0)
            {
                output = (Terminal)OutputStack.Children[0];

            }//Pour dessiner le composant
            typeComponenet.Height = terminal.Height * Math.Max(nbrinput, nbrOutput);
            if (nbrSelection > 2)
            {
                typeComponenet.Width = terminal.Width * 1.5 * nbrSelection; //here 
            }
            else { typeComponenet.Width = terminal.Width * 4; }

            typeComponenet.Data = StreamGeometry.Parse(path);
            typeComponenet.Stretch = Stretch.Fill;
            typeComponenet.StrokeThickness = 0;
            typeComponenet.Fill = Brushes.RoyalBlue;
            typeComponenet.Margin = new Thickness(14, 25, 0, 0);
            typeComponenet.HorizontalAlignment = HorizontalAlignment.Left;
            typeComponenet.VerticalAlignment = VerticalAlignment.Top;
            grid.Height = terminal.Height * Math.Max(nbrinput, nbrOutput) + 25;
            grid.Children.Add(typeComponenet);


            selectionStack.Margin = new Thickness(terminal.Width, 0, terminal.Width, 0);

            for (int i = 0; i < nbrSelection; i++)
            {
                Terminal terminalSelection = new Terminal();
                double x = terminalSelection.terminal_grid.Height;

                terminalSelection.LayoutTransform = new RotateTransform(90);


                terminalSelection.Margin = new Thickness(-terminal.Width / Math.Pow(2, nbrSelection) - terminal.Width + 3, 0, 0, 2);
                selectionStack.Children.Add(terminalSelection);
            }



        }
        //Methode pour recalculer la position du composants, on calcule la pos de chaque terminal prenant en considération les filles liées à lui
        public void recalculer_pos()
        {
            foreach (Terminal terminal in inputStack.Children)
            {
                terminal.recalculer(this.rotation);
            }
            foreach (Terminal terminal in OutputStack.Children)
            {
                terminal.recalculer(this.rotation);
            }
            foreach (Terminal terminal in selectionStack.Children)
            {
               
                terminal.recalculer(this.rotation);
            }


        }

        public void Delete(object sender, RoutedEventArgs e)
        {

            this.Delete_elements();
        }

        public void Delete_elements()
        {
            //StandardComponent component =UserClass.TryFindParent<StandardComponent>((((MenuItem)sender).Parent as ContextMenu).PlacementTarget);
            foreach (StandardComponent component in MainWindow.elementsSelected)
            {
           //     fenetre.miseAJourPile();
                if(component is Chronogramme)
                {
                    ((Chronogramme)component).remove();
                }
                foreach (Terminal terminal in component.inputStack.Children)
                {
                    try
                    {
                        for (int i = 0; i < terminal.wires.Count; i++)
                        {
                            ((Wireclass)terminal.wires[i]).Destroy();

                        }
                    }
                    catch (ArgumentOutOfRangeException) { }
                    /*foreach (Wireclass wire in terminal.wires)
                    {
                        wire.Destroy();
                    }*/
                }
                foreach (Terminal terminal in component.OutputStack.Children)
                {

                    for (int i = terminal.wires.Count - 1; i >= 0; i--)
                    {

                        ((Wireclass)terminal.wires[i]).Destroy();
                    }
              
                }
                foreach (Terminal terminal in component.selectionStack.Children)
                {

                    for (int i = terminal.wires.Count - 1; i >= 0; i--)
                    {

                        ((Wireclass)terminal.wires[i]).Destroy();
                    }

                }
                canvas.Children.Remove(component);
                MainWindow window = UserClass.TryFindParent<MainWindow>(canvas);
                window.desactiveProp();
            }
        }

        public abstract void Run();





        public void AddInputs()
        {
            Terminal terminal = new Terminal();
            terminal.IsOutpt = false;
            inputStack.Children.Add(terminal);
            inputs_tab.Add(false);


        }

        public void RemoveInputs()
        {
            Terminal terminal = null;
            Wireclass wire = null;

            foreach (Terminal tmp in inputStack.Children)
            {
                terminal = tmp;
            }
            foreach (Wireclass tmp in terminal.wires)
            {
                wire = tmp;
            }
            if (wire != null) wire.Destroy();
            inputStack.Children.Remove(terminal);
            try
            {
                inputs_tab.RemoveAt(1);
            }
            catch
            {

            }


        }

        private void standardcomponent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.elementsSelected != null && !(Mouse.RightButton == MouseButtonState.Pressed))
            {

                if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)) //Une selection simple 
                {

                    if (!MainWindow.elementsSelected.Contains((StandardComponent)sender)) //si ce n'est pas un clic sur le meme element
                    {
                        foreach (StandardComponent elementSelected in MainWindow.elementsSelected) //Deslectionner tout les elements selectionner
                        {
                            deSelectElement(elementSelected);
                        }
                        MainWindow.elementsSelected.Clear();
                        MainWindow.elementsSelected.Add(this);
                        this.IsSelect = true;
                        selectElement(this);

                    }
                    else
                    {

                        int i = MainWindow.elementsSelected.IndexOf((StandardComponent)sender);
                        deSelectElement(MainWindow.elementsSelected[i]);
                        MainWindow.elementsSelected.Remove((StandardComponent)sender);

                    }

                }
                else
                {
                    if (MainWindow.elementsSelected.Contains((StandardComponent)sender))
                    {
                        int i = MainWindow.elementsSelected.IndexOf((StandardComponent)sender);
                        deSelectElement(MainWindow.elementsSelected[i]);
                        MainWindow.elementsSelected.Remove((StandardComponent)sender);
                    }
                    else
                    {
                        MainWindow.elementsSelected.Add(this);
                        this.IsSelect = true;
                        selectElement(this);

                    }


                }




            }



            MainWindow window = UserClass.TryFindParent<MainWindow>(canvas);
            if (UserClass.IsInputChangeable((StandardComponent)sender))
            {
                window.activeProp();
                window.modifieProperties();
                if (sender is XOR || sender is XNOR)
                {
                    window.NbrEntreText.Visibility = Visibility.Collapsed;
                    window.ComboBoxProperties.Visibility = Visibility.Collapsed;
                }

            }
            else
            {
                window.desactiveProp();
                window.modifieProperties();

            }


        }

        public static void selectElement(StandardComponent component)
        {
            component.typeComponenet.StrokeThickness = 2;
            component.typeComponenet.Stroke = Brushes.Black;
        }

        public static void deSelectElement(StandardComponent component)
        {
            component.typeComponenet.Stroke = Brushes.RoyalBlue;
            component.IsSelect = false;
        }

        public int nbrInputs()
        {
            int tmp = 0;
            foreach (Terminal terminal in inputStack.Children)
            {
                tmp++;
            }
            return tmp;
        }

        public int nbrOutputs()
        {
            int tmp = 0;
            foreach (Terminal terminal in OutputStack.Children)
            {
                tmp++;
            }
            return tmp;
        }

        public int nbrSelections()
        {
            int tmp = 0;
            foreach (Terminal terminal in selectionStack.Children)
            {
                tmp++;
            }
            return tmp;
        }

        public virtual void redessiner(string path)
        {
            Terminal terminal = new Terminal();
            int nbrInput;
            foreach (Terminal tmp in inputStack.Children)
            {
                terminal = tmp;
            }

            if (this.nbrInputs() == 0)
            {
                nbrInput = 1;
            }
            else nbrInput = this.nbrInputs();

            output.Margin = new Thickness(4.5, 11 * (nbrInput - 1), 4.5, 0);
            grid.Height = nbrInput * 22 + 25;
            typeComponenet.Height = terminal.Height * nbrInput;
            typeComponenet.Width = terminal.Width * 4;

            typeComponenet.Data = StreamGeometry.Parse(path);
            typeComponenet.Stretch = Stretch.Fill;
            typeComponenet.StrokeThickness = 0;
            typeComponenet.Fill = Brushes.RoyalBlue;
            typeComponenet.Margin = new Thickness(14, 25, 0, 0);
            typeComponenet.HorizontalAlignment = HorizontalAlignment.Left;
            typeComponenet.VerticalAlignment = VerticalAlignment.Top;
            recalculer_pos();
            if (IsSelect) selectElement(this);
            canvas.UpdateLayout();

        }

        public void update_input()
        {
    
            selections_tab.Clear();

            int i = 0;
            foreach (Terminal terminal in inputStack.Children)
            {
                inputs_tab[i] = (false);
                if (terminal.wires.Count != 0)
                {

                    Wireclass wire = (Wireclass)terminal.wires[0];
                    if (!terminal.IsInversed)
                    {
                        inputs_tab[i] = wire.state;

                    }
                    else
                    {
                        inputs_tab[i] = !wire.state;
                    }

                }
                else
                {
                    if (terminal.IsInversed)
                    {
                        inputs_tab[i] = true;
                    }
                }
                i++;
            }
            i = 0;
     
            foreach (Terminal terminal in selectionStack.Children)
            {
                

                selections_tab.Add(false);
                if (terminal.wires.Count != 0)
                {

                    Wireclass wire = (Wireclass)terminal.wires[0];
                    if (!terminal.IsInversed)
                    {
                        selections_tab[i] = wire.state;

                    }
                    else
                    {
                        selections_tab[i] = !wire.state;
                    }

                }
                else
                {
                    if (terminal.IsInversed)
                    {
                        selections_tab[i] = true;
                    }
                }
                i++;
               
            }
        }

        public void update_output()
        {
            int i = 0;
            foreach (Terminal terminal in OutputStack.Children)
            {
                foreach (Wireclass wire in terminal.wires)
                {
                    wire.state = (Boolean)outputs_tab[i];
                }
                i++;
            }
        }

        public void RotateRightEvent(object sender, RoutedEventArgs e)
        {

            this.RotateRight();
        }

        public void RotateRight()
        {
            rotation += 90;
            if (rotation == 360)
            {
                rotation = 0;
            }
            RotateComponent(rotation);
            canvas.UpdateLayout();
        }

        public void RotateLeftEvent(object sender, RoutedEventArgs e)
        {
            this.RotateLeft();
        }

        public void RotateLeft()
        {
            rotation -= 90;
            if (rotation == -360)
            {
                rotation = 0;
            }
            RotateComponent(rotation);
        }

        public void RotateComponent(int rotation)
        {
            RotateTransform rt = new RotateTransform(rotation);
            this.LayoutTransform = rt;
            this.recalculer_pos();
            canvas.UpdateLayout();
            this.recalculer_pos();

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
                if (!(typeof(Line) == tableau[i].GetType()) && ((tableau[i] as StandardComponent).IsSelect))
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
                    newChild.PreviewMouseLeftButtonDown += fenetre.MouseLeftButtonDown;
                    newChild.PreviewMouseMove += fenetre.MouseMove;
                    newChild.PreviewMouseLeftButtonUp += fenetre.PreviewMouseLeftButtonUp;

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
                    fenetre.miseAJourPile();
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
                if (!(typeof(Line) == tableau[i].GetType()) && ((tableau[i] as StandardComponent).IsSelect))
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
                    newChild.PreviewMouseLeftButtonDown += fenetre.MouseLeftButtonDown;
                    newChild.PreviewMouseMove += fenetre.MouseMove;
                    newChild.PreviewMouseLeftButtonUp += fenetre.PreviewMouseLeftButtonUp;
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
            for (int j = 0; j < liste_copier.Count; j++)
            {
                canvas.Children.Add(liste_copier[j]);
            }
        }


        // Rac Clavier 

    }
}

