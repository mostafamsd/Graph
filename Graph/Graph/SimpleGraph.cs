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
    public class SimpleGraph
    {
        public List<Node> Nodes;
        public List<Edge> Edges;
        public SimpleGraph()
        {
            this.Nodes = new List<Node>();
            Edges = new List<Edge>();
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
    public class Edge
    {
        public Node src;
        public Node dest;
        public Brush Color;
        public long Length;
        public Point Center;
        public double M;
        public double B;
        public Edge(Node s,Node d)
        {
            this.src = s;
            this.dest = d;
            this.Color = Brushes.Black;
            this.M = (s.YCordinate - d.YCordinate) / (s.XCordinate - d.XCordinate);
            this.B = s.YCordinate - (M * s.XCordinate);
            this.Center.X = (s.XCordinate + d.XCordinate) / 2;
            this.Center.Y = (s.YCordinate + d.YCordinate) / 2;
        }
    }
    public class Node
    {
        public long Value;
        public List<Node> Adj;
        public int Radius;
        public double XCordinate;
        public double YCordinate;
        public Brush Color;
        
        public Node(long value)
        {
            this.Adj = new List<Node>();
            this.Value = value;
            this.Radius = 20;
            this.Color = Brushes.White;
        }
        public void AddNeighbor(Node neighbor)
        {
            Adj.Add(neighbor);
            neighbor.Adj.Add(this);
        }
    }
}
