using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    [DebuggerNonUserCode]
    // The only time we re-implement because 23b requires a wholly different approach
    public class Day23b
    {
        public static (int, int) START = (0, 0);
        public static (int, int) END = (0, 0);
        public static List<Node> NODEPOINTS = new List<Node>();
        public static (int, int)[] DIRECTIONS = new (int, int)[4] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        public static bool[,] MAZE;

        public static void Run()
        {
            var inputLines = new List<string>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }
                inputLines.Add(line);
            }

            MAZE = new bool[inputLines[0].Length, inputLines.Count];
            for (var x = 0; x < inputLines[0].Length; x++)
            {
                for (var y = 0; y < inputLines.Count; y++)
                {
                    // Part B: Treat all slopes as regular branches
                    if (inputLines[y][x] == '#')
                    {
                        MAZE[x, y] = false;
                    }
                    else
                    {
                        MAZE[x, y] = true;
                    }
                    //maze[x, y] = inputLines[y][x];

                    if (y == 0 && inputLines[y][x] == '.')
                    {
                        START = (x, y);
                    }

                    if (y == inputLines.Count - 1 && inputLines[y][x] == '.')
                    {
                        END = (x, y);
                    }

                }
            }

            // Find each point which can branch
            for (var x = 0; x < inputLines[0].Length; x++)
            {
                for (var y = 0; y < inputLines.Count; y++)
                {
                    if (MAZE[x, y])
                    {
                        var directionsAllowed = 0;
                        foreach (var direction in DIRECTIONS)
                        {
                            try
                            {
                                if (MAZE[x + direction.Item1, y + direction.Item2]) { directionsAllowed++; }
                            }
                            catch { }
                        }

                        if (directionsAllowed >= 3)
                        {
                            NODEPOINTS.Add(new Node() { position = (x, y) });
                        }
                    }
                }
            }

            // Convert the maze into a graph
            var startNode = new Node() { position = START };
            var endNode = new Node() { position = END };
            NODEPOINTS.Add(startNode);
            NODEPOINTS.Add(endNode);

            var nodesTraversed = new HashSet<Node>() { };
            var nodesToTraverse = new List<Node>() { startNode };

            nodesTraversed.Add(startNode);
            while (nodesToTraverse.Count > 0)
            {
                var n = nodesToTraverse[0];
                nodesToTraverse.RemoveAt(0);

                n.findNeighbors();
                foreach (var neighbor in n.neighbors.Values)
                {
                    if (!nodesTraversed.Contains(neighbor))
                    {
                        nodesTraversed.Add(neighbor);
                        nodesToTraverse.Add(neighbor);
                    }
                }
            }

            // Exhaustively search all paths
            List<Path> successfulPaths = new List<Path>();
            Stack<Path> pathsToExplore = new Stack<Path>();
            pathsToExplore.Push(new Path(startNode));

            var maxLength = 0;
            while (pathsToExplore.Count > 0)
            {
                var currentPath = pathsToExplore.Pop();
                var lastNode = currentPath.Encountered.Last();
                foreach (var n in lastNode.neighbors)
                {
                    var direction = n.Key;
                    var nextNode = n.Value;

                    if (!currentPath.Encountered.Contains(nextNode))
                    {
                        var newPath = new Path(nextNode);
                        newPath.Encountered = new HashSet<Node>().Union(currentPath.Encountered).ToHashSet();
                        newPath.Directions = currentPath.Directions.GetRange(0, currentPath.Directions.Count);

                        newPath.Directions.Add(direction);
                        newPath.Encountered.Add(nextNode);

                        if (nextNode == endNode)
                        {
                            newPath.EvalLength();
                            successfulPaths.Add(newPath);

                            if (newPath.Length > maxLength)
                            {
                                maxLength = newPath.Length;
                                Console.WriteLine($"NEW LENGTH: {maxLength} AT: {successfulPaths.Count}");
                            }
                        }
                        else
                        {
                            pathsToExplore.Push(newPath);
                        }
                    }
                }
            }

            // Terminates at 6514, 1110467 paths tried
            Console.WriteLine(successfulPaths.Max(p => p.Length));
        }

        public class Path
        {
            public HashSet<Node> Encountered = new HashSet<Node>();
            public List<(int, int)> Directions = new List<(int, int)>();
            public int Length;
            public Path(Node startNode)
            {
                Encountered.Add(startNode);
            }

            public void EvalLength()
            {
                // Always start from the start point
                var n = NODEPOINTS.First(n => n.position == START);
                var totalDist = 0;
                foreach (var d in Directions)
                {
                    totalDist += n.distance[d];
                    n = n.neighbors[d];
                }

                Length = totalDist;
            }
        }

    public class Node
        {
            public (int, int) position;
            public Dictionary<(int, int), Node> neighbors = new Dictionary<(int, int), Node>();
            public Dictionary<(int, int), int> distance = new Dictionary<(int, int), int>();

            public override int GetHashCode()
            {
                return position.Item1 + position.Item2 * 150;
            }

            public override bool Equals(object? obj)
            {
                var o = obj as Node;
                return position == o.position;
            }

            public override string ToString()
            {
                return position.Item1 + "," + position.Item2;
            }

            public void findNeighbors()
            {
                foreach (var direction in DIRECTIONS)
                {
                    try
                    {
                        var nextPosition = (position.Item1 + direction.Item1, position.Item2 + direction.Item2);
                        if (MAZE[nextPosition.Item1, nextPosition.Item2])
                        {
                            (neighbors[direction], distance[direction]) = findNextNode(nextPosition, position, 0);
                        }
                    }
                    catch { }
                }
            }

            private (Node, int) findNextNode((int, int) currentPosition, (int, int) lastPosition, int distance)
            {
                var findNode = NODEPOINTS.FirstOrDefault(c => c.position == currentPosition);
                if (findNode != null)
                {
                    return (findNode, distance + 1);
                }

                foreach (var direction in DIRECTIONS)
                {
                    try
                    {
                        var nextPosition = (currentPosition.Item1 + direction.Item1, currentPosition.Item2 + direction.Item2);
                        if (nextPosition != lastPosition
                            && MAZE[nextPosition.Item1, nextPosition.Item2])
                        {
                            return findNextNode(nextPosition, currentPosition, distance + 1);
                        }
                    }
                    catch { }
                }

                throw new InvalidOperationException();
            }
        }
    }
}
