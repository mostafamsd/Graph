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

        public IState ChangeColor(Point p)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(Point p)
        {
            List<Node> listOfNode = MainWindow.g.Nodes;
            double X = p.X;
            double Y = p.Y;
            Node Second;
            for (int i = 0; i < listOfNode.Count; i++)
            {
                Second = listOfNode[i];
                if (Math.Abs(Second.XCordinate - X) < Second.Radius
                    && Math.Abs(Second.YCordinate - Y) < Second.Radius)
                {
                    if (FirstNode != Second && !FirstNode.Adj.Contains(Second))
                    {
                        Edge edge = MainWindow.g.AddEdge(FirstNode, Second);
                        Line L_edge = DrawEdge(edge);
                        L_edge.MouseLeftButtonDown += Edge_MouseLeftButtonDown;
                        //MainWindow.Main.EnableDrag(L_edge);
                        MainWindow.Main.DrawingScreen.Children.Add(L_edge);
                        MainWindow.Main.EdgesOnScreen.Add(L_edge);
                        return new SelectFirstNodeState(null,MainWindow.Main.ADDEdgeButton);
                    }
                }
            }
            return this;
        }

        private void Edge_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.UnSelectElement(MainWindow.SelectedElement);
            MainWindow.SelectElement((UIElement)sender);
            MainWindow.SelectedElement = (UIElement)sender;
        }

        private Line DrawEdge(Edge edge)
        {
            Line L_edge = new Line();
            L_edge.StrokeThickness = 3;
            L_edge.Stroke = edge.Color;
            L_edge.X1 = edge.src.XCordinate;
            L_edge.Y1 = edge.src.YCordinate;
            L_edge.X2 = edge.dest.XCordinate;
            L_edge.Y2 = edge.dest.YCordinate;
            Panel.SetZIndex(L_edge, 0);
            return L_edge;
        }

        public IState DragNode(object sender, MouseEventArgs e)
            => this;
    }
}
