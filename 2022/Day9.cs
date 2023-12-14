using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day9
    {
        public static void Run()
        {
            const int LENGTH = 10;
            var tailPoss = new (double, double)[LENGTH];
            
            for (var i = 0; i < LENGTH; i++)
            {
                tailPoss[i] = (0, 0);
            }

            var visitedPositions = new HashSet<(double, double)>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;
                var spl = line.Split(' ');
                var direction = spl[0];
                var distance = double.Parse(spl[1]);

                for (var i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            tailPoss[0] = (tailPoss[0].Item1, tailPoss[0].Item2 + 1);
                            break;
                        case "D":
                            tailPoss[0] = (tailPoss[0].Item1, tailPoss[0].Item2 - 1);
                            break;
                        case "L":
                            tailPoss[0] = (tailPoss[0].Item1 - 1, tailPoss[0].Item2);
                            break;
                        case "R":
                            tailPoss[0] = (tailPoss[0].Item1 + 1, tailPoss[0].Item2);
                            break;
                    }

                    for (var j = 1; j < LENGTH; j++)
                    {
                        tailPoss[j] = GetNewPos(tailPoss[j], tailPoss[j - 1]);
                    }

                    visitedPositions.Add((tailPoss[LENGTH - 1].Item1, tailPoss[LENGTH - 1].Item2));
                }
            }

            Console.WriteLine(visitedPositions.Count);
        }

        private static (double, double) GetNewPos((double, double) tailPos, (double, double) headPos)
        {
            var directionToMove = (headPos.Item1 - tailPos.Item1, headPos.Item2 - tailPos.Item2);

            if (directionToMove.Item1 == 0 && directionToMove.Item2 > 1) { return (tailPos.Item1, tailPos.Item2 + 1); }
            else if (directionToMove.Item1 == 0 && directionToMove.Item2 < -1) { return (tailPos.Item1, tailPos.Item2 - 1); }
            else if (directionToMove.Item1 > 1 && directionToMove.Item2 == 0) { return (tailPos.Item1 + 1, tailPos.Item2); }
            else if (directionToMove.Item1 < -1 && directionToMove.Item2 == 0) { return  (tailPos.Item1 - 1, tailPos.Item2); }

            else if (directionToMove.Item1 == 1 && directionToMove.Item2 > 1) { return (tailPos.Item1 + 1, tailPos.Item2 + 1); }
            else if (directionToMove.Item1 == 1 && directionToMove.Item2 < -1) { return (tailPos.Item1 + 1, tailPos.Item2 - 1); }
            else if (directionToMove.Item1 == -1 && directionToMove.Item2 > 1) { return (tailPos.Item1 - 1, tailPos.Item2 + 1); }
            else if (directionToMove.Item1 == -1 && directionToMove.Item2 < -1) { return (tailPos.Item1 - 1, tailPos.Item2 - 1); }

            else if (directionToMove.Item1 > 1 && directionToMove.Item2 == 1) { return (tailPos.Item1 + 1, tailPos.Item2 + 1); }
            else if (directionToMove.Item1 < -1 && directionToMove.Item2 == 1) { return (tailPos.Item1 - 1, tailPos.Item2 + 1); }
            else if (directionToMove.Item1 > 1 && directionToMove.Item2 == -1) { return (tailPos.Item1 + 1, tailPos.Item2 - 1); }
            else if (directionToMove.Item1 < -1 && directionToMove.Item2 == -1) { return (tailPos.Item1 - 1, tailPos.Item2 - 1); }
            return tailPos;
        }
    }
}
