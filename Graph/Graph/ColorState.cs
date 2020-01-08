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
        public IState ChangeColor(object sender)
        {
            SolidColorBrush color =
                new SolidColorBrush(MainWindow.Main.colorpiker.SelectedColor.GetValueOrDefault());
            if(sender is Node)
                ((Node)sender).UiNode.Color = color;
            if (sender is Edge)
                ((Edge)sender).UiEdge.Color = color;
            return this;
        }

        public IState DragNode(object sender, MouseEventArgs e)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(object sender)
            => this;
    }
}
