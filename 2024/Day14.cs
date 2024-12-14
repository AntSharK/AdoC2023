using System.Drawing;

namespace AdventOfCode2024
{
    public static class Day14
    {
        public static void Run()
        {
            var robots = new List<(int startX, int startY, int speedX, int speedY)>();
            while (true)
            {
                var line = Console.ReadLine();
                if (line.Length <= 0)
                {
                    break;
                }

                var splitLine = line.Split(new[] { '=', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                robots.Add((Int32.Parse(splitLine[1]), Int32.Parse(splitLine[2]), 
                    Int32.Parse(splitLine[4]), Int32.Parse(splitLine[5])));
            }

            const int width = 101;
            const int height = 103;
            const int time = 1;

            // Sample input
            //const int width = 11;
            //const int height = 7;
            for (var iteration = 1; iteration < 9999; iteration++)
            {
                var newRobots = new List<(int x, int y, int speedX, int speedY)>();
                foreach (var r in robots)
                {
                    var finalX = (r.startX + time * r.speedX) % width;
                    var finalY = (r.startY + time * r.speedY) % height;

                    if (finalX < 0) { finalX = width + finalX; }
                    if (finalY < 0) { finalY = height + finalY; }

                    newRobots.Add((finalX, finalY, r.speedX, r.speedY));
                }

                var q1 = 0;
                var q2 = 0;
                var q3 = 0;
                var q4 = 0;
                foreach (var f in newRobots)
                {
                    var halfW = (width) / 2;
                    var halfH = (height) / 2;
                    if (f.x < halfW && f.y < halfH) { q1++; }
                    else if (f.x < halfW && f.y > (height - halfH - 1)) { q3++; }
                    else if (f.x > (width - halfW - 1) && f.y < halfH) { q2++; }
                    else if (f.x > (width - halfW - 1) && f.y > (height - halfH - 1)) { q4++; }
                }

                Console.WriteLine($"{q1}x{q2}x{q3}x{q4}={q1 * q2 * q3 * q4}");

                // P2: DISPLAY!!
                var grid = new bool[width, height];
                foreach (var f in newRobots)
                {
                    grid[f.x, f.y] = true;
                }
                
                var bmp = new RawBitmap(width, height);
                /*
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                }
                */
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (grid[x, y])
                        {
                             bmp.SetPixel(x, y, Color.White);
                        }
                    }
                }

                bmp.Save(iteration + ".bmp");
                Console.WriteLine($"Iteration:{iteration}");
                robots = newRobots;
            }
        }
    }
    public class RawBitmap
    {
        public readonly int Width;
        public readonly int Height;
        private readonly byte[] ImageBytes;

        public RawBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            ImageBytes = new byte[width * height * 4];
        }

        public void SetPixel(int x, int y, Color color)
        {
            int offset = ((Height - y - 1) * Width + x) * 4;
            ImageBytes[offset + 0] = color.B;
            ImageBytes[offset + 1] = color.G;
            ImageBytes[offset + 2] = color.R;
        }

        public byte[] GetBitmapBytes()
        {
            const int imageHeaderSize = 54;
            byte[] bmpBytes = new byte[ImageBytes.Length + imageHeaderSize];
            bmpBytes[0] = (byte)'B';
            bmpBytes[1] = (byte)'M';
            bmpBytes[14] = 40;
            Array.Copy(BitConverter.GetBytes(bmpBytes.Length), 0, bmpBytes, 2, 4);
            Array.Copy(BitConverter.GetBytes(imageHeaderSize), 0, bmpBytes, 10, 4);
            Array.Copy(BitConverter.GetBytes(Width), 0, bmpBytes, 18, 4);
            Array.Copy(BitConverter.GetBytes(Height), 0, bmpBytes, 22, 4);
            Array.Copy(BitConverter.GetBytes(32), 0, bmpBytes, 28, 2);
            Array.Copy(BitConverter.GetBytes(ImageBytes.Length), 0, bmpBytes, 34, 4);
            Array.Copy(ImageBytes, 0, bmpBytes, imageHeaderSize, ImageBytes.Length);
            return bmpBytes;
        }

        public void Save(string filename)
        {
            byte[] bytes = GetBitmapBytes();
            File.WriteAllBytes(filename, bytes);
        }
    }
}
