using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class TextParser
    {
        public static SimpleGraph Parse(string path)
        {
            string [] content = File.ReadAllLines(path);
            SimpleGraph graph = new SimpleGraph();
            foreach(var c in content)
            {
                var tok = c.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tok.Length == 1)
                {
                    graph.AddNode(new Node(long.Parse(tok[0]),new Point(0,0)));
                }
                else
                {
                    graph.AddEdge(graph.Find(long.Parse(tok[0])), graph.Find(long.Parse(tok[1])));
                }
            }
            return graph;
        }
    }
}
