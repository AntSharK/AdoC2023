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
            var bananasFromSequence = new Dictionary<(int, int, int, int), int>();

            var count = 0;
            while (true)
            {
                var k = Console.ReadLine();
                if (k == "") { break; }

                var secret = Int64.Parse(k);

                var prices = new int[2001];
                var priceChanges = new int[2000];
                prices[0] = GetBananas(secret);

                var sequenceSeen = new HashSet<(int, int, int, int)>();
                for (var i = 1; i <= 2000; i++)
                {
                    secret = Iterate(secret);

                    // You get this many bananas by buying at this time
                    var price = GetBananas(secret);
                    prices[i] = price;

                    var delta = price - prices[i - 1];
                    priceChanges[i - 1] = delta;

                    if (i > 3)
                    {
                        var sequence = (priceChanges[i - 4], priceChanges[i - 3], priceChanges[i - 2], priceChanges[i - 1]);
                        if (!sequenceSeen.Contains(sequence))
                        {
                            if (!bananasFromSequence.ContainsKey(sequence))
                            {
                                bananasFromSequence[sequence] = prices[i];
                            }
                            else
                            {
                                bananasFromSequence[sequence] += prices[i];
                            }

                            sequenceSeen.Add(sequence);
                        }
                    }
                }

                //var currentCount = GetBananasFromSequence(prices, priceChanges, -1,0,-1,2);
                //count += currentCount;
                //Console.WriteLine($"Sequence adds: {currentCount} to {count}");
            }

            // 2277 is too high (from 1, 1, -2, -2)
            // 2259 is also too high
            var sortedBananaSequence = bananasFromSequence.OrderByDescending(c => c.Value);
            Console.WriteLine($"Max Value: {sortedBananaSequence.First().Value} From: {sortedBananaSequence.First().Key}");
        }

        public static int GetBananasFromSequence(int[] prices, int[] priceChanges, int a, int b, int c, int d)
        {
            for (var i = 3; i < 2000; i++)
            {
                if (priceChanges[i - 3] == a
                    && priceChanges[i - 2] == b
                    && priceChanges[i - 1] == c
                    && priceChanges[i] == d)
                {
                    return prices[i + 1];
                }
            }

            return 0;
        }

        public static int GetBananas(long secret)
        {
            return (int)(secret % 10);
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
