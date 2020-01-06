using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph
{
    public class ColorState : IState
    {
        public ColorState(Button oldButton,Button newButton)
        {
            MainWindow.UnSelectTool(oldButton);
            MainWindow.SelectTool(newButton);
            MainWindow.CurrentButton = newButton;
        }
        public IState ChangeColor(Point p)
        {
            List<Node> listofnode = MainWindow.g.Nodes;
            for (int i = 0; i < listofnode.Count; i++)
            {
                Node select = listofnode[i];
                if (Math.Abs(select.XCordinate - p.X) < select.Radius
                    && Math.Abs(select.YCordinate - p.Y) < select.Radius)
                {
                    SolidColorBrush color =
                        new SolidColorBrush(MainWindow.Main.colorpiker.SelectedColor.GetValueOrDefault());
                    select.Color = color;
                    (MainWindow.Main.NodesOnScreen[i]).Background = color;
                    return this;
                }
            }
            List<Edge> listofedge = MainWindow.g.Edges;
            for (int i = 0; i < listofedge.Count; i++)
            {
                Edge select = listofedge[i];
                if (Math.Abs(p.X - select.Center.X) <= Math.Abs(select.src.XCordinate - select.dest.XCordinate) / 2
                 && Math.Abs(p.Y - select.Center.Y) <= Math.Abs(select.src.YCordinate - select.dest.YCordinate) / 2)
                {
                    if (Math.Abs((p.X * select.M + select.B) - p.Y) <= 5)
                    {
                        
                        SolidColorBrush color =
                        new SolidColorBrush(MainWindow.Main.colorpiker.SelectedColor.GetValueOrDefault());
                        select.Color = color;
                        (MainWindow.Main.EdgesOnScreen[i]).Stroke = color;
                        return this;
                    }
                }
            }
            return this;
        }

        public IState DragNode(object sender, MouseEventArgs e)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(Point p)
            => this;
    }
}
