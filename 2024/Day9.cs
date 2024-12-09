using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day9
    {
        public static void Run()
        {
            var line = "dummy";
            var input = "";
            while (line != "")
            {
                line = Console.ReadLine();
                input = input + line;
            }

            var fileNum = 0;
            var fsList = new List<int>();

            // Part 2, store metadata
            var files = new List<(int start, int length, int fileNum)>();
            var space = new List<(int start, int length)>();

            var currentSpot = 0;
            for (var i = 0; i < input.Length; i = i + 2)
            {
                var num = Int32.Parse(input[i].ToString());
                var blanks = 0;
                try
                {
                    blanks = Int32.Parse(input[i + 1].ToString());
                    space.Add((currentSpot + num, blanks));
                }
                // out of bounds
                catch { }

                files.Add((currentSpot, num, fileNum));

                for (var x = 0; x < num; x++)
                {
                    fsList.Add(fileNum);
                    currentSpot++;
                }

                for (var x = 0; x < blanks; x++)
                {
                    // -1 denotes a blank
                    fsList.Add(-1);
                    currentSpot++;
                }

                fileNum++;
            }

            var fs = fsList.ToArray();

            // VS Console has issues with very long single lines, so split input into multiple lines
            // We could read from a file but... Oh well
            //Console.WriteLine(String.Join(' ', fs.Select(x => x.ToString())));
            //Console.WriteLine(string.Join(' ', files.Select(x => x.fileNum + ":" + x.start + "-" + x.length)));
            //Console.WriteLine(string.Join(' ', space.Select(x => x.start + "-" + x.length)));

            for (var i = files.Count - 1; i > -1; i--)
            {
                //Console.WriteLine(String.Join(' ', fs.Select(x => x.ToString())));
                var f = files[i];

                // Find a space for this
                var spaceFound = -1;
                for (var j = 0; j < space.Count; j++)
                {
                    var s = space[j];
                    if (s.start > f.start)
                    {
                        // File cannot be moved
                        break;
                    }

                    if (s.length >= f.length)
                    {
                        spaceFound = j;
                        break;
                    }
                }

                // File cannot be moved
                if (spaceFound == -1)
                {
                    continue;
                }

                for (var x = 0; x < f.length; x++)
                {
                    fs[space[spaceFound].start + x] = f.fileNum;
                    fs[f.start + x] = -1;
                }

                space[spaceFound] = (space[spaceFound].start + f.length, space[spaceFound].length - f.length);
                if (space[spaceFound].length == 0)
                {
                    space.RemoveAt(spaceFound);
                }
            }

            Console.WriteLine("Received Input of Length: " + input.Length + " For FS of: " + fs.Length);

            /* Part 1 compacting
            var leftPoint = 0;
            var rightPoint = fs.Length - 1;

            while (leftPoint <= rightPoint)
            {
                if (fs[leftPoint] >= 0)
                {
                    leftPoint++;
                    continue;
                }

                if (fs[rightPoint] <= 0)
                {
                    rightPoint--;
                    continue;
                }

                fs[leftPoint] = fs[rightPoint];
                fs[rightPoint] = -1;
                rightPoint--;
            }
            */

            // Output the FS
            //Console.WriteLine(String.Join(' ', fs.Select(x => x.ToString())));

            // Write the checksum
            var checkSum = 0d;
            for (var i = 0; i < fs.Length; i++)
            {
                if (fs[i] < 0)
                {
                    continue;
                }

                checkSum += fs[i] * i;
            }

            Console.WriteLine(checkSum);
        }
    }
}
