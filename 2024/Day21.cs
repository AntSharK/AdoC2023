using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day21
    {
        public static void Run()
        {
            var total = 0l;
            while (true)
            {
                var k = Console.ReadLine();
                if (k == "") { break; }

                var secret = Int64.Parse(k);
                for (var i = 0; i < 2000; i++)
                {
                    secret = Iterate(secret);
                }

                Console.WriteLine($"{k}: {secret}");
                total += secret;
            }

            Console.WriteLine($"Total: {total}");
        }

        public static long Iterate(long secret)
        {
            var a1 = secret * 64;
            secret = Mix(a1, secret);
            secret = Prune(secret);
            var a2 = secret / 32;
            secret = Mix(a2, secret);
            secret = Prune(secret);
            var a3 = secret * 2048;
            secret = Mix(a3, secret);
            secret = Prune(secret);

            return secret;
        }

        public static long Mix(long val, long secret)
        {
            return secret ^ val;
        }

        public static long Prune(long secret)
        {
            return secret % 16777216;
        }
    }
}
