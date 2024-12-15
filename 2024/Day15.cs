using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode2024
{
    public static class Day15
    {
        public static void Run()
        {
            var line = "dummy";
            var inLines = new List<string>();
            while (line != "")
            {
                line = Console.ReadLine();
                if (line.Length <= 0) { break; }

                // Part 2: widen
                line = line.Replace(".", "..");
                line = line.Replace("#", "##");
                line = line.Replace("O", "[]");
                line = line.Replace("@", "@.");

                inLines.Add(line);
            }

            var map = new char[inLines[0].Length, inLines.Count];
            var robotPos = (-1, -1);
            for (var y = 0; y < inLines.Count; y++)
            {
                var tca = inLines[y].ToCharArray();
                for (var x = 0; x < tca.Length; x++)
                {
                    map[x, y] = tca[x];
                    if (map[x, y] == '@')
                    {
                        robotPos = (x, y);
                    }
                }
            }

            var gameDirections = new Dictionary<char, (int, int)>()
            {
                { 'w', (0, -1) },
                { 'd', (1, 0) },
                { 'a', (-1, 0) },
                { 's', (0, 1) },
            };
            // Simulate this in a game
            /*while (true)
            {
                var c = Console.ReadKey();
                var direction = gameDirections[c.KeyChar];
                Move2(map, robotPos, direction);
                robotPos = FindRobot(map);
                Print(map);
            }*/

            line = "dummy";
            var instructions = "";
            while (line != "")
            {
                line = Console.ReadLine();
                instructions = instructions + line;
            }

            var directions = new Dictionary<char, (int, int)>()
            {
                { '^', (0, -1) },
                { '>', (1, 0) },
                { '<', (-1, 0) },
                { 'v', (0, 1) },
            };
            
            // Move
            foreach (var c in instructions)
            {
                var direction = directions[c];
                //robotPos = Move(map, robotPos, direction);
                Move2(map, robotPos, direction);
                robotPos = FindRobot(map);

                Thread.Sleep(25);
                Console.WriteLine($"DIRECTION: {c}");
                Print(map);
            }

            Print(map);

            // Calculate score
            var score = GetScore(map);

            Console.WriteLine(score);
        }

        public static bool Move2(char[,] map, (int, int) thingToMove, (int, int) direction)
        {
            var thingBeingMoved = map[thingToMove.Item1, thingToMove.Item2];
            if (thingBeingMoved == '.') { return true; }
            if (thingBeingMoved == '#') { return false; }
            var nextSquare = (thingToMove.Item1 + direction.Item1, thingToMove.Item2 + direction.Item2);
            var nextThing = map[nextSquare.Item1, nextSquare.Item2];

            if (thingBeingMoved == '@')
            {
                var successful = Move2(map, nextSquare, direction);

                // Actually do the move
                if (successful)
                {
                    map[thingToMove.Item1, thingToMove.Item2] = '.';
                    map[nextSquare.Item1, nextSquare.Item2] = '@';
                }

                return successful;
            }

            if (thingBeingMoved == '[' || thingBeingMoved == ']')
            {
                // If moving left/right
                if (direction.Item1 != 0)
                {
                    var successful = Move2(map, (thingToMove.Item1 + 2*direction.Item1, thingToMove.Item2 + 2*direction.Item2), direction);
                    if (successful)
                    {
                        map[thingToMove.Item1 + 2 * direction.Item1, thingToMove.Item2 + 2 * direction.Item2] = map[thingToMove.Item1 + 1 * direction.Item1, thingToMove.Item2 + 1 * direction.Item2];
                        map[thingToMove.Item1 + 1 * direction.Item1, thingToMove.Item2 + 1 * direction.Item2] = map[thingToMove.Item1, thingToMove.Item2];
                        map[thingToMove.Item1, thingToMove.Item2] = '.';
                    }

                    return successful;
                }
                // Moving up/down
                else if (direction.Item1 == 0)
                {
                    var successful = false;
                    if (thingBeingMoved == '[')
                    {
                        successful = Move2(map, (thingToMove.Item1, thingToMove.Item2 + direction.Item2), direction)
                            && Move2(map, (thingToMove.Item1 + 1, thingToMove.Item2 + direction.Item2), direction);

                        if (successful)
                        {
                            map[thingToMove.Item1, thingToMove.Item2 + direction.Item2] = map[thingToMove.Item1, thingToMove.Item2];
                            map[thingToMove.Item1 + 1, thingToMove.Item2 + direction.Item2] = map[thingToMove.Item1 + 1, thingToMove.Item2];
                            map[thingToMove.Item1, thingToMove.Item2] = '.';
                            map[thingToMove.Item1 + 1, thingToMove.Item2] = '.';
                        }
                    }
                    else if (thingBeingMoved == ']')
                    {
                        successful = Move2(map, (thingToMove.Item1, thingToMove.Item2 + direction.Item2), direction)
                            && Move2(map, (thingToMove.Item1 - 1, thingToMove.Item2 + direction.Item2), direction);

                        if (successful)
                        {
                            map[thingToMove.Item1, thingToMove.Item2 + direction.Item2] = map[thingToMove.Item1, thingToMove.Item2];
                            map[thingToMove.Item1 - 1, thingToMove.Item2 + direction.Item2] = map[thingToMove.Item1 - 1, thingToMove.Item2];
                            map[thingToMove.Item1, thingToMove.Item2] = '.';
                            map[thingToMove.Item1 - 1, thingToMove.Item2] = '.';
                        }
                    }

                    return successful;
                }
            }

            throw new Exception($"INVALID THING ENCOUNTERED:{thingBeingMoved}");
        }
        
        public static int GetScore(char[,] map)
        {
            var score = 0;
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                {
                    var c = map[x, y];

                    // Part 2:
                    if (c == '[')
                    //if (c == 'O')
                    {
                        score += y * 100 + x;
                    }
                }
            }

            return score;
        }

        public static (int, int) FindRobot(char[,] map)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == '@')
                    {
                        return (x, y);
                    }
                }
            }

            throw new Exception("NO ROBOT");
        }

        public static void Print(char[,] map)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                {
                    sb.Append(map[x, y]);
                }
                sb.Append('\n');
            }
            Console.WriteLine(sb.ToString());
        }

        public static (int, int) Move(char[,] map, (int, int) robotPos, (int, int) direction)
        {
            var nextSquare = (robotPos.Item1 + direction.Item1, robotPos.Item2 + direction.Item2);
            var nextThing = map[nextSquare.Item1, nextSquare.Item2];

            if (nextThing == '#') { return robotPos; }

            if (nextThing == '.') {

                map[nextSquare.Item1, nextSquare.Item2] = '@';
                map[robotPos.Item1, robotPos.Item2] = '.';
                return nextSquare;
            }

            if (nextThing == 'O')
            {
                var nextnextSquare = (nextSquare.Item1 + direction.Item1, nextSquare.Item2 + direction.Item2);
                var nextnextThing = map[nextnextSquare.Item1, nextnextSquare.Item2];

                while (nextnextThing != '.' && nextnextThing != '#')
                {
                    nextnextSquare = (nextnextSquare.Item1 + direction.Item1, nextnextSquare.Item2 + direction.Item2);
                    nextnextThing = map[nextnextSquare.Item1, nextnextSquare.Item2];
                }

                if (nextnextThing == '#') { return robotPos; }
                if (nextnextThing == '.')
                {
                    map[nextnextSquare.Item1, nextnextSquare.Item2] = 'O';
                    map[nextSquare.Item1, nextSquare.Item2] = '@';
                    map[robotPos.Item1, robotPos.Item2] = '.';
                    return nextSquare;
                }
            }

            throw new Exception("INVALID INPUT");
        }
    }
}
