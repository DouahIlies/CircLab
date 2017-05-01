using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using CircLab.Component;
using CircLab.LogicGate;
using CircLab.SequentialComponent;

namespace CircLab
{
    static class CircuitXML
    {
        static public XElement CreateCircuitXML(Canvas canvas)
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
                    if (shape is Clock)
                    {
                        gt.SetAttributeValue("HighLevelms", ((Clock)g).HighLevelms);
                        gt.SetAttributeValue("LowLevelms", ((Clock)g).LowLevelms);
                    }
                    if(shape is JK)
                    {
                        if (((JK)shape).Trigger == JK.TriggerType.FallingEdge)
                            gt.SetAttributeValue("TriggerType", 0);
                        else gt.SetAttributeValue("TriggerType", 1);
                    }
                    if(shape is Registre)
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
                        if (((CirculerRegister)shape).CircularType == CirculerRegister.Type.Left)
                            gt.SetAttributeValue("CircularType", 0);
                        else if (((CirculerRegister)shape).CircularType == CirculerRegister.Type.Right)
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

        public static void Save(string path, Canvas canvas)
        {
            CreateCircuitXML(canvas).Save(path);
        }

        

        private static StandardComponent CreateGate(XElement gate)
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
                    return new Clock(temp, temp2, MainWindow.Delay);
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
                case "Registre":
                    temp = int.Parse(gate.Attribute("TriggerType").Value);
                    switch(temp)
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





            }
            throw new ArgumentException("unknown gate");
        }
        public static void Load(string path, ref Canvas canvas)
        {
            var circuit = XElement.Load(path);
            canvas.Children.Clear();
            canvas.Width = int.Parse(circuit.Attribute("Width").Value);
            canvas.Height = int.Parse(circuit.Attribute("Height").Value);
            var gid = new Dictionary<int, StandardComponent>();
            foreach (XElement gate in circuit.Element("Gates").Elements())
            {
                StandardComponent shape = CreateGate(gate);
                shape.SetValue(Canvas.LeftProperty, double.Parse(gate.Attribute("X").Value));
                shape.SetValue(Canvas.TopProperty, double.Parse(gate.Attribute("Y").Value));
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
                if(!int.TryParse(wire.Element("From").Attribute("ID").Value,out temp)) continue;
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
            }
        }
    }
}
