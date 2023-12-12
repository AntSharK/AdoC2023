using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023._2022
{
    internal class Day25
    {
        public static void Run()
        {
            var s = 0l;
            while (true)
            {
                var l = Console.ReadLine();
                if (l == "") break;

                var d = SnafuToDecimal(l);
                Console.WriteLine("Decimal=" + d);
                s += d;
            }

            Console.WriteLine("Sum=" + s);
            var ds = DecimalToSnafuReversed(s).Reverse();
            foreach (var c in ds) { Console.Write(c); }
        }

        private static string DecimalToSnafuReversed(long d)
        {
            if (d == 0) return "";

            var mod5 = d % 5;
            switch (mod5)
            {
                case 0:
                    return "0" + DecimalToSnafuReversed(d / 5);
                case 1:
                    return "1" + DecimalToSnafuReversed((d - 1) / 5);
                case 2:
                    return "2" + DecimalToSnafuReversed((d - 2) / 5);
                case 3:
                    return "=" + DecimalToSnafuReversed((d + 2) / 5);
                case 4:
                    return "-" + DecimalToSnafuReversed((d + 1) / 5);
            }

            return null;
        }

        private static long SnafuToDecimal(string s)
        {
            var d = 0l;
            var idx = 1l;
            foreach (var c in s.Reverse())
            {
                switch (c)
                {
                    case '1':
                        d += idx;
                        break;
                    case '2':
                        d += 2 * idx;
                        break;
                    case '0':
                        break;
                    case '-':
                        d -= idx;
                        break;
                    case '=':
                        d -= 2 * idx;
                        break;
                }

                idx = idx * 5;
            }

            return d;
        }
    }
}
