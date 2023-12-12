using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4
    {
        public static void Run()
        {
            var line = "dummy";
            var numWinnersPerCard = new Dictionary<int, int>();
            var cardNumber = 0;
            while (line != "")
            {
                cardNumber++;
                var numWinners = 0;
                line = Console.ReadLine();
                line = line.Substring(line.IndexOf(':') + 1);
                if (line == "") { break; }

                var cardSplit = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                var nums = cardSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                HashSet<string> winners = new HashSet<string>();
                foreach (var num in nums)
                {
                    winners.Add(num);
                }

                var myCards = cardSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var card in myCards)
                {
                    if (winners.Contains(card))
                    {
                        numWinners++;
                    }
                }

                numWinnersPerCard[cardNumber] = numWinners;
            }

            var numberOfCards = new Dictionary<int, int>();
            for (var i = 1; i < cardNumber; i++)
            {
                numberOfCards[i] = 1;
            }

            for (var i = 1; i < cardNumber; i++)
            {
                for (var j = 1; j <= numWinnersPerCard[i]; j++)
                {
                    if (i + j < cardNumber)
                    {
                        numberOfCards[i + j] += numberOfCards[i];
                    }
                }
            }

            Console.WriteLine(numberOfCards.Values.Sum());
        }
    }
}
