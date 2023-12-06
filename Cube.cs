using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Cube
    {
        public static void Run()
        {
            string line = "dummy";
            var sum = 0;
            var gameNum = 0;
            while (line != "")
            {
                gameNum++;
                line = Console.ReadLine();
                if (line == "")
                {
                    break;
                }

                line = line.Substring(line.IndexOf(':') + 1);

                var games = line.Split(';');
                var minR = 0;
                var minG = 0;
                var minB = 0;
                foreach (var game in games)
                {

                    var balls = game.Split(',');
                    foreach (var b in balls)
                    {
                        var btrim = b.Trim();
                        var bts = btrim.Split(' ');
                        var count = int.Parse(bts[0]);
                        var col = bts[1];

                        switch (col)
                        {
                            case "blue":
                                if (minB < count) minB = count;
                                break;
                            case "red":
                                if (minR < count) minR = count;
                                break;
                            case "green":
                                if (minG < count) minG = count;
                                break;
                            default:
                                break;
                        }
                    }
                }

                sum += minB * minR * minG;
            }

            Console.WriteLine(sum);
        }
    }
}
