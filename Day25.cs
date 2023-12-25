using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day25
    {
        public static void Run()
        {
            var graph = new Dictionary<string, Node>();
            var intGraph = new Dictionary<int, Node>();
            var connections = new List<(string, string)>();
            var connectionUsed = new Dictionary<(string, string), int>();
            var rng = new Random();

            var nodeNumber = 0;
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }

                var nodeName = line.Split(':')[0];
                var neighbors = line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (!graph.ContainsKey(nodeName))
                {
                    graph[nodeName] = new Node() { Name = nodeName, Number = nodeNumber++, };
                    intGraph[nodeNumber] = graph[nodeName];
                }

                foreach (var neighbor in neighbors)
                {
                    if (!graph.ContainsKey(neighbor))
                    {
                        graph[neighbor] = new Node() { Name = neighbor, Number = nodeNumber++, };
                        intGraph[nodeNumber] = graph[neighbor];
                    }
                }

                foreach (var neighbor in neighbors)
                {
                    var node = graph[nodeName];
                    var neighborNode = graph[neighbor];
                    node.Neighbors.Add(neighborNode);
                    neighborNode.Neighbors.Add(node);
                    connections.Add((nodeName, neighbor));
                    connectionUsed[(nodeName, neighbor)] = 0;
                }
            }

            Console.WriteLine($"Graph has {graph.Count()} nodes."); // 1527 nodes.
            Console.WriteLine($"Graph has {connections.Count()} removable connections."); // 3419 nodes. This means N^3 is ~12 billion, which is do-able
            /*
            for (var x = 0; x < 10000; x++) // Pick 10000 random paths
            {
                var n1 = graph.Values.ElementAt(rng.Next(graph.Values.Count() - 1));
                var n2 = graph.Values.ElementAt(rng.Next(graph.Values.Count() - 1));

                if (n1 == n2) continue;

                // Find the shortest path from n1 to n2
                var pathNodes = n1.FindPath(n2);

                for (var i = 0; i < pathNodes.Count - 1; i++)
                {
                    var c1 = pathNodes[i];
                    var c2 = pathNodes[i + 1];
                    var conn = (c1.Name, c2.Name);
                    if (!connectionUsed.ContainsKey(conn))
                    {
                        conn = (c2.Name, c1.Name);
                    }

                    connectionUsed[conn]++;
                }
            }

            var connectionsUsedTuple = new List<(int, string, string)>();
            foreach (var c in connectionUsed)
            {
                connectionsUsedTuple.Add((c.Value, c.Key.Item1, c.Key.Item2));
            }

            //connectionsUsedTuple.Sort((a, b) => a.Item1 - b.Item1);
            connectionsUsedTuple.Sort();

            foreach (var i in connectionsUsedTuple)
            {
                Console.WriteLine($"Connection {i.Item2}/{i.Item3}: {i.Item1}.");
            }
            */
            /*
Connection pbx/lsq: 558.
Connection njx/hkn: 564.
Connection krf/pzr: 605.
Connection njx/htv: 606.
Connection qpj/pbx: 647.
Connection zfk/pbx: 652.
Connection zvk/sxx: 1217.
Connection sss/pzr: 1445.
Connection njx/pbx: 2291.
             */

            // Brute force does not work
            var c1 = ("zvk", "sxx");
            var c2 = ("sss", "pzr");
            var c3 = ("njx", "pbx");
            var encountered = new HashSet<string>();
            var nodesToTraverse = new Queue<Node>();

            var startNode = graph.Values.First();
            encountered.Add(startNode.Name);
            nodesToTraverse.Enqueue(startNode);
            while (nodesToTraverse.Count > 0) 
            {
                var nodeToExplore = nodesToTraverse.Dequeue();

                foreach (var neighbor in nodeToExplore.Neighbors)
                {
                    if (!encountered.Contains(neighbor.Name)
                        && c1 != (neighbor.Name, nodeToExplore.Name)
                        && c1 != (nodeToExplore.Name, neighbor.Name)
                        && c2 != (neighbor.Name, nodeToExplore.Name)
                        && c2 != (nodeToExplore.Name, neighbor.Name)
                        && c3 != (neighbor.Name, nodeToExplore.Name)
                        && c3 != (nodeToExplore.Name, neighbor.Name))
                    {
                        nodesToTraverse.Enqueue(neighbor);
                        encountered.Add(neighbor.Name);
                    }
                }
            }

            var firstComponentSize = encountered.Count;

            // Copy and paste of above lines
            startNode = graph.Values.First(n => !encountered.Contains(n.Name));
            encountered.Add(startNode.Name);
            nodesToTraverse.Enqueue(startNode);
            while (nodesToTraverse.Count > 0)
            {
                var nodeToExplore = nodesToTraverse.Dequeue();

                foreach (var neighbor in nodeToExplore.Neighbors)
                {
                    if (!encountered.Contains(neighbor.Name)
                        && c1 != (neighbor.Name, nodeToExplore.Name)
                        && c1 != (nodeToExplore.Name, neighbor.Name)
                        && c2 != (neighbor.Name, nodeToExplore.Name)
                        && c2 != (nodeToExplore.Name, neighbor.Name)
                        && c3 != (neighbor.Name, nodeToExplore.Name)
                        && c3 != (nodeToExplore.Name, neighbor.Name))
                    {
                        nodesToTraverse.Enqueue(neighbor);
                        encountered.Add(neighbor.Name);
                    }
                }
            }

            var secondComponentSize = encountered.Count - firstComponentSize;
            Console.WriteLine($"{c1.Item1}/{c1.Item2}, {c2.Item1}/{c2.Item2}, {c3.Item1}/{c3.Item2} gives components {firstComponentSize} and {secondComponentSize}.");
            Console.WriteLine(firstComponentSize * secondComponentSize);
        }

        public class Node
        {
            public HashSet<Node> Neighbors = new HashSet<Node>();
            public string Name;
            public int Number;

            public override string ToString()
            {
                return Name;
            }

            public List<Node> FindPath(Node n2)
            {
                var encountered = new HashSet<Node>();
                var pathQ = new Queue<List<Node>>();
                var nodeQ = new Queue<Node>();

                encountered.Add(this);
                nodeQ.Enqueue(this);
                pathQ.Enqueue(new List<Node>());

                var currentNode = this;

                while (nodeQ.Count > 0)
                {
                    currentNode = nodeQ.Dequeue();
                    var currentPath = pathQ.Dequeue();

                    if (currentNode == n2) { return currentPath; }

                    foreach (var neighbor in currentNode.Neighbors)
                    {
                        if (!encountered.Contains(neighbor))
                        {
                            encountered.Add(neighbor);
                            nodeQ.Enqueue(neighbor);
                            var newPath = new List<Node>();
                            newPath.AddRange(currentPath);
                            newPath.Add(neighbor);
                            pathQ.Enqueue(newPath);
                        }
                    }
                }

                throw new InvalidOperationException();
            }
        }

    }
}
