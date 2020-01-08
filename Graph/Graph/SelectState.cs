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
                var now = e.GetPosition(MainWindow.Main.DrawingScreen);
                if(sender is Node)
                {
                    ((Node)sender).XCordinate = now.X - MainWindow.Main.DragStart.Value.X + ((Node)sender).UiNode.Radius;
                    ((Node)sender).YCordinate = now.Y - MainWindow.Main.DragStart.Value.Y + ((Node)sender).UiNode.Radius;
                }
            }
            return this;
        }
        public IState ChangeColor(object sender)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(object sender)
            => this;
    }
}
