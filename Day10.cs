using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdventOfCode2023
{
    internal class Day10
    {
        public static void Run()
        {
            var line = "dummy";
            var input = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }

                input.Add(line.Trim());
            }

            var width = input[0].Length;
            var height = input.Count;

            var maze = new Node[width, height];
            var cy = 0;
            foreach (var l in input)
            {
                var cx = 0;
                foreach (var c in l)
                {
                    var n = new Node();
                    n.Symbol = c;
                    maze[cx, cy] = n;
                    n.FriendlyName = cx + "," + cy;
                    n.x = cx;
                    n.y = cy;
                    cx++;
                }
                cy++;
            }

            var start = (0, 0);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var n = maze[x, y];

                    try
                    {
                        switch (n.Symbol)
                        {
                            case '|':
                                n.Connections.Add(maze[x, y - 1]);
                                n.Connections.Add(maze[x, y + 1]);
                                break;
                            case '-':
                                n.Connections.Add(maze[x + 1, y]);
                                n.Connections.Add(maze[x - 1, y]);
                                break;
                            case 'L':
                                n.Connections.Add(maze[x + 1, y]);
                                n.Connections.Add(maze[x, y - 1]);
                                break;
                            case 'J':
                                n.Connections.Add(maze[x - 1, y]);
                                n.Connections.Add(maze[x, y - 1]);
                                break;
                            case '7':
                                n.Connections.Add(maze[x - 1, y]);
                                n.Connections.Add(maze[x, y + 1]);
                                break;
                            case 'F':
                                n.Connections.Add(maze[x + 1, y]);
                                n.Connections.Add(maze[x, y + 1]);
                                break;
                            case 'S':
                                start = (x, y);
                                break;
                            case '.':
                            default:
                                break;
                        }
                    }
                    catch // Swallow index out of bounds exceptions lolol
                    {

                    }
                }
            }

            var startCandidates = new List<Node>();
            var directions = new List<(int, int)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };
            var validDirections = new HashSet<(int, int)>();
            foreach (var d in directions)
            {
                try
                {
                    startCandidates.Add(maze[start.Item1 + d.Item1, start.Item2 + d.Item2]);
                    validDirections.Add(d);
                }
                catch { }
            }

            var filteredStartCandidates = new List<Node>();
            foreach (var sc in startCandidates)
            {
                if (sc.Connections.Contains(maze[start.Item1, start.Item2]))
                {
                    filteredStartCandidates.Add(sc);
                }
            }

            var loopTiles = new List<Node>();
            foreach (var sc in filteredStartCandidates)
            {
                var nodesSoFar = new List<Node>();
                sc.Connections.Remove(maze[start.Item1, start.Item2]);

                (var loopLength, var encounteredNodes) = sc.FindLoopToStart();
                if (loopLength > 0)
                {
                    // Have to divide this by 2
                    Console.WriteLine("Loop length " + loopLength);
                    Console.WriteLine(encounteredNodes);

                    encounteredNodes = encounteredNodes + ":" + sc.FriendlyName;

                    // Re-constitute the enclosure and perform flood fill
                    var parseThis = encounteredNodes.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in parseThis)
                    {
                        var pt = p.Split(',');
                        var px = pt[0];
                        var py = pt[1];
                        loopTiles.Add(maze[int.Parse(px), int.Parse(py)]);
                    }

                    // Re-color the maze! All loop tiles are marked X and non-loop-tiles are O.
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
                        {
                            var n = maze[x, y];
                            if (loopTiles.Contains(n))
                            {
                                n.Color = -2;
                            }
                            else
                            {
                                n.Color = 0;
                            }
                        }
                    }

                    break;
                }

                sc.Connections.Add(maze[start.Item1, start.Item2]);
            }

            // Copy polygon area from stackoverflow
            var polyArea = 0;
            loopTiles.Add(loopTiles.First());
            for (var i = 1; i < loopTiles.Count - 1; i = i + 1)
            {
                polyArea += loopTiles[i].y * (loopTiles[i - 1].x - loopTiles[i + 1].x);
            }

            polyArea = Math.Abs(polyArea / 2);
            Console.WriteLine("Polygon area: " + polyArea);
            var solution = polyArea - loopTiles.Count / 2 + 1;
            Console.WriteLine("Solution: " + solution);

            /*
            // Replace the start tile
            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1 - 1, start.Item2]) && filteredStartCandidates.Contains(maze[start.Item1 + 1, start.Item2]))
                    maze[start.Item1, start.Item2].Symbol = '-';
            }
            catch { }

            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1 - 1, start.Item2]) && filteredStartCandidates.Contains(maze[start.Item1, start.Item2 + 1]))
                    maze[start.Item1, start.Item2].Symbol = '7';
            }
            catch { }

            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1 - 1, start.Item2]) && filteredStartCandidates.Contains(maze[start.Item1, start.Item2 - 1]))
                    maze[start.Item1, start.Item2].Symbol = 'J';
            }
            catch { }

            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1, start.Item2 - 1]) && filteredStartCandidates.Contains(maze[start.Item1, start.Item2 + 1]))
                    maze[start.Item1, start.Item2].Symbol = 'L';
            }
            catch { }

            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1, start.Item2 - 1]) && filteredStartCandidates.Contains(maze[start.Item1, start.Item2 + 1]))
                    maze[start.Item1, start.Item2].Symbol = '|';
            }
            catch { }

            try
            {
                if (filteredStartCandidates.Contains(maze[start.Item1 + 1, start.Item2]) && filteredStartCandidates.Contains(maze[start.Item1, start.Item2 + 1]))
                    maze[start.Item1, start.Item2].Symbol = 'F';
            }
            catch { }

            Console.WriteLine($"Starting symbol is: {maze[start.Item1, start.Item2].Symbol}");
            Console.WriteLine($"SIZE: X:{width}, Y:{height}");

            // And because I didn't read the instructions carefully, draw a map of "in between pipes"
            for (var y = 0; y < height - 1; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var current = maze[x, y];
                    var below = maze[x, y + 1];
                    if ((current.Symbol == '-' || current.Symbol == 'L' || current.Symbol == 'J') &&
                        (below.Symbol == 'F' || below.Symbol == '-' || below.Symbol == '7'))
                    {
                        current.PassthroughSymbol = '-';
                    }
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width - 1; x++)
                {
                    var current = maze[x, y];
                    var right = maze[x + 1, y];
                    if ((current.Symbol == '|' || current.Symbol == 'J' || current.Symbol == '7') &&
                        (right.Symbol == 'F' || right.Symbol == '|' || right.Symbol == 'L'))
                    {
                        current.PassthroughSymbol = '|';

                        if (y != 0 && maze[x, y-1].PassthroughSymbol == '-')
                        {
                            maze[x, y - 1].PassthroughSymbol = '7';
                        }

                        if (y != 0 && maze[x + 1, y - 1].PassthroughSymbol == '-')
                        {
                            maze[x, y - 1].PassthroughSymbol = 'F';
                        }

                        if (y < height-2 && maze[x, y + 1].PassthroughSymbol == '-')
                        {
                            maze[x, y + 1].PassthroughSymbol = 'L';
                        }

                        if (y < height-2 && maze[x + 1, y + 1].PassthroughSymbol == '-')
                        {
                            maze[x, y + 1].PassthroughSymbol = 'J';
                        }
                    }
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var n = maze[x, y];
                    Console.Write(n.PassthroughSymbol);
                }

                Console.WriteLine();
            }

            //Flood fill
            var colorMaps = new Dictionary<int, HashSet<Node>>();
            var directionsPlusDiagonals = new List<(int, int)> { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var n = maze[x, y];
                    if (n.Color == 0)
                    {
                        n.Color = Node.FloodFillColor;
                        Node.FloodFillColor++;

                        Console.WriteLine("Flood filling color: " + n.Color);

                        var nodesToFill = new List<Node>() { n };
                        var encounteredNodes = new HashSet<Node>() { n };
                        while (nodesToFill.Count > 0)
                        {
                            var nodeToFill = nodesToFill[0];
                            nodesToFill.RemoveAt(0);

                            foreach (var d in directionsPlusDiagonals)
                            {
                                try
                                {
                                    var nextNode = maze[nodeToFill.x + d.Item1, nodeToFill.y + d.Item2];
                                    if (!encounteredNodes.Contains(nextNode))
                                    {
                                        if (nextNode.Color == 0)
                                        {
                                            encounteredNodes.Add(nextNode);
                                            nodesToFill.Add(nextNode);
                                            nextNode.Color = n.Color;
                                        }

                                        // Follow pipes to connect sets
                                        if (nodeToFill.PassthroughSymbol == '.'
                                            && (nextNode.PassthroughSymbol == '-' || nextNode.PassthroughSymbol == '|'))
                                        {
                                            if ((nextNode.PassthroughSymbol == '-' && (d == (1, 0) || d == (-1, 0)))
                                                || (nextNode.PassthroughSymbol == '|' && (d == (0, 1) || d == (0, -1))))
                                            {
                                                var ptNode = nextNode;
                                                var currDirection = d;
                                                while (ptNode.PassthroughSymbol != '.')
                                                {
                                                    encounteredNodes.Add(ptNode);
                                                    switch (ptNode.PassthroughSymbol)
                                                    {
                                                        case '|':
                                                            ptNode = currDirection == (0, 1) ? maze[ptNode.x, ptNode.y + 1] : maze[ptNode.x, ptNode.y - 1];
                                                            break;
                                                        case '-':
                                                            ptNode = currDirection == (1, 0) ? maze[ptNode.x + 1, ptNode.y] : maze[ptNode.x - 1, ptNode.y];
                                                            break;
                                                        case 'F':
                                                            ptNode = currDirection == (-1, 0) ? maze[ptNode.x, ptNode.y + 1] : maze[ptNode.x + 1, ptNode.y];
                                                            currDirection = currDirection == (-1, 0) ? (0, 1) : (1, 0);
                                                            break;
                                                        case 'J':
                                                            ptNode = currDirection == (1, 0) ? maze[ptNode.x, ptNode.y - 1] : maze[ptNode.x - 1, ptNode.y];
                                                            currDirection = currDirection == (1, 0) ? (0, -1) : (-1, 0);
                                                            break;
                                                        case '7':
                                                            ptNode = currDirection == (1, 0) ? maze[ptNode.x, ptNode.y + 1] : maze[ptNode.x - 1, ptNode.y];
                                                            currDirection = currDirection == (1, 0) ? (0, 1) : (-1, 0);
                                                            break;
                                                        case 'L':
                                                            ptNode = currDirection == (-1, 0) ? maze[ptNode.x, ptNode.y - 1] : maze[ptNode.x + 1, ptNode.y];
                                                            currDirection = currDirection == (-1, 0) ? (0, 1) : (1, 0);
                                                            break;
                                                    }
                                                }

                                                encounteredNodes.Add(ptNode);
                                                nodesToFill.Add(ptNode);
                                                ptNode.Color = n.Color;
                                            }
                                        }
                                    }
                                }
                                catch { } // Catch index out of bounds
                            }
                        }
                    }
                }
            }
            */

            // Check every set - if it is not on the boundary and next to a loop, it is enclosed
            var membersInSet = new Dictionary<int, int>();
            var setsOnBoundary = new HashSet<int>();
            var setsEnclosedByLoop = new HashSet<int>();

            for (var x = 0; x < width; x++)
            {
                var left = maze[x, 0];
                var right = maze[x, height - 1];
                if (left.Color > 0) { setsOnBoundary.Add(left.Color); }
                if (right.Color > 0) { setsOnBoundary.Add(right.Color); }
            }
            for (var y = 0; y < height; y++)
            {
                var top = maze[0, y];
                var bottom = maze[width - 1, y];
                if (top.Color > 0) { setsOnBoundary.Add(top.Color); }
                if (bottom.Color > 0) { setsOnBoundary.Add(bottom.Color); }
            }

            for (var x = 1; x < width - 1; x++)
            {
                for (var y = 1; y < height - 1; y++)
                {
                    if (maze[x, y].Color > 0 && !setsOnBoundary.Contains(maze[x, y].Color))
                    {
                        foreach (var d in directions)
                        {
                            if (maze[x + d.Item1, y + d.Item2].Color == -2)
                            {
                                setsEnclosedByLoop.Add(maze[x, y].Color);
                            }
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.Write("Boundary: ");
            foreach (var c in setsOnBoundary) { Console.Write(c + ","); }

            Console.WriteLine();
            Console.Write("Enclosed: ");
            foreach (var c in setsEnclosedByLoop) { Console.Write(c + ","); }

            Console.WriteLine();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var n = maze[x, y];
                    //Console.Write(n.Color + "\t");
                    if (!membersInSet.ContainsKey(n.Color))
                    {
                        membersInSet[n.Color] = 0;
                    }

                    membersInSet[n.Color]++;
                }

                Console.WriteLine();
            }

            var sum = 0;
            foreach (var enclosedSet in setsEnclosedByLoop)
            {
                Console.WriteLine($"Set {enclosedSet} has {membersInSet[enclosedSet]} members.");
                sum = sum + membersInSet[enclosedSet];
            }

            Console.WriteLine(sum);
        }

        private class Node
        {
            public char Symbol;
            public char PassthroughSymbol = '.';
            public List<Node> Connections = new List<Node>();
            public string FriendlyName;
            public int x;
            public int y;
            public int Color;
            public static int FloodFillColor = 1;

            public override string ToString()
            {
                return FriendlyName;
            }

            public override int GetHashCode()
            {
                return x * 10000 + y;
            }

            public override bool Equals(object? obj)
            {
                var k = obj as Node;
                return k.x == this.x && k.y == this.y;
            }

            public (int, string) FindLoopToStart()
            {
                // A proper DFS
                var nodesToTraverse = new List<Node>();
                var distanceSoFar = new List<int>();
                var pathToNode = new List<string>();
                HashSet<Node> encounteredNodes = new HashSet<Node>();

                // Don't need to handle loops of 1
                foreach (var node in this.Connections)
                {
                    nodesToTraverse.Add(node);
                    distanceSoFar.Add(2);
                    pathToNode.Add(node.FriendlyName);
                    encounteredNodes.Add(node);
                }

                while (nodesToTraverse.Count > 0)
                {
                    var nodeToTraverse = nodesToTraverse[0];
                    nodesToTraverse.RemoveAt(0);
                    var distanceToNode = distanceSoFar[0];
                    distanceSoFar.RemoveAt(0);
                    var pathSoFar = pathToNode[0];
                    pathToNode.RemoveAt(0);

                    if (nodeToTraverse.Symbol == 'S')
                    {
                        return (distanceToNode, pathSoFar);
                    }

                    foreach (var node in nodeToTraverse.Connections)
                    {
                        if (!encounteredNodes.Contains(node))
                        {
                            nodesToTraverse.Add(node);
                            distanceSoFar.Add(distanceToNode + 1);
                            encounteredNodes.Add(node);
                            pathToNode.Add(pathSoFar + ":" + node.FriendlyName);
                        }
                    }
                }

                return (-1, "");
            }
        }
    }

}
