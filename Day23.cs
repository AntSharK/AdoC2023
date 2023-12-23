using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2023
{
    [DebuggerNonUserCode]
    internal class Day23
    {
        public static void Run()
        {
            var inputLines = new List<string>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }
                inputLines.Add(line);
            }

            (int, int) start = (0, 0);
            (int, int) end = (0, 0);
            var maze = new char[inputLines[0].Length, inputLines.Count];
            for(var x = 0; x < inputLines[0].Length; x++)
            {
                for (var y = 0; y < inputLines.Count; y++)
                {

                    // Part B: Treat all slopes as regular branches
                    if (inputLines[y][x] == '#')
                    {
                        maze[x, y] = '#';
                    }
                    else
                    {
                        maze[x, y] = '.';
                    }
                    //maze[x, y] = inputLines[y][x];

                    if (y == 0 && inputLines[y][x] == '.')
                    {
                        start = (x, y);
                    }

                    if (y == inputLines.Count - 1 && inputLines[y][x] == '.')
                    {
                        end = (x, y);
                    }

                }
            }

            // From looking at input, we can just branch
            var pathsToExplore = new List<Path>();
            var finishedPaths = new List<Path>();
            pathsToExplore.Add(new Path(start));
            while (pathsToExplore.Count > 0)
            {
                // Keep going down this path until we hit a fork or the end
                var currentPath = pathsToExplore[0];
                pathsToExplore.RemoveAt(0);

                var split = currentPath.ExploreTillFork(maze);     
                if (currentPath.PreviousTile == end)
                {
                    finishedPaths.Add(currentPath);
                    Console.WriteLine($"COUNT: {finishedPaths.Count}");

                    if (finishedPaths.Count > 4000) { break; }
                    // This gives 3115, which is too low
                }

                if (split != null)
                {
                    foreach (var newTile in split)
                    {
                        var newPath = new Path(newTile);
                        newPath.Encountered = newPath.Encountered.Union(currentPath.Encountered).ToHashSet();
                        pathsToExplore.Add(newPath);
                        //Console.WriteLine($"Path at ({currentPath.PreviousTile.Item1},{currentPath.PreviousTile.Item2}) splits to ({newTile.Item1},{newTile.Item2})");
                    }
                }
            }

            // The start isn't counted - so minus 1 from this answer: 2111-1 = 2110
            Console.WriteLine(finishedPaths.Max(p => p.Encountered.Count));
        }

        public class Node
        {
            public (int, int) position;
            public (int, Node) left;
            public (int, Node) right;
            public (int, Node) up;
            public (int, Node) down;

            public override int GetHashCode()
            {
                return position.Item1 + position.Item2 * 150;
            }

            public override bool Equals(object? obj)
            {
                var o = obj as Node;
                return position == o.position;
            }
        }

        public class Path
        {
            public HashSet<(int, int)> Encountered = new HashSet<(int, int)>();
            public (int, int) PreviousTile;
            static (int, int)[] directions = new (int, int)[4] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            public Path((int, int) pathStart)
            {
                Encountered.Add(pathStart);
                PreviousTile = pathStart;
            }

            public List<(int, int)> ExploreTillFork(char[,] maze) // Returns the next tiles to explore on the fork
            {
                var tileAtCurrent = maze[PreviousTile.Item1, PreviousTile.Item2];
                if (tileAtCurrent == '.')
                {
                    var possibleDirs = new List<(int, int)>();
                    foreach (var direction in directions)
                    {
                        try
                        {
                            var nextTile = maze[PreviousTile.Item1 + direction.Item1, PreviousTile.Item2 + direction.Item2];
                            if (nextTile != '#'
                                && !Encountered.Contains((PreviousTile.Item1 + direction.Item1, PreviousTile.Item2 + direction.Item2)))
                            {
                                // Additional condition: Can't go against slopes
                                if (nextTile == '<' && direction != (-1, 0)) { }
                                else if (nextTile == '>' && direction != (1, 0)) { }
                                else if (nextTile == '^' && direction != (0, -1)) { }
                                else if (nextTile == 'v' && direction != (0, 1)) { }
                                else
                                {
                                    possibleDirs.Add(direction);
                                }
                            }
                        }
                        catch { }
                    }

                    if (possibleDirs.Count == 0)
                    {
                        return null;
                    }

                    if (possibleDirs.Count > 1)
                    {
                        var retVal = new List<(int, int)>();
                        foreach (var direction in possibleDirs)
                        {
                            retVal.Add((PreviousTile.Item1 + direction.Item1, PreviousTile.Item2 + direction.Item2));
                        }

                        return retVal;
                    }

                    var locationToExplore = (PreviousTile.Item1 + possibleDirs[0].Item1, PreviousTile.Item2 + possibleDirs[0].Item2);
                    this.PreviousTile = locationToExplore;
                    this.Encountered.Add(locationToExplore);
                    return ExploreTillFork(maze);
                }
                else
                {
                    (int, int) locationToExplore = (0, 0);
                    switch (tileAtCurrent)
                    {
                        case '>':
                            locationToExplore = (PreviousTile.Item1 + 1, PreviousTile.Item2);
                            break;
                        case '<':
                            locationToExplore = (PreviousTile.Item1 - 1, PreviousTile.Item2);
                            break;
                        case 'v':
                            locationToExplore = (PreviousTile.Item1, PreviousTile.Item2 + 1);
                            break;
                        case '^':
                            locationToExplore = (PreviousTile.Item1, PreviousTile.Item2 - 1);
                            break;
                    }

                    this.PreviousTile = locationToExplore;
                    this.Encountered.Add(locationToExplore);
                    return ExploreTillFork(maze);
                }

                throw new InvalidOperationException();
            }
        }
    }
}
