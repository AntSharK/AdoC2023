using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day16
    {
        public static void Run()
        {
            var lines = new List<string>();

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;

                lines.Add(line);
            }

            var maze = new char[lines[0].Length, lines.Count];
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    maze[j, i] = line[j];
                }
            }

            var maxTop = 0;
            for (var i = 0; i < lines[0].Length; i++)
            {
                var startBeam = (i, -1, 0, 1);
                var e = FindEnergizedTiles(maze, startBeam);
                if (e > maxTop)
                {
                    maxTop = e;
                }
            }
            Console.WriteLine($"TOP: {maxTop}");

            var maxLeft = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                var startBeam = (-1, i, 1, 0);
                var e = FindEnergizedTiles(maze, startBeam);
                if (e > maxLeft)
                {
                    maxLeft = e;
                }
            }
            Console.WriteLine($"LEFT: {maxLeft}");

            var maxRight = 0;
            var rightEdge = lines[0].Length;
            for (var i = 0; i < lines.Count; i++)
            {
                var startBeam = (rightEdge, i, -1, 0);
                var e = FindEnergizedTiles(maze, startBeam);
                if (e > maxRight)
                {
                    maxRight = e;
                }
            }
            Console.WriteLine($"RIGHT: {maxRight}");

            var maxBottom = 0;
            var bottomEdge = lines.Count();
            for (var i = 0; i < lines[0].Length; i++)
            {
                var startBeam = (i, bottomEdge, 0, -1);
                var e = FindEnergizedTiles(maze, startBeam);
                if (e > maxBottom)
                {
                    maxBottom = e;
                }
            }
            Console.WriteLine($"BOTTOM: {maxBottom}");
        }

        private static int FindEnergizedTiles(char[,] maze, (int, int, int, int) startBeam)
        {
            // X, Y, XDir, YDir
            HashSet<(int, int, int, int)> beams = new HashSet<(int, int, int, int)>();
            beams.Add(startBeam);

            var beamsStillTravelling = new List<(int, int, int, int)>();
            beamsStillTravelling.Add(startBeam);

            while (beamsStillTravelling.Count > 0)
            {
                var nextBeam = beamsStillTravelling[0];
                beamsStillTravelling.RemoveAt(0);

                try
                {
                    // Advance beam in direction of movement, then change direction
                    nextBeam.Item1 += nextBeam.Item3;
                    nextBeam.Item2 += nextBeam.Item4;
                    var c = maze[nextBeam.Item1, nextBeam.Item2];

                    var xDir = nextBeam.Item3;
                    var yDir = nextBeam.Item4;
                    switch (c)
                    {
                        case '\\':
                            // (0, 1) => (1, 0), (1, 0) => (0, 1) || (0, -1) => (-1, 0), (-1, 0) => (0, -1)
                            nextBeam.Item3 = yDir;
                            nextBeam.Item4 = xDir;
                            break;
                        case '/':
                            // (0, 1) => (-1, 0), (-1, 0) => (0, 1) || (0, -1) => (1, 0), (1, 0) => (0, -1)
                            nextBeam.Item3 = -yDir;
                            nextBeam.Item4 = -xDir;
                            break;
                        case '|':
                            if (xDir == 1 || xDir == -1)
                            {
                                nextBeam.Item3 = 0;
                                nextBeam.Item4 = 1;

                                // Spawn a new beam
                                var splitBeam = (nextBeam.Item1, nextBeam.Item2, 0, -1);
                                if (!beams.Contains(splitBeam))
                                {
                                    beams.Add(splitBeam);
                                    beamsStillTravelling.Add(splitBeam);
                                }
                            }
                            break;
                        case '-':
                            if (yDir == 1 || yDir == -1)
                            {
                                nextBeam.Item3 = 1;
                                nextBeam.Item4 = 0;

                                // Spawn a new beam
                                var splitBeam = (nextBeam.Item1, nextBeam.Item2, -1, 0);
                                if (!beams.Contains(splitBeam))
                                {
                                    beams.Add(splitBeam);
                                    beamsStillTravelling.Add(splitBeam);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if (!beams.Contains(nextBeam))
                    {
                        beams.Add(nextBeam);
                        beamsStillTravelling.Add(nextBeam);
                    }
                }
                // If out of bounds, abort
                catch
                {
                }
            }

            var coords = new HashSet<(int, int)>();
            foreach (var beam in beams)
            {
                coords.Add((beam.Item1, beam.Item2));
            }

            // Minus 1 to discount the start
            Console.WriteLine($"Start at ({startBeam.Item1}, {startBeam.Item2}) energizes {(coords.Count - 1)} tiles.");
            return (coords.Count - 1);
        }


        private class Beam
        {
            public int XPos;
            public int YPos;
            public int XDir;
            public int YDir;
        }

    }
}
