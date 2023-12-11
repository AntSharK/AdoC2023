using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day15
    {
        public static void Run()
        {
            var line = "";
            var xyd = new List<(double, double, double)>();
            var beaconLocations = new List<(double, double)>();
            while (true)
            {
                line = Console.ReadLine();
                if (!line.StartsWith("Sensor")) { break; }

                var sp = line.Split(':');
                var sensorPortion = sp[0];
                var beaconPortion = sp[1];
                var sx = double.Parse(sensorPortion.Substring(sensorPortion.IndexOf("x=") + 2, sensorPortion.IndexOf("y=") - sensorPortion.IndexOf("x=") - 4));
                var sy = double.Parse(sensorPortion.Substring(sensorPortion.IndexOf("y=") + 2));

                var bx = double.Parse(beaconPortion.Substring(beaconPortion.IndexOf("x=") + 2, beaconPortion.IndexOf("y=") - beaconPortion.IndexOf("x=") - 4));
                var by = double.Parse(beaconPortion.Substring(beaconPortion.IndexOf("y=") + 2));

                var distance = Math.Abs(sx - bx) + Math.Abs(sy - by);
                xyd.Add((sx, sy, distance));
                beaconLocations.Add((bx, by));
            }

            var rowNum = 2000000;
            var rangeExclude = RangeExclude(rowNum, xyd);

            var excludedNumbers = new HashSet<double>();
            foreach (var range in rangeExclude)
            {
                for (var i = range.Item1; i <= range.Item2; i++)
                {
                    excludedNumbers.Add(i);
                }
            }

            foreach (var b in beaconLocations)
            {
                if (b.Item2 == rowNum)
                {
                    if (excludedNumbers.Contains(b.Item1))
                    {
                        excludedNumbers.Remove(b.Item1);
                    }
                }
            }

            Console.WriteLine(excludedNumbers.Count);
        }

        private static List<(double, double)> RangeExclude(double y, List<(double, double, double)> xyds)
        {
            var retVal = new List<(double, double)>();
            foreach (var xyd in xyds)
            {
                var yDistLeft = xyd.Item3 - Math.Abs(y - xyd.Item2);
                if (yDistLeft >= 0)
                {
                    retVal.Add((xyd.Item1 - yDistLeft, xyd.Item1 + yDistLeft));
                }
            }

            return retVal;
        }
    }
}
