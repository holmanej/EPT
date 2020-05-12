using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EVE_Production_Tool
{
    class RouteFinder
    {
        public class Node
        {
            public int ID { get; set; }
            public int Distance { get; set; }
            public int Parent { get; set; }

            public Node(int id, int d, int p)
            {
                ID = id;
                Distance = d;
                Parent = p;
            }
        }

        List<Node> Nodes = new List<Node>();

        public RouteFinder(int origin, double security)
        {
            Dictionary<int, List<int>> Graph = new Dictionary<int, List<int>>();
            List<Node> newNodes = new List<Node>();

            foreach (string line in System.IO.File.ReadAllLines("NewJumpMap.txt"))
            {
                string[] parts = line.Split(',');
                if (double.Parse(parts[1]) >= security)
                {
                    int system = int.Parse(parts[0]);
                    List<int> gates = new List<int>();
                    for (int i = 2; i < parts.Count(); i++)
                    {
                        gates.Add(int.Parse(parts[i]));
                    }
                    Graph.Add(system, gates);

                    if (system == origin)
                    {
                        Nodes.Add(new Node(system, 0, -1));
                        foreach (int gate in gates)
                        {
                            newNodes.Add(new Node(gate, 1, system));
                        }
                    }
                }
            }
            int dist = 2;
            for (int i = 0; i <= Graph.Count;)
            {
                //Debug.WriteLine(i + "/" + Graph.Count);
                List<Node> temp = new List<Node>();
                foreach (KeyValuePair<int, List<int>> entry in Graph)
                {
                    foreach (Node n in newNodes)
                    {
                        if (n.ID == entry.Key)
                        {
                            foreach (int gate in entry.Value)
                            {
                                temp.Add(new Node(gate, dist, n.ID));
                            }
                            Nodes.Add(n);
                            entry.Value.Clear();
                            i++;
                        }
                    }
                }
                newNodes.Clear();
                newNodes.AddRange(temp);
                dist++;
            }
        }

        public int GetDistance(int target)
        {
            foreach (Node n in Nodes)
            {
                if (n.ID == target)
                {
                    return n.Distance;
                }
            }
            return -1;
        }

        public List<int> GetRoute(int target)
        {
            List<int> route = new List<int> { target };
            Node c = Nodes.Find(n => n.ID == target);
            while (c.Parent != -1)
            {
                c = Nodes.Find(n => n.ID == c.Parent);
                route.Add(c.ID);
            }
            return route;
        }

        public List<int> GetSystemsInRange(int range)
        {
            return Nodes.FindAll(n => n.Distance <= range).ConvertAll(n => n.ID);
        }
    }
}
