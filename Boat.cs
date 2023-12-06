using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Boat
    {
        public static void Run()
        {
            //var times = new List<int>() { 61, 70, 90, 66 };
            //var distances = new List<int>() { 643, 1184, 1362, 1041 };

            //var times = new List<int>() { 7, 15, 30 };
            //var distances = new List<int>() { 9, 40, 200 };

            var times = new List<double>() { 61709066 };
            var distances = new List<double>() { 643118413621041 };


            var numWays = 1d;
            for (var i = 0; i < times.Count; i++)
            {
                var time = times[i];
                var distance = distances[i];

                for (int x = 1; x < time/2; x++)
                {
                    var totalTravelled = x * (time - x);
                    if (totalTravelled > distance)
                    {
                        var numberOfWays = time - x - x + 1;
                        numWays = numWays * numberOfWays;
                        Console.WriteLine(numberOfWays);
                        break;
                    }
                }
            }

            Console.WriteLine(numWays);
        }
    }
}
