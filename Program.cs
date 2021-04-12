using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;


namespace ZipPic
{
    class Program
    {
        private static void ShowHeader() { 
            ZipPicDir zpDir = new ZipPicDir();
            zpDir.Help();

            Console.WriteLine();
            Console.WriteLine("We will search the `*kra` files.");
            Console.WriteLine($"What is the directory we will search file?\n\r(Default path: {zpDir.DefaultDir})");
        }

        static void Main(string[] args)
        {
            ShowHeader();

            ZipPicDir zpDir = new ZipPicDir();
            zpDir.WorkDir = Console.ReadLine();
            
            try {
                Console.WriteLine();
                Console.WriteLine($"We will work on the first file: {zpDir.GetFiles().FirstOrDefault()}");
                if (Directory.Exists(zpDir.TempDir))
                {
                    zpDir.CopyInTempDir();
                } 
                else 
                {
                    zpDir.CreateTempDir();
                }            
            }
            catch (Exception e) 
            {
                Console.WriteLine("[ERROR]:\r\n{0}", e.ToString());
            }

            zpDir.ExtractToImage();

            // var currentDate = DateTime.Now;
            // Console.WriteLine($"{Environment.NewLine}Hello, {name}, on {currentDate:d} at {currentDate:t}!");
            // Console.Write($"{Environment.NewLine}Press any key to exit...");
            // Console.ReadKey(true);
        }
    }
}
