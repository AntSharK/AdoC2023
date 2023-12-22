using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day22
    {
        public static void Run()
        {
            var bricks = new List<Brick>();
            var n = 1;

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;

                var coords = line.Split('~');
                var b1 = coords[0].Split(',');
                var b2 = coords[1].Split(",");
                bricks.Add(new Brick()
                {
                    start = (int.Parse(b1[0]), int.Parse(b1[1]), int.Parse(b1[2])),
                    end = (int.Parse(b2[0]), int.Parse(b2[1]), int.Parse(b2[2])),
                    number = n,
                });

                n++;
            }

            // Find the range
            var xMax = bricks.Max(b => Math.Max(b.end.Item1, b.start.Item1));
            var yMax = bricks.Max(b => Math.Max(b.end.Item2, b.start.Item2));
            var zMax = bricks.Max(b => Math.Max(b.end.Item3, b.start.Item3));

            Console.WriteLine($"{xMax} - {yMax} - {zMax}");

            // Sort bricks by start, and then DROP THEM
            bricks.Sort((a, b) => a.start.Item3 - b.start.Item3);

            var filledSpace = new Brick[xMax + 1, yMax + 1, zMax + 1];
            foreach (var brick in bricks)
            {
                brick.Drop(filledSpace);
            }

            var redundantBricks = 0;
            foreach (var brick in bricks)
            {
                brick.isRedundant = true;
                if (brick.bricksAbove.Count == 0)
                {
                    brick.isRedundant = true;
                }
                else
                {
                    foreach (var brickAbove in brick.bricksAbove)
                    {
                        if (brickAbove.bricksBelow.Count <= 1)
                        {
                            brick.isRedundant = false;
                            break;
                        }
                    }
                }
            }

            // Part B: sort bricks again
            bricks.Sort((a, b) => a.start.Item3 - b.start.Item3);

            foreach (var brick in bricks)
            {
                if (!brick.isRedundant)
                {
                    Console.WriteLine($"Brick {brick} is not redundant.");
                    redundantBricks++;

                    // Part B - Just simulate falling bricks - only 1288-434 simulations
                    // Drop every brick again, except the current brick
                    var newFilledSpace = new Brick[xMax + 1, yMax + 1, zMax + 1];
                    foreach (var b in bricks)
                    {
                        if (b != brick)
                        {
                            var originalStart = b.start;
                            var originalEnd = b.end;
                            b.Drop(newFilledSpace);

                            if (b.start != originalStart) { brick.fallingBricks++; }

                            b.start = originalStart;
                            b.end = originalEnd;
                        }
                    }
                }
            }

            Console.WriteLine(bricks.Sum(b => b.fallingBricks));

            // 785 is too high
            // 614 is too high
        }

        private class Brick
        {
            public (int, int, int) start;
            public (int, int, int) end;
            public int number;
            public HashSet<Brick> bricksAbove = new HashSet<Brick>();
            public HashSet<Brick> bricksBelow = new HashSet<Brick>();
            public bool isRedundant;
            public int fallingBricks = 0;

            public void Drop(Brick[,,] filledSpace)
            {
                // It's a block - so just make everything increasing from start to end
                if (start.Item3 > end.Item3)
                {
                    var lower = end.Item3;
                    end.Item3 = start.Item3;
                    start.Item3 = lower;
                }
                if (start.Item2 > end.Item2)
                {
                    var lower = end.Item2;
                    end.Item2 = start.Item2;
                    start.Item2 = lower;
                }
                if (start.Item1 > end.Item1)
                {
                    var lower = end.Item1;
                    end.Item1 = start.Item1;
                    start.Item1 = lower;
                }

                var stopDownwardMotion = false;
                while (!stopDownwardMotion)
                {
                    if (start.Item3 - 1 <= 0)
                    {
                        break;
                    }

                    for (var x = start.Item1; x<= end.Item1; x++)
                    {
                        for (var y = start.Item2; y <= end.Item2; y++)
                        {
                            var brickSpot = filledSpace[x, y, start.Item3 - 1];
                            if (brickSpot != null)
                            {
                                stopDownwardMotion = true;
                                this.bricksBelow.Add(brickSpot);
                                brickSpot.bricksAbove.Add(this);
                            }
                        }
                    }

                    if (!stopDownwardMotion)
                    {
                        start.Item3--;
                        end.Item3--;
                    }
                }

                // Fill in the spots
                for (var x = start.Item1; x <= end.Item1; x++)
                {
                    for (var y = start.Item2; y <= end.Item2; y++)
                    {
                        for (var z = start.Item3; z <= end.Item3; z++)
                        {
                            filledSpace[x, y, z] = this;
                        }
                    }
                }
            }

            public override string ToString()
            {
                return number.ToString();
            }
        }
    }
}
