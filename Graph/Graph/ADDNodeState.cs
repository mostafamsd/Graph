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
            Canvas Screen = MainWindow.Main.DrawingScreen;
            Node node;
            try
            {
                node = new Node(long.Parse(MainWindow.Main.NewNodeValue.Text));
                node.XCordinate = p.X ;
                node.YCordinate = p.Y ;
            }
            catch (FormatException)
            {
                return this;
            }

            MainWindow.g.AddNode(node);

            Border border = MakeBorder(node);
            TextBlock textBlock = MakeTextBlock(node);

            border.Child = textBlock;
            border.MouseLeftButtonDown += Node_MouseLeftButtonDown;
            Screen.Children.Add(border);
            MainWindow.Main.NodesOnScreen.Add(border);
            Canvas.SetLeft(border, node.XCordinate - node.Radius);
            Canvas.SetTop(border, node.YCordinate - node.Radius);
            MainWindow.Main.EnableDrag(border);
            return this;
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.UnSelectElement(MainWindow.SelectedElement);
            MainWindow.SelectElement((UIElement)sender);
            MainWindow.SelectedElement = (UIElement)sender;
            MainWindow.Main.AllowAddNode = false;
        }

        private static TextBlock MakeTextBlock(Node node)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = node.Value.ToString();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontSize = 20;
            Panel.SetZIndex(textBlock, 2);
            return textBlock;
        }

        private static Border MakeBorder(Node node)
        {
            Border border = new Border();
            border.Width = node.Radius * 2;
            border.Height = node.Radius * 2;
            border.CornerRadius = new CornerRadius(1000);
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(3);
            border.Background = node.Color;
            Panel.SetZIndex(border, 1);
            return border;
        }

        public IState SelectNode(Point p)
            => this;

        public IState ChangeColor(Point p)
            => this;

        public IState DragNode(object sender, MouseEventArgs e)
            => this;
    }
}
