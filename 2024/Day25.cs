using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Day25
    {
        public static void Run()
        {
            var locks = new List<List<int>>();
            var keys = new List<List<int>>();

            var line = "dummy";
            while (true)
            {
                var keyOrLock = new List<string>();
                while (line != "")
                {
                    line = Console.ReadLine();
                    if (line.Length > 0)
                    {
                        keyOrLock.Add(line);
                    }
                }

                if (keyOrLock.Count == 0) { break; }

                // This is a lock
                var map = new char[keyOrLock[0].Length, keyOrLock.Count];
                for(var y = 0; y < keyOrLock.Count; y++)
                {
                    var ca = keyOrLock[y].ToCharArray();
                    for (var x = 0; x < ca.Length; x++)
                    {
                        map[x, y] = ca[x];
                    }
                }

                var isLock = (map[0, 0] == '#');
                var heightList = new List<int>();
                var endOfLookChar = isLock ? '#' : '.';
                for (var x = 0; x < keyOrLock[0].Length; x++) 
                {
                    for (var y = keyOrLock.Count - 1; y >= 0; y--)
                    {
                        if (map[x, y] == endOfLookChar)
                        {
                            heightList.Add(keyOrLock.Count - y - 1);
                            break;
                        }
                    }
                }

                if (isLock)
                {
                    locks.Add(heightList);
                }
                else
                {
                    keys.Add(heightList);
                }

                line = "dummy";
            }

            var fits = 0;
            foreach (var key in keys)
            {
                foreach (var loc in locks)
                { 
                    if (KeyGoesInLock(key, loc))
                    {
                        fits++;
                    }
                }
            }

            Console.WriteLine(fits);
        }

        private static bool KeyGoesInLock(List<int> key, List<int> loc)
        {
            for (var i = 0; i < key.Count; i++)
            {
                if (key[i] > loc[i]) { return false; }
            }

            return true;
        }
    }
}
