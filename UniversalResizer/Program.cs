using System;
using System.Drawing;
using System.IO;

namespace UniversalResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var inputFolder = new DirectoryInfo(args[0]);
            //var normalizedFolder = new DirectoryInfo(args[1]);
            //var loResFolder = new DirectoryInfo(args[2]);
            //var size = Convert.ToInt32(args[3]);

            const string user = "brabancl";
            var inputFolder = new DirectoryInfo($@"C:\Users\{user}\Desktop\input");
            var normalizedFolder = new DirectoryInfo($@"C:\Users\{user}\Desktop\normalized");
            var loResFolder = new DirectoryInfo($@"C:\Users\{user}\Desktop\lores");
            var normalizedSize = 512;
            var loResSize = 32;

            foreach (var folder in new[] { normalizedFolder.FullName, loResFolder.FullName })
            {
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder, true);
                }
                Directory.CreateDirectory(folder);
            }

            var brush = new SolidBrush(Color.Black);
            var files = inputFolder.GetFiles();
            var index = 0;
            Console.WriteLine("Processing images");
            foreach (var fileInfo in files)
            {
                var newFileName = index.ToString("0000000") + ".jpg";
                var img = Image.FromFile(fileInfo.FullName);
                var scale = Math.Min(Convert.ToDouble(normalizedSize) / Convert.ToDouble(img.Width), Convert.ToDouble(normalizedSize) / Convert.ToDouble(img.Height));

                var normalizedBmp = new Bitmap(normalizedSize, normalizedSize);
                var normalizedGraph = Graphics.FromImage(normalizedBmp);
                var loResBmp = new Bitmap(loResSize, loResSize);
                var loResGraph = Graphics.FromImage(loResBmp);

                var scaleWidth = Convert.ToDouble(img.Width) * scale;
                var scaleHeight = Convert.ToDouble(img.Height) * scale;

                normalizedGraph.FillRectangle(brush, new RectangleF(0, 0, normalizedSize, normalizedSize));
                normalizedGraph.DrawImage(img, (normalizedSize - Convert.ToInt32(scaleWidth)) / 2, (normalizedSize - Convert.ToInt32(scaleHeight)) / 2, Convert.ToInt32(scaleWidth), Convert.ToInt32(scaleHeight));
                normalizedBmp.Save(normalizedFolder.FullName + "\\" + newFileName);

                loResGraph.DrawImage(normalizedBmp, 0, 0, loResSize, loResSize);
                loResBmp.Save(loResFolder.FullName + "\\" + newFileName);
                index++;
                DrawTextProgressBar(index, files.Length);
                //Console.WriteLine($"{index}/{files.Length} images processed");
            }
            Console.WriteLine("");
            Console.WriteLine($"{files.Length} images processed");
        }

        private static void DrawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}
