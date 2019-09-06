using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var inputFolder = new DirectoryInfo(@"C:\Users\Borovak\Desktop\input");
            var normalizedFolder = new DirectoryInfo(@"C:\Users\Borovak\Desktop\normalized");
            var loResFolder = new DirectoryInfo(@"C:\Users\Borovak\Desktop\lores");
            var normalizedSize = 1024;
            var loResSize = 32;

            foreach (var folder in new[] { normalizedFolder.FullName, loResFolder.FullName })
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

            var brush = new SolidBrush(Color.Black);
            foreach (var fileInfo in inputFolder.GetFiles())
            {
                var img = Bitmap.FromFile(fileInfo.FullName);
                var scale = Math.Min(normalizedSize / img.Width, normalizedSize / img.Height);

                var normalizedBmp = new Bitmap(normalizedSize, normalizedSize);
                var normalizedGraph = Graphics.FromImage(normalizedBmp);
                var loResBmp = new Bitmap(loResSize, loResSize);
                var loResGraph = Graphics.FromImage(loResBmp);

                var scaleWidth = img.Width * scale;
                var scaleHeight = img.Height * scale;

                normalizedGraph.FillRectangle(brush, new RectangleF(0, 0, normalizedSize, normalizedSize));
                normalizedGraph.DrawImage(img, (normalizedSize - scaleWidth) / 2, (normalizedSize - scaleHeight) / 2, scaleWidth, scaleHeight);
                normalizedBmp.Save(normalizedFolder.FullName + "\\" + fileInfo.Name);

                loResGraph.DrawImage(normalizedBmp, 0, 0, loResSize, loResSize);
                loResBmp.Save(loResFolder.FullName + "\\" + fileInfo.Name);
            }
        }
    }
}
