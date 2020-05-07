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

            public Node(string id, int d, string p) : this()
            {
                ID = id;
                Distance = d;
                Parent = p;
            }

            public Node()
            {
            }
        }

        private string[] Graph { get; set; }

        public RouteFinder(string file)
        {
            Graph = System.IO.File.ReadAllLines(file);
        }

        public int GetDistance(string origin, string target)
        {
            List<Node> nodes = new List<Node>();
            List<Node> visited = new List<Node>();
            foreach (string line in System.IO.File.ReadAllLines("SystemLUTfile.txt"))
            {
                string id = line.Substring(0, 8);
                if (id == origin)
                {
                    visited.Add(new Node(id, 0, null));
                    Console.WriteLine("current: " + id + "  target: " + target);
                }
                else
                {
                    nodes.Add(new Node(id, -1, null));
                }
            }

            for (int i = 0; nodes.Count > 0; i++)
            {
                List<Node> newNodes = new List<Node>();
                if (visited.Exists(n => n.ID == target))
                {
                    return i;
                }
                foreach (Node c in visited)
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
                visited.AddRange(newNodes);
            }
            return -1;
        }

        public List<Node> GetSystemsInRange(string origin, int range)
        {
            List<Node> nodes = new List<Node>();
            List<Node> visited = new List<Node>();
            foreach (string line in System.IO.File.ReadAllLines("SystemLUTfile.txt"))
            {
                string id = line.Substring(0, 8);
                if (id == origin)
                {
                    visited.Add(new Node(id, 0, null));
                    Console.WriteLine("current: " + id);
                }
                else
                {
                    nodes.Add(new Node(id, -1, null));
                }
            }

            for (int i = 0; i <= range; i++)
            {
                Console.WriteLine("i: " + i + "  cnt: " + nodes.Count);
                List<Node> newNodes = new List<Node>();
                foreach (Node c in visited)
                {
                    foreach (string line in Graph)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0] == c.ID && nodes.Exists(n => n.ID == parts[1]))
                        {
                            Node temp = new Node(parts[1], i, c.ID);
                            newNodes.Add(temp);
                            nodes.Remove(nodes.Find(n => n.ID == parts[1]));
                            Console.WriteLine("System: " + temp.ID + "  D: " + temp.Distance);
                        }
                    }
                }
                visited.AddRange(newNodes);
            }
            return visited;
        }
    }
}
