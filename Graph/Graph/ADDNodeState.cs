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
    public class ADDNodeState : IState
    {
        public ADDNodeState(Button oldButton,Button newButton)
        {
            MainWindow.UnSelectTool(oldButton);
            MainWindow.SelectTool(newButton);
            MainWindow.CurrentButton = newButton;
        }
        public IState DrawNode(Point p)
        {
            Node node;
            try
            {
                node = new Node(long.Parse(MainWindow.Main.NewNodeValue.Text), p);
            }
            catch (FormatException)
            {
                return this;
            }
            MainWindow.g.AddNode(node);
            MainWindow.Main.EnableDrag(node.UiNode.Border);
            MainWindow.Main.DrawingScreen.Children.Add(node.UiNode.Border);
            return this;
        }
        public IState SelectNode(object sender)
            => this;
        public IState ChangeColor(object sender)
            => this;
        public IState DragNode(object sender, MouseEventArgs e)
            => this;
    }
}
