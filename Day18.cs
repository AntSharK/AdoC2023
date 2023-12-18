using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day18
    {
        public static void Run()
        {
            var inputs = new List<(string, int, string)>();

            var directions = new Dictionary<string, (int, int)>()
            {
                { "R", (1, 0) },
                { "D", (0, 1) },
                { "L", (-1, 0) },
                { "U", (0, -1) },
            };

            var partbDirections = new Dictionary<char, (int, int)>()
            {
                { '0', (1, 0) },
                { '1', (0, 1) },
                { '2', (-1, 0) },
                { '3', (0, -1) },
            };

            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") { break; }

                var ls = line.Split(' ');
                var direction = ls[0];
                var num = int.Parse(ls[1]);
                var color = ls[2];
                inputs.Add((direction, num, color));
            }

            var points = new List<(long, long)>();
            var currentPoint = (0l, 0l);
            var perimeter = 0l;
            points.Add(currentPoint);
            foreach (var input in inputs)
            {
                // Part B: Ignore the first 2 parts, just focus on "color"
                var realInput = input.Item3.Substring(2, 6);
                var currentDirection = partbDirections[realInput[5]];
                var magnitude = Convert.ToUInt32(realInput.Substring(0, 5), 16);
                var newPoint = (currentPoint.Item1 + currentDirection.Item1 * magnitude, currentPoint.Item2 + currentDirection.Item2 * magnitude);

                //var currentDirection = directions[input.Item1];
                //var newPoint = (currentPoint.Item1 + currentDirection.Item1 * input.Item2, currentPoint.Item2 + currentDirection.Item2 * input.Item2);
                points.Add(newPoint);
                currentPoint = newPoint;
                perimeter += magnitude;
            }

            var innerArea = ShoelaceAlgorithm(points);
            Console.WriteLine($"{innerArea} - {perimeter}");
            Console.WriteLine($"Area Covered: {innerArea + perimeter / 2 + 1}");
        }

        public static long ShoelaceAlgorithm(List<(long, long)> tiles)
        {
            var polyArea = 0l;
            tiles.Add(tiles[0]);
            for (var i = 1; i < tiles.Count - 1; i = i + 1)
            {
                polyArea += tiles[i].Item2 * (tiles[i - 1].Item1 - tiles[i + 1].Item1);
            }

            return Math.Abs(polyArea / 2l);
        }
    }
}
