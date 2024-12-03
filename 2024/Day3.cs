using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day3
    {
        public static void Run()
        {
            // Oops - the input gets split into multiple lines
            var k = "dummy";
            string line = "";
            while (k != "")
            {
                k = Console.ReadLine();
                line = line + k;
            }

            var sum = 0d;
            while (line.Contains("mul("))
            {
                // Find the next "don't"
                var deactivateIndex = line.IndexOf("don't()");
                var mulIndex = line.IndexOf("mul(");

                if (deactivateIndex > 0 && mulIndex > deactivateIndex)
                {
                    line = line.Substring(deactivateIndex + 6);
                    
                    if (line.Contains("do()"))
                    {
                        line = line.Substring(line.IndexOf("do()"));
                    }

                    continue;
                }

                line = line.Substring(mulIndex + 4);
                var nextComma = line.IndexOf(",");
                var nextClose = line.IndexOf(")");

                if (nextComma == -1 || nextClose == -1)
                {
                    break;
                }

                if (nextClose < nextComma)
                {
                    continue;
                }

                var firstNum = line.Substring(0, nextComma);
                var secondNum = line.Substring(nextComma + 1, nextClose - nextComma - 1);

                if (double.TryParse(firstNum, out var firstNumParsed)
                    && double.TryParse(secondNum, out var secondNumParsed))
                {
                    sum += firstNumParsed * secondNumParsed;
                    Console.WriteLine(firstNumParsed + " * " + secondNumParsed);
                }
            }

            Console.WriteLine(sum);
        }
    }
}
