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
    public class SelectSecondNodeState : IState
    {
        public Node FirstNode;
        public SelectSecondNodeState(Node firstnode)
        {
            this.FirstNode = firstnode;
        }

        public IState ChangeColor(object sender)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(object sender)
        {
            if(sender is Node)
            {
                if (((Node)sender) != FirstNode )
                {
                    if (!((Node)sender).Adj.Contains(FirstNode))
                    {
                        Edge edge = MainWindow.g.AddEdge(FirstNode, (Node)sender);
                        MainWindow.Main.DrawingScreen.Children.Add(edge.UiEdge.Line);
                        return new SelectFirstNodeState(null, MainWindow.Main.ADDEdgeButton);
                    }
                }
            }
            return this;
        }
        public IState DragNode(object sender, MouseEventArgs e)
            => this;
    }
}
