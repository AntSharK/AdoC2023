using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Camel
    {
        public static void Run()
        {
            List<Hand> hands = new List<Hand>();
            var line = "dummy";
            while (line != "")
            {
                line = Console.ReadLine();

                if (line == "")
                {
                    break;
                }

                var ls = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var currentHand = ls[0];
                var bid = ls[1];

                var h = new Hand();
                h.OriginalString = currentHand;
                h.Bid = double.Parse(bid);
                var rankCounter = 15 * 15 * 15 * 15 * 15;
                foreach (var c in currentHand)
                {
                    var digit = -1;
                    if (char.IsDigit(c))
                    {
                        digit = int.Parse(c.ToString());
                    }
                    else
                    {
                        switch (c)
                        {
                            case 'T':
                                digit = 10;
                                break;
                            case 'J':
                                digit = 0;
                                break;
                            case 'Q':
                                digit = 12;
                                break;
                            case 'K':
                                digit = 13;
                                break;
                            case 'A':
                                digit = 14;
                                break;
                        }
                    }
                    h.Cards[digit]++;
                    h.Ranking += digit * rankCounter;
                    rankCounter = rankCounter / 15;
                }

                hands.Add(h);
            }

            // J is now parsed as 0
            foreach (var hand in hands)
            {
                foreach (var c in hand.Cards)
                {
                    if (c.Value > 0
                        && c.Key != 0) // Exclude jokers from count
                    {
                        hand.Num[c.Value]++;
                    }
                }
            }

            foreach (var hand in hands)
            {
                if (hand.Num[5] >= 1)
                {
                    hand.ActualRanking = 10;
                }
                else if (hand.Num[4] >= 1)
                {
                    hand.ActualRanking = 9 + hand.Cards[0];
                }
                else if (hand.Num[3] >= 1 && hand.Num[2] >= 1)
                {
                    hand.ActualRanking = 8.5;
                }
                else if (hand.Num[3] >= 1)
                {
                    hand.ActualRanking = 8 + hand.Cards[0];
                }
                else if (hand.Num[2] >= 2)
                {
                    hand.ActualRanking = 7.5 + hand.Cards[0];
                }
                else if (hand.Num[2] >= 1)
                {
                    hand.ActualRanking = 7 + hand.Cards[0];
                }
                else
                {
                    hand.ActualRanking = 6 + hand.Cards[0];
                }

                // Exception for 5 jokers
                if (hand.Cards[0] == 5)
                {
                    hand.ActualRanking = 10;
                }
            }

            hands.Sort();

            var totalWinnings = 0d;
            var rank = 0;
            foreach (var h in hands)
            {
                rank++;
                totalWinnings += h.Bid * rank;
            }

            Console.WriteLine(totalWinnings);
        }
    }

    public class Hand : IComparable<Hand>
    {
        public string OriginalString;
        public double Bid;
        public Dictionary<int, int> Cards = new Dictionary<int, int>();
        public double Ranking = 0;

        public Dictionary<int, int> Num = new Dictionary<int, int>();
        public double ActualRanking = 0;

        public Hand()
        {
            for (var i = 0; i <= 14; i++)
            {
                Cards[i] = 0;
            }
            for (var i = 1; i <= 5; i++)
            {
                Num[i] = 0;
            }
        }

        public int CompareTo(Hand? other)
        {
            if (this.ActualRanking != other.ActualRanking)
            {
                return this.ActualRanking > other.ActualRanking ? 1 : -1;
            }
            else if (this.Ranking != other.Ranking)
            {
                return this.Ranking > other.Ranking ? 1 : -1;
            }

            return 0;
        }
    }

}
