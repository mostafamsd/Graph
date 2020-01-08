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
using QuickGraph;
using Microsoft.Msagl;
using Microsoft.Win32;
using System.IO;

namespace Graph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SimpleGraph g;
        public IState State;
        public static MainWindow Main;
        public List<Border> NodesOnScreen;
        public List<Line> EdgesOnScreen;
        public static Button CurrentButton;
        private double Rate = 1;
        public Point PreMousePoint;
        public static UIElement SelectedElement;
        public Point? DragStart = null;
        public bool AllowAddNode;
        public MainWindow()
        {
            InitializeComponent();
            g = new SimpleGraph();
            this.State = new SelectState(SelectButton, SelectButton);
            Main = (MainWindow)MainWindows;
            SelectedElement = null;
            AllowAddNode = true;
        }
        public static void SelectElement(UIElement element)
        {
            if (element == null)
                return;
            if (element is Line)
            {
                ((Line)element).StrokeThickness = 5;
            }
            else
            {
                ((Border)element).BorderThickness = new Thickness(5);
            }
        }
        public static void UnSelectElement(UIElement element)
        {
            if (element == null)
                return;
            if (element is Line)
            {
                ((Line)element).StrokeThickness = 3;
            }
            else
            {
                ((Border)element).BorderThickness = new Thickness(3);
            }
        }
        public static void SelectTool(Button b)
        {
            if (b != null)
                b.BorderThickness = new Thickness(3);
        }
        public static void UnSelectTool(Button b)
        {
            if (b != null)
                b.BorderThickness = new Thickness(1);
        }

        private void ADDNodeButton_Click(object sender, RoutedEventArgs e)
        {
            State = new ADDNodeState(CurrentButton, ADDNodeButton);
        }

        private void DrawingScreen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double XMouse = e.GetPosition(DrawingScreen).X;
            double YMouse = e.GetPosition(DrawingScreen).Y;
            Point mouse = new Point(XMouse, YMouse);
            if (AllowAddNode)
                State = State.DrawNode(mouse);

            //State = State.SelectNode(mouse);
            AllowAddNode = true;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            State = new SelectState(CurrentButton, SelectButton);
        }

        private void ADDEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            State = new SelectFirstNodeState(CurrentButton, ADDEdgeButton);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            State = new ColorState(CurrentButton, ColorButton);
        }

        private void DrawingScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentButton == MoveButton)
                PreMousePoint = e.GetPosition(DrawingScreen);

            double XMouse = e.GetPosition(DrawingScreen).X;
            double YMouse = e.GetPosition(DrawingScreen).Y;
            Point mouse = new Point(XMouse, YMouse);
            this.State = State.ChangeColor(mouse);
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            State = new MoveState(CurrentButton, MoveButton);
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            this.Rate += 0.2;
            DrawingScreen.LayoutTransform = new ScaleTransform(Rate, Rate);
            DrawingScreen.UpdateLayout();
        }
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Rate <= 1)
                return;
            this.Rate -= 0.2;
            DrawingScreen.LayoutTransform = new ScaleTransform(Rate, Rate);
            DrawingScreen.UpdateLayout();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadedfile = new OpenFileDialog();
            loadedfile.Filter = "Text file (*.txt*)|*.txt*|Json file (*.json*)|*.json*|Xml file (*.xml*)|*.xml*";
            if (loadedfile.ShowDialog() == false)
                return;

            FilePath.Text = loadedfile.FileName;
        }

        private void DrawGraphFromFile_Click(object sender, RoutedEventArgs e)
        {
            string input = FilePath.Text.Split('.').Last();
            SimpleGraph graph;
            switch (input)
            {
                case "txt":
                    graph = TextParser.Parse(FilePath.Text);
                    break;
                case "xml":
                    graph = XmlParser.Parse(FilePath.Text);
                    break;
                default:
                    graph = JsonParser.Parse(FilePath.Text);
                    break;
            }


        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Text file (*.txt*)|*.txt*";
            file.FileName = "MyGraph.txt";
            if (file.ShowDialog() == false)
                return;

            List<string> lines = new List<string>();
            g.Nodes.ForEach(n => lines.Add(n.Value.ToString()));
            g.Edges.ForEach(d => lines.Add($"{d.src.Value} {d.dest.Value}"));

            File.WriteAllLines(file.FileName, lines);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DrawingScreen.Children.Clear();
            g.Clear();
        }

        private void DrawingScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentButton == MoveButton && e.LeftButton == MouseButtonState.Pressed)
            {
                Point now = e.GetPosition(DrawingScreen);
                double ChangeX = now.X - PreMousePoint.X;
                double ChangeY = now.Y - PreMousePoint.Y;
                for (int i = 0; i < g.Nodes.Count; i++)
                {
                    g.Nodes[i].XCordinate += ChangeX;
                    g.Nodes[i].YCordinate += ChangeY;
                }
                for (int i = 0; i < g.Edges.Count; i++)
                {
                    g.Edges[i].UiEdge.Line.X1 += ChangeX;
                    g.Edges[i].UiEdge.Line.Y1 += ChangeY;
                    g.Edges[i].UiEdge.Line.X2 += ChangeX;
                    g.Edges[i].UiEdge.Line.Y2 += ChangeY;
                }
                PreMousePoint = now;
            }
        }
        public void EnableDrag(UIElement element)
        {
            element.MouseDown += Element_MouseDown;
            element.MouseUp += Element_MouseUp;
        }

        private void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            DragStart = null;
            element.ReleaseMouseCapture();
        }
        private void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            DragStart = e.GetPosition(element);
            element.CaptureMouse();
        }

    }
}
