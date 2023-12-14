using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day4
    {
        public static void Run()
        {
            var output = 0;
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;

                var inp = line.Split(',');
                var p1 = inp[0].Split('-').Select(x => int.Parse(x)).ToArray();
                var p2 = inp[1].Split('-').Select(x => int.Parse(x)).ToArray();

                var isSandwich = false;
                if (p1[0] >= p2[0] && p1[1] <= p2[1])
                { 
                    isSandwich = true;
                }
                else if (p1[0] <= p2[0] && p1[1] >= p2[1])
                { 
                    isSandwich = true; 
                }
                // Part 2
                else if (p1[0] >= p2[0] && p1[0] <= p2[1])
                {
                    isSandwich = true;
                }
                else if (p1[1] >= p2[0] && p1[1] <= p2[1])
                {
                    isSandwich = true;
                }

                if (isSandwich)
                {
                    output++;
                }
            }

            Console.WriteLine(output);
        }
    }
}
