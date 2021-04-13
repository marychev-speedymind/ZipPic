using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using ImageMagick;


namespace ZipPic
{
    class Program
    {

        // public readonly static int Chunk = 1242;
        // public readonly static int Space = 52;

        //private static void ShowHeader() { 
        //    ZipPicDir zpDir = new();
        //    zpDir.Help();

        //    Console.WriteLine();
        //    Console.WriteLine("We will search the `*kra` files.");
        //    Console.WriteLine($"What is the directory we will search file?\n\r(Default path: {zpDir.DefaultDir})");
        //}

        //static bool ByteArrayCompare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        //{
        //    return a1.SequenceEqual(a2);
        //}

        static void Crop(MagickImage magickImage, string fullPath, int chunk, int spacing) 
        {
            int width = magickImage.Width;
            MagickGeometry geometry = new();

            while (width > 0) 
            {
                geometry.Width = chunk;
                geometry.Height = magickImage.Height;
                geometry.X += (chunk + spacing);
                geometry.Y = 0;

                CreateScreen(magickImage, geometry, fullPath);

                width -= (chunk + spacing);
            }
        }
        
        static FileInfo GetFullPathScreen(string path, MagickGeometry g)
        {
            string chinkImagePath = $"{Path.GetDirectoryName(path)}";
            string chinkImageName = $"{Path.GetFileNameWithoutExtension(path)}({g.Width} X {g.Height})--[{g.X}, {g.Y}].png";
            return new FileInfo($"{chinkImagePath}\\{chinkImageName}");
        }

        static void CreateScreen(MagickImage image, MagickGeometry geometry, string path)
        {
            var screen = image.Clone(geometry);
            screen.Write(GetFullPathScreen(path, geometry), MagickFormat.Png);
        }

        static void Main(string[] args)
        {
            string TargetImage = "mergedimage.png";

            // string workDir = Console.ReadLine();
            string workDir = @"C:/mmc";

            foreach (string fullPath in Directory.EnumerateFiles(workDir, "*.kra", SearchOption.AllDirectories))
            {
                Console.WriteLine(fullPath);

                using (var file = File.OpenRead(fullPath))
                using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
                {
                    var entry = zip.Entries.Where(x => x.ToString() == TargetImage).FirstOrDefault();
                    using MagickImage image = new(entry.Open());

                    switch (image.Height)
                    {
                        case 2208:
                            Crop(image, fullPath, 1242, 52);
                            break;
                        case 2688:
                            Crop(image, fullPath, 1242, 52);
                            break;
                        case 2732:
                            Crop(image, fullPath, 2732, 255);
                            break;
                        default:
                            Crop(image, fullPath, image.Width, 0);
                            break;
                    }

                    //if (image.Height == 2732)
                    //{
                    //    Console.WriteLine(image);
                    //}
                    //else if (image.Height == 2208)
                    //{
                    //    Crop(image, f, 1242, 52);
                    //}
                    //else if (image.Height == 2268)
                    //{
                    //    Console.WriteLine(image);
                    //}
                    //else
                    //{
                    //    Console.WriteLine(image);
                    //}

                }
            }


            //var bytes1 = File.ReadAllBytes(@"C:\Temp\screen1.png");
            //var bytes2 = File.ReadAllBytes(@"C:\Temp\screen2.png");
            //var equal = ByteArrayCompare(bytes1, bytes2);

            //ShowHeader();
            //ZipPicDir zpDir = new();

            // zpDir.WorkDir = Console.ReadLine();

            //try {
            //    Console.WriteLine();
            //    Console.WriteLine($"We will work on the first file: {zpDir.GetFiles().FirstOrDefault()}");
            //    if (Directory.Exists(zpDir.TempDir))
            //    {
            //        zpDir.CopyInTempDir();
            //    } 
            //    else 
            //    {
            //        zpDir.CreateTempDir();
            //    }            
            //}
            //catch (Exception e) 
            //{
            //    Console.WriteLine("[ERROR]:\r\n{0}", e.ToString());
            //}

            // var img = zpDir.GetImageStream(@"C:\Users\speed\Downloads\1242X2208.zip");

            //using (MagickImage image = new(zpDir.GetImageStream(@"C:\Users\speed\Downloads\1242X2208.zip")))
            //{
            //    var screenshot1 = image.Clone(0, 0, 1242, 2208);
            //    screenshot1.Write(new FileInfo(@"C:\Temp\screen1.png"), MagickFormat.Png);
            //}


            // var currentDate = DateTime.Now;
            // Console.WriteLine($"{Environment.NewLine}Hello, {name}, on {currentDate:d} at {currentDate:t}!");
            // Console.Write($"{Environment.NewLine}Press any key to exit...");
            Console.WriteLine("Finish! Press any keys");
            Console.ReadKey(true);
        }
    }
}
