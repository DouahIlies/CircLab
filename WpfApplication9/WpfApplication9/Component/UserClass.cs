using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CircLab.LogicGate;
using CircLab.SequentialComponent;

namespace CircLab.Component
{
    public static class UserClass
    {
        
        public static T TryFindParent<T>(DependencyObject current) where T : class
        {
            DependencyObject parent = VisualTreeHelper.GetParent(current);
            if (parent == null)
                return null;

            if (parent is T)
                return parent as T;
            else
                return TryFindParent<T>(parent);
        }

        public static T TryFindLogicalParent<T>(FrameworkElement current) where T : class
        {
            DependencyObject parent = current.Parent;
            if (parent == null)
                return null;

            if (parent is T)
                return parent as T;
            else
                return TryFindLogicalParent<T>((FrameworkElement)parent);
        }

        public static ArrayList FiltreSrandardComponent(Canvas canvas)
        {
            ArrayList arrayList = new ArrayList();
            try {
                for (int i = 0; i < canvas.Children.Capacity; i++)
                {
               //     MessageBox.Show(canvas.Children.Capacity.ToString() + "   " + i.ToString());
                    if (canvas.Children[i] is StandardComponent)
                    {
                        arrayList.Add(canvas.Children[i]);
                    }
                }
            }
            catch { }
            return arrayList;
        }

       public static bool IsFrontChangeable(StandardComponent component)
       {
            if (component is SequentialComponent.FlipFlop) return true;
            else if (component is SequentialComponent.programmablRegister) return true;
            else if (component is SequentialComponent.CirculerRegister) return true;
            else if (component is SequentialComponent.Registre) return true;
            else if (component is SequentialComponent.JK) return true;
            else return false;
       }

       public static bool IsNiveauChangeable(StandardComponent component)
       {
           if (component is SequentialComponent.FlipFlop) return true;
           else if (component is SequentialComponent.programmablRegister) return true;
           else if (component is SequentialComponent.CirculerRegister) return true;
           else if (component is SequentialComponent.Registre) return true;
           else return false;
       }

        public static bool IsInputChangeable(StandardComponent component)
        {
            if (component is AND) return true;
            else if (component is NAND) return true;
            else if (component is NOR) return true;
            else if (component is OR) return true;
            else if (component is Registre) return true;
            else if (component is CirculerRegister) return true;
            else if (component is programmablRegister) return true;
            else return false;

        }
        
        public static bool IsCompteur(StandardComponent component)
        {
            if (component is SequentialComponent.compteurN) return true;
            else if (component is SequentialComponent.CompteurModN) return true;
            else if (component is SequentialComponent.DecompteurN) return true;
            else if (component is SequentialComponent.DecompteurModN) return true;
            else return false;
        }
    }
}
