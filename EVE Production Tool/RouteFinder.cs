using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EVE_Production_Tool
{
    class RouteFinder
    {
        public class Node
        {
            public string ID { get; set; }
            public int Distance { get; set; }
            public string Parent { get; set; }

            public Node(string id, int d, string p)
            {
                ID = id;
                Distance = d;
                Parent = p;
            }
        }

        private string[] Graph { get; set; }
        public List<Node> Paths = new List<Node>();

        public RouteFinder(string file, string origin, int range)
        {
            Graph = System.IO.File.ReadAllLines(file);

            List<Node> nodes = new List<Node>();
            foreach (string line in System.IO.File.ReadAllLines("SystemLUTfile.txt"))
            {
                string id = line.Substring(0, 8);
                if (id == origin)
                {
                    Paths.Add(new Node(id, 0, null));
                }
                else
                {
                    nodes.Add(new Node(id, -1, null));
                }
            }

            for (int i = 0; i <= range; i++)
            {
                List<Node> newNodes = new List<Node>();
                foreach (Node c in Paths)
                {
                    foreach (string line in Graph)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0] == c.ID && nodes.Exists(n => n.ID == parts[1]))
                        {
                            Node temp = new Node(parts[1], i, c.ID);
                            newNodes.Add(temp);
                            nodes.Remove(nodes.Find(n => n.ID == parts[1]));
                        }
                    }
                }
                Paths.AddRange(newNodes);
            }
        }

        public int GetDistance(string target)
        {
            return Paths.Find(n => n.ID == target).Distance;
        }

        public List<string> GetRoute(string target)
        {
            List<string> route = new List<string> { target };
            Node c = Paths.Find(n => n.ID == target);
            while (c.Parent != null)
            {
                c = Paths.Find(n => n.ID == c.Parent);
                route.Add(c.ID);
            }
            return route;
        }

        public List<string> GetSystemsInRange(int range)
        {
            return Paths.FindAll(n => n.Distance <= range).ConvertAll(n => n.ID);
        }
    }
}
