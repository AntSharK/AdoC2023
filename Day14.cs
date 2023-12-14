using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2023
{
    internal class Day14
    {
        public static void Run()
        {
            var rocks = new List<char[]>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;
                rocks.Add(line.ToCharArray());
            }

            List<string> encounteredRocks = new List<string>();
            var s = SerializeRocks(rocks);
            encounteredRocks.Add(s);

            var totalCount = 1000000000;
            for (var i = 1; i <= totalCount; i++)
            {
                TiltNorth(rocks);
                TiltLeft(rocks);
                TiltSouth(rocks);
                TiltRight(rocks);

                var serialize = SerializeRocks(rocks);
                Console.WriteLine($"Score {i} == {CalculateScore(rocks)}");

                if (!encounteredRocks.Contains(serialize))
                {
                    encounteredRocks.Add(serialize);
                }
                else
                {
                    var idx = encounteredRocks.IndexOf(serialize);

                    Console.WriteLine($"{i} == {idx}");
                    var cycleLength = (i - idx);
                    var cyLeft = totalCount - i;
                    i = i + (cyLeft / cycleLength) * cycleLength;
                }
            }

            CalculateScore(rocks);
        }

        private static string SerializeRocks(List<char[]> rocks)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < rocks.Count; y++)
            {
                for (var x = 0; x < rocks[0].Length; x++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        sb.Append($"[{y}.{x}]");
                    }
                }
            }

            return sb.ToString();
        }

        private static double CalculateScore(List<char[]> rocks)
        {
            var score = 0d;
            for (var y = 0; y < rocks.Count; y++)
            {
                for (var x = 0; x < rocks[0].Length; x++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        score += (rocks.Count - y);
                    }
                    //Console.Write(rocks[y][x]);
                }
                //Console.WriteLine();
            }

            //Console.WriteLine(score);
            return score;
        }

        private static void TiltNorth(List<char[]> rocks)
        {
            for (var y = 1; y < rocks.Count; y++)
            {
                for (var x = 0; x < rocks[0].Length; x++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        var newY = y - 1;
                        if (rocks[newY][x] != '.') continue;

                        while (newY > 0
                            && rocks[newY - 1][x] == '.')
                        {
                            newY--;
                        }

                        rocks[y][x] = '.';
                        rocks[newY][x] = 'O';
                    }
                }
            }
        }

        private static void TiltSouth(List<char[]> rocks)
        {
            for (var y = rocks.Count - 2; y >= 0; y--)
            {
                for (var x = 0; x < rocks[0].Length; x++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        var newY = y + 1;
                        if (rocks[newY][x] != '.') continue;

                        while (newY < rocks.Count - 1
                            && rocks[newY + 1][x] == '.')
                        {
                            newY++;
                        }

                        rocks[y][x] = '.';
                        rocks[newY][x] = 'O';
                    }
                }
            }
        }

        private static void TiltLeft(List<char[]> rocks)
        {
            for (var x = 1; x < rocks[0].Length; x++)
            {
                for (var y = 0; y < rocks.Count; y++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        var newX = x - 1;
                        if (rocks[y][newX] != '.') continue;

                        while (newX > 0
                            && rocks[y][newX - 1] == '.')
                        {
                            newX--;
                        }

                        rocks[y][x] = '.';
                        rocks[y][newX] = 'O';
                    }
                }
            }
        }

        private static void TiltRight(List<char[]> rocks)
        {
            for (var x = rocks[0].Length - 2; x >= 0; x--)
            {
                for (var y = 0; y < rocks.Count; y++)
                {
                    if (rocks[y][x] == 'O')
                    {
                        var newX = x + 1;
                        if (rocks[y][newX] != '.') continue;

                        while (newX < rocks[0].Length - 1
                            && rocks[y][newX + 1] == '.')
                        {
                            newX++;
                        }

                        rocks[y][x] = '.';
                        rocks[y][newX] = 'O';
                    }
                }
            }
        }
    }
}
