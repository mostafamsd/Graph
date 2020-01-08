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
    static class DefaultValues
    {
        public static double NodeRaduis = 20;
        public static Brush NodeFill = Brushes.White;
        public static Brush NodeStrokeColor = Brushes.Black;
        public static Brush EdgeFill = Brushes.Black;
        public static double EdgeStrokeThickness = 3;

    }
    public class SimpleGraph
    {
        public List<Node> Nodes;
        public List<Edge> Edges;
        public SimpleGraph()
        {
            this.Nodes = new List<Node>();
            this.Edges = new List<Edge>();
        }
        public Edge AddEdge(Node src,Node dest)
        {
            Edge e = new Edge(src, dest);
            Edges.Add(e);
            src.AddNeighbor(dest);
            return e;
        }
        public void AddNode(Node newnode)
        {
            Nodes.Add(newnode);
        }
        public Node Find(long value)
        {
            foreach(var n in Nodes)
            {
                if (n.Value == value)
                    return n;
            }
            return null;
        }
        public void Clear()
        {
            this.Nodes.Clear();
            this.Edges.Clear();
        }
    }
    public class UIEdge
    {
        public Line Line;
        public Node src;
        public Node dest;
        private Brush _Color;
        public Brush Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                Line.Stroke = value;
            }
        }
        public UIEdge(Node src,Node dest)
        {
            this.src = src;
            this.dest = dest;
            this.Line = MakeLine();
            UpdateLine();
            this.Color = DefaultValues.EdgeFill;
        }
        private Line MakeLine()
        {
            Line line = new Line();
            line.StrokeThickness = DefaultValues.EdgeStrokeThickness;
            line.Stroke = DefaultValues.EdgeFill;
            Panel.SetZIndex(line, 0);
            return line;
        }
        public void UpdateLine()
        {
            Line.X1 = src.XCordinate;
            Line.Y1 = src.YCordinate;
            Line.X2 = dest.XCordinate;
            Line.Y2 = dest.YCordinate;
        }
    }
    public class Edge
    {
        public Node src;
        public Node dest;
        public UIEdge UiEdge;
        public long Length;
        public Edge(Node s,Node d)
        {
            this.src = s;
            this.dest = d;
            this.UiEdge = new UIEdge(s,d);
            UiEdge.Line.MouseLeftButtonDown += Edge_MouseLeftButtonDown;
        }

        private void Edge_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.UnSelectElement(MainWindow.SelectedElement);
            MainWindow.SelectElement((UIElement)sender);
            MainWindow.SelectedElement = (UIElement)sender;
            MainWindow.Main.State.ChangeColor(this);
        }
    }
    public class UINode
    {
        private Brush _Color;
        private double _Radius;
        public long Value;
        public Border Border;
        public TextBlock Textblock;
        private double CanvasLeft;
        private double CanvasTop;

        public double Radius
        {
            get
            {
                return _Radius;
            }
            set
            {
                UpdateBorder(value - _Radius);
                _Radius = value;
            }
        }
        public Brush Color
        {
            set
            {
                Border.Background = value;
                _Color = value;
            }
            get
            {
                return _Color;
            }
        }
        public UINode(long value,double XCenter,double YCenter)
        {
            Value = value;
            Border = MakeBorder();
            Textblock = MakeTextBlock();
            Border.Child = Textblock;
            SetCanvasLeft(XCenter - DefaultValues.NodeRaduis);
            SetCanvasTop(YCenter - DefaultValues.NodeRaduis);
            _Radius = DefaultValues.NodeRaduis;
            Color = DefaultValues.NodeFill;
        }

        public void SetCanvasTop(double canvastop)
        {
            this.CanvasTop = canvastop;
            Canvas.SetTop(Border, CanvasTop);
        }

        public void SetCanvasLeft(double canvasleft)
        {
            this.CanvasLeft = canvasleft;
            Canvas.SetLeft(Border, CanvasLeft);
        }

        private TextBlock MakeTextBlock()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = Value.ToString();
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontSize = 20;
            Panel.SetZIndex(textBlock, 2);
            return textBlock;
        }
        private Border MakeBorder()
        {
            Border border = new Border();
            border.Width = DefaultValues.NodeRaduis * 2;
            border.Height = DefaultValues.NodeRaduis * 2;
            border.CornerRadius = new CornerRadius(1000);
            border.BorderBrush = DefaultValues.NodeStrokeColor;
            border.BorderThickness = new Thickness(3);
            border.Background = DefaultValues.NodeFill;
            Panel.SetZIndex(border, 1);
            return border;
        }
        private void UpdateBorder(double change)
        {
            Border.Width += change * 2;
            Border.Height += change * 2;
            SetCanvasLeft(Canvas.GetLeft(Border) - change);
            SetCanvasTop(Canvas.GetTop(Border) - change);
        }

    }
    public class Node
    {
        public long Value;
        public List<Node> Adj;
        private double _XCordinate;
        private double _YCordinate;
        public UINode UiNode;

        public double XCordinate
        {
            get
            {
                return _XCordinate;
            }
            set
            {
                _XCordinate = value;
                UiNode.SetCanvasLeft(value - UiNode.Radius);
                for (int i = 0; i < MainWindow.g.Edges.Count; i++)
                {
                    if (MainWindow.g.Edges[i].src == this || MainWindow.g.Edges[i].dest == this)
                        MainWindow.g.Edges[i].UiEdge.UpdateLine();
                }
            }
        }
        public double YCordinate
        {
            get
            {
                return _YCordinate;
            }
            set
            {
                _YCordinate = value;
                UiNode.SetCanvasTop(value - UiNode.Radius);
            }
        }
        
        public Node(long value,Point center)
        {
            this.Adj = new List<Node>();
            this.Value = value;
            this.UiNode = new UINode(Value, center.X, center.Y);
            this.XCordinate = center.X;
            this.YCordinate = center.Y;
            UiNode.Border.MouseLeftButtonDown += Node_MouseLeftButtonDown;
            UiNode.Border.MouseMove += Node_MouseMove;
            UiNode.Border.MouseWheel += Border_MouseWheel;
        }

        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0 && UiNode.Radius <= 60)
                this.UiNode.Radius += 1;
            else if (e.Delta > 0 && UiNode.Radius >= 15)
                this.UiNode.Radius -= 1;
        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            MainWindow.Main.State = MainWindow.Main.State.DragNode(this, e);
        }
        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.UnSelectElement(MainWindow.SelectedElement);
            MainWindow.SelectElement((UIElement)sender);
            MainWindow.SelectedElement = (UIElement)sender;
            MainWindow.Main.AllowAddNode = false;
            MainWindow.Main.State = MainWindow.Main.State.ChangeColor(this);
            MainWindow.Main.State = MainWindow.Main.State.SelectNode(this);
        }
        public void AddNeighbor(Node neighbor)
        {
            Adj.Add(neighbor);
            neighbor.Adj.Add(this);
        }
    }
}
