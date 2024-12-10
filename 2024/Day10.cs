using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode2024
{
    public static class Day10
    {
        public static void Run()
        {
            var line = "dummy";
            var lines = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line.Length > 0)
                {
                    lines.Add(line);
                }
            }

            var map = new int[lines[0].Length, lines.Count];
            List<LavaNode> nodeList = new List<LavaNode>();
            var nodeArray = new LavaNode[lines[0].Length, lines.Count];
            for (var y = 0; y < lines.Count; y++)
            {
                var ca = lines[y].ToCharArray();
                for (var x = 0; x < ca.Length; x++)
                {
                    var val = Int32.Parse(ca[x].ToString());
                    map[x, y] = val;
                    var newNode = new LavaNode() { Pos = (x, y), Height = val };
                    nodeList.Add(newNode);
                    nodeArray[x, y] = newNode;
                }
            }

            (int, int)[] directions = { (0, 1), (0, -1), (1, 0), (-1, 0) };
            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[0].Length; x++)
                {
                    var a = nodeArray[x, y];

                    foreach (var d in directions)
                    {
                        try {
                            var neighborNode = nodeArray[x + d.Item1, y + d.Item2];
                            if (neighborNode.Height == a.Height + 1)
                            {
                                a.CanWalkTo.Add(neighborNode);
                                neighborNode.CanWalkFrom.Add(a);
                            }
                        }
                        catch { }
                    }
                }
            }

            // Sort and populate in order
            nodeList.Sort();
            foreach (var n in nodeList)
            {
                if (n.Height == 9)
                {
                    n.ReachableEnds.Add(n);
                    n.WaysFrom = 1;
                }

                foreach (var neighbor in n.CanWalkFrom)
                {
                    neighbor.ReachableEnds.UnionWith(n.ReachableEnds);
                    neighbor.WaysFrom = neighbor.WaysFrom + n.WaysFrom;
                }
            }

            // Calculate the score
            var waysSum = 0d;
            foreach (var n in nodeList)
            {
                if (n.Height == 0)
                {
                    Console.WriteLine($"({n.Pos.x},{n.Pos.y}) - {n.ReachableEnds.Count()} ways.");
                    Console.WriteLine($"({n.Pos.x},{n.Pos.y}) - {n.WaysFrom} ways.");
                    waysSum += n.WaysFrom;
                }
            }

            Console.WriteLine(waysSum);
        }

        public class LavaNode : IComparable<LavaNode>
        {
            public (int x, int y) Pos;
            public int Height;

            // Part 2 - number of paths
            public int WaysFrom = 0;

            public HashSet<LavaNode> ReachableEnds = new HashSet<LavaNode>();
            public List<LavaNode> CanWalkTo = new List<LavaNode>();
            public List<LavaNode> CanWalkFrom = new List<LavaNode>();

            public int CompareTo(LavaNode? other)
            {
                // Descending order - highest to lowest
                return -(this.Height - other.Height);
            }
        }
    }
}
