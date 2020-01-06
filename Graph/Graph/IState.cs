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
namespace Graph
{
    public interface IState
    {
        IState DragNode(object sender, MouseEventArgs e);
        IState DrawNode(Point p);
        IState SelectNode(Point p);
        IState ChangeColor(Point p);
        
    }
}
