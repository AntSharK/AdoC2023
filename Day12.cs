using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day12
    {
        public static void Run()
        {
            var totalNum = 0l;
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "") break;

                var ss = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var arrangement = ss[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();


                // Split each arrangement into overlapping parts and non overlapping parts
                var arrangementOverlap = ss[0] + "?" + ss[0];
                var combiOverlap = GenerateAll(arrangementOverlap);
                var combiWithoutExtra = GenerateAll(ss[0]);

                var arrangementTwice = new List<int>();
                arrangementTwice.AddRange(arrangement);
                arrangementTwice.AddRange(arrangement);

                var numArrangementsWithOverlap = 0l;
                foreach (var combi in combiOverlap)
                {
                    var isValid = CheckString(combi, arrangementTwice);
                    if (isValid) numArrangementsWithOverlap++;
                }

                var numArrangementWithoutExtra = 0l;
                foreach (var combi in combiWithoutExtra)
                {
                    var isValid = CheckString(combi, arrangement);
                    if (isValid) numArrangementWithoutExtra++;
                }

                // 4 extra combinations arise from the "middle" part
                var multFactor = numArrangementsWithOverlap / numArrangementWithoutExtra;
                var mult = numArrangementWithoutExtra * numArrangementWithoutExtra * numArrangementWithoutExtra * numArrangementWithoutExtra * numArrangementWithoutExtra;
            }

            Console.WriteLine(totalNum);
        }

        private static List<string> GenerateAll(string s)
        {
            var retVal = new List<string>();
            if (!s.Contains('?'))
            {
                return new List<string>() { s };
            }

            var firstMark = s.IndexOf('?');

            var sdot = s.Substring(0, firstMark) + '.' + s.Substring(firstMark + 1, s.Length - firstMark - 1);
            var shash = s.Substring(0, firstMark) + '#' + s.Substring(firstMark + 1, s.Length - firstMark - 1);

            retVal.AddRange(GenerateAll(sdot));
            retVal.AddRange(GenerateAll(shash));
            return retVal;
        }

        private static bool CheckString(string s, List<int> arrangement)
        {
            if (!s.Contains('#') && arrangement.Count > 0) return false;

            var firstMark = s.IndexOf('#');
            var firstArr = arrangement[0];
            if (arrangement.Count == 1
                && s.Length == firstArr)
            {
                return !s.Contains('.');
            }

            var countLength = s.IndexOf('.', firstMark) - firstMark;
            if (countLength < 0) { countLength = s.Length - firstMark; }
            if (countLength != firstArr)
            {
                return false;
            }

            var remainingString = s.Substring(firstMark + firstArr);

            if (arrangement.Count == 1)
            {
                return !remainingString.Contains('#');
            }

            var reducedArr = new List<int>();
            foreach (var i in arrangement) reducedArr.Add(i);
            reducedArr.RemoveAt(0);
            return CheckString(remainingString, reducedArr);
        }

        /*
        private static long FindCombi(string s, List<int> arrangement)
        {
            // Check if this arrangement is impossible
            var firstMark = s.IndexOf('?');
            if (firstMark < 0) { return 0; }

            var sdot = s.Substring(0, firstMark) + '.' + s.Substring(firstMark + 1, s.Length - firstMark - 1);
            var shash = s.Substring(0, firstMark) + '#' + s.Substring(firstMark + 1, s.Length - firstMark - 1);

            var dotArrangements = 0l;
            if (IsPossibleArrangement(sdot, arrangement))
            {
                dotArrangements = FindCombi(sdot, arrangement);
            }

            var hashArrangements = 0l;
            if (IsPossibleArrangement(shash, arrangement))
            {
                hashArrangements = FindCombi(shash, arrangement);
            }

            return dotArrangements + hashArrangements;
        }

        private static bool IsPossibleArrangement(string s, List<int> arrangement)
        {
            if (arrangement.Count == 0 && !s.Contains('#')) {
                return true;
            }

            var firstElement = arrangement[0];
            var minLength = arrangement.Sum() + arrangement.Count() - 1;
            var arrTrim = new List<int>();
            foreach (var i in arrangement)
            {
                arrTrim.Add(i);
            }
            arrTrim.RemoveAt(0);

            for(var i = 0; i <= s.Length - minLength; i++)
            {
                var firstN = s.Substring(0, firstElement);
                if (!firstN.Contains('.'))
                {
                    if (i + firstElement == s.Length)
                    {
                        return true;
                    }
                    if (s[i + firstElement] != '#')
                    {
                        return IsPossibleArrangement(s.Substring(i + firstElement + 1), arrTrim);
                    }
                }
            }

            return false;
        }
        */
    }
}
