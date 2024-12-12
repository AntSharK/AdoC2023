using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Day12
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

            var map = new char[lines[0].Length, lines.Count];
            for(var y = 0; y < lines.Count; y++)
            {
                var i = lines[y].ToCharArray();
                for(var x = 0; x < i.Length; x++)
                {
                    map[x, y] = i[x];
                }
            }

            var regions = new List<Region>();
            var pointsEncountered = new HashSet<(int x, int y)>();
            var directions = new List<(int x, int y)>()
            {
                (0, 1), (1, 0), (0, -1), (-1, 0)
            };

            for (var y = 0; y < lines.Count; y++)
            {
                for (var x = 0; x < lines[0].Length; x++)
                {
                    if (!pointsEncountered.Contains((x,y)))
                    {
                        var region = new Region()
                        {
                            Plant = map[x, y]
                        };

                        pointsEncountered.Add((x, y));
                        var pointsToEncounter = new List<(int x, int y)>();
                        pointsToEncounter.Add((x, y));

                        while (pointsToEncounter.Count > 0)
                        {
                            var point = pointsToEncounter.First();
                            pointsToEncounter.RemoveAt(0);
                            region.Points.Add((point.x, point.y));

                            foreach (var d in directions)
                            {
                                var newPoint = (point.x + d.x, point.y + d.y);
                                try
                                {
                                    var c = map[newPoint.Item1, newPoint.Item2];
                                    if (!pointsEncountered.Contains(newPoint)
                                        && c == region.Plant)
                                    {
                                        pointsToEncounter.Add(newPoint);
                                        pointsEncountered.Add(newPoint);
                                    }
                                }
                                // Out of bounds, do nothing
                                catch { }
                            }
                        }

                        regions.Add(region);
                    }
                }
            }

            var ts = 0;
            foreach (var r in regions)
            {
                ts += r.Score();
                Console.WriteLine(r);
            }

            Console.WriteLine(ts);
        }

        public class Region
        {
            public HashSet<(int x, int y)> Points = new();
            public char Plant;

            public override string ToString()
            {
                var k = this.Points.Select((p) => $"({p.x},{p.y})");
                return Plant + ":" + string.Join(';', k) + $"Area:{Points.Count}, Perimeter:{this.Perimeter()}, Faces:{this.Faces()}.";
            }

            public int Score()
            {
                return Points.Count * this.Faces();
            }

            public int Faces()
            {
                // A face ON TOP, from left to right
                var topFaces = new Dictionary<(int startx, int starty), (int endx, int endy)>();

                // A face on the RIGHT, from top to bottom
                var rightFaces = new Dictionary<(int startx, int starty), (int endx, int endy)>();

                var corners = new HashSet<(int x, int y)>();

                foreach (var p in Points)
                {
                    var hasTop = false;
                    var hasBottom = false;
                    var hasLeft = false;
                    var hasRight = false;

                    if (!Points.Contains((p.x, p.y - 1)))
                    {
                        if (!topFaces.ContainsKey((p.x, p.y)))
                        {
                            topFaces[(p.x, p.y)] = (p.x + 1, p.y); // Add a face above
                            hasTop = true;
                        }
                    }

                    if (!Points.Contains((p.x, p.y + 1)))
                    {
                        if (!topFaces.ContainsKey((p.x, p.y + 1)))
                        {
                            topFaces[(p.x, p.y + 1)] = (p.x + 1, p.y + 1); // Add a face below
                            hasBottom = true;
                        }
                    }

                    if (!Points.Contains((p.x - 1, p.y)))
                    {
                        if (!rightFaces.ContainsKey((p.x, p.y)))
                        {
                            rightFaces[(p.x, p.y)] = (p.x, p.y + 1); // Add a face on the left
                            hasLeft = true;
                        }
                    }

                    if (!Points.Contains((p.x + 1, p.y)))
                    {
                        if (!rightFaces.ContainsKey((p.x + 1, p.y)))
                        {
                            rightFaces[(p.x + 1, p.y)] = (p.x + 1, p.y + 1); // Add a face on the right
                            hasRight = true;
                        }
                    }

                    if (hasTop && hasLeft)
                    {
                        corners.Add((p.x, p.y));
                    }
                    if (hasTop && hasRight)
                    {
                        corners.Add((p.x + 1, p.y));
                    }
                    if (hasBottom && hasLeft)
                    {
                        corners.Add((p.x, p.y + 1));
                    }
                    if (hasBottom && hasRight)
                    {
                        corners.Add((p.x + 1, p.y + 1));
                    }
                }

                // Start joining faces
                while (true)
                {
                    try
                    {
                        var thingToJoin = topFaces.First(b =>
                            !corners.Contains(b.Value) && // Don't combine with corners
                            topFaces.ContainsKey(b.Value));

                        topFaces[thingToJoin.Key] = topFaces[thingToJoin.Value];
                        topFaces.Remove(thingToJoin.Value);
                    }
                    catch { break; }
                }

                while (true)
                {
                    try
                    {
                        var thingToJoin = rightFaces.First(b =>
                            !corners.Contains(b.Value) &&   
                            rightFaces.ContainsKey(b.Value));

                        rightFaces[thingToJoin.Key] = rightFaces[thingToJoin.Value];
                        rightFaces.Remove(thingToJoin.Value);
                    }
                    catch { break; }
                }

                return topFaces.Count + rightFaces.Count;
            }

            public int Perimeter()
            {
                var perimeter = 0;
                foreach (var p in Points)
                {
                    var k = Points.Count((n) => 
                        (n.x, n.y) == (p.x + 1, p.y) ||
                        (n.x, n.y) == (p.x, p.y + 1) ||
                        (n.x, n.y) == (p.x - 1, p.y) ||
                        (n.x, n.y) == (p.x, p.y - 1));
                    perimeter = perimeter + 4 - k;
                }

                return perimeter;
            }
        }
    }
}
