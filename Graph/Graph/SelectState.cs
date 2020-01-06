using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graph
{
    class SelectState : IState
    {
        public SelectState(Button oldButton,Button newButton)
        {
            MainWindow.UnSelectTool(oldButton);
            MainWindow.SelectTool(newButton);
            MainWindow.CurrentButton = newButton;
        }
        public IState DragNode(object sender, MouseEventArgs e)
        {
            if (MainWindow.Main.DragStart != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var element = (UIElement)sender;
                var now = e.GetPosition(MainWindow.Main.DrawingScreen);
                int i = MainWindow.Main.NodesOnScreen.IndexOf((Border)element);
                MainWindow.g.Nodes[i].XCordinate = now.X - MainWindow.Main.DragStart.Value.X + MainWindow.g.Nodes[i].Radius;
                MainWindow.g.Nodes[i].YCordinate = now.Y - MainWindow.Main.DragStart.Value.Y + MainWindow.g.Nodes[i].Radius;
                Canvas.SetLeft(element, now.X - MainWindow.Main.DragStart.Value.X);
                Canvas.SetTop(element, now.Y - MainWindow.Main.DragStart.Value.Y);
            }
            return this;
        }
        public IState ChangeColor(Point p)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(Point p)
            => this;
    }
}
