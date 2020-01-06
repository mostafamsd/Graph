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
    public class SelectFirstNodeState : IState
    {
        public SelectFirstNodeState(Button oldButton,Button newButton)
        {
            MainWindow.UnSelectTool(oldButton);
            MainWindow.SelectTool(newButton);
            MainWindow.CurrentButton = newButton;
        }
        public IState ChangeColor(Point p)
            => this;

        public IState DragNode(object sender, MouseEventArgs e)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(Point p)
        {
            List<Node> listOfNode = MainWindow.g.Nodes;
            double X = p.X;
            double Y = p.Y;
            Node first;
            for (int i = 0; i < listOfNode.Count; i++)
            {
                first = listOfNode[i];
                if (Math.Abs(first.XCordinate - X) < first.Radius
                    && Math.Abs(first.YCordinate - Y) < first.Radius)
                {
                    return new SelectSecondNodeState(first);
                }
            }
            return this;
        }
    }
}
