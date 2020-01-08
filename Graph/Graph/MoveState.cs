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
    class MoveState : IState
    {
        public MoveState(Button oldButton,Button newButton)
        {
            MainWindow.UnSelectTool(oldButton);
            MainWindow.SelectTool(newButton);
            MainWindow.CurrentButton = newButton;
        }
        
        public IState ChangeColor(object sender)
            => this;

        public IState DragNode(object sender, MouseEventArgs e)
            => this;

        public IState DrawNode(Point p)
            => this;

        public IState SelectNode(object sender)
            => this;
    }
}
