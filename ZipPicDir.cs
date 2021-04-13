using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using ImageMagick;


namespace ZipPic
{
    public class ZipPicDir
    {
        public readonly List<string> HomeDir;  //  Directory.GetCurrentDirectory().Split('/').ToList();
        public readonly string TargetImage = "mergedimage.png";
        public string DefaultDir;
        public string TempDir;
        public string PathToZip;

        private string ubutuDefaultDir;
        private string windowsDefaultDir;
        private string workDir;
        

        public ZipPicDir() 
        {
            this.HomeDir = (Environment.OSVersion.Platform.ToString().ToLower().Contains("win")) 
                ? new List<string> { "c", "User", Environment.UserName, "Downloads" } 
                : Directory.GetCurrentDirectory().Split('/').ToList();
            
            this.ubutuDefaultDir = $"/{HomeDir[1]}/{HomeDir[2]}/Downloads";
            this.windowsDefaultDir = $"{HomeDir[0]}:\\{HomeDir[1]}\\{HomeDir[2]}\\{HomeDir[3]}";

            // this.DefaultDir = (Environment.OSVersion.Platform.ToString().Contains("win")) ? windowsDefaultDir : ubutuDefaultDir;
            this.DefaultDir = windowsDefaultDir;
            this.workDir = this.DefaultDir;
            this.TempDir = $"{WorkDir}/ZipPic";
        }

        public string WorkDir
        {
            get => workDir;
            set => workDir = (value != String.Empty) ? value : this.DefaultDir;
        }
        
        public void CreateTempDir()
        {
            DirectoryInfo tmp = Directory.CreateDirectory(TempDir);
            Console.WriteLine($"The {tmp} directory was created successfully at {Directory.GetCreationTime(TempDir)}.");
        }

        public void CopyInTempDir()
        {
            string pathFrom = GetFiles().FirstOrDefault();
            string fileName = Path.GetFileName(pathFrom);
            this.PathToZip = TempDir + "/" + fileName.Replace("kra", "zip");
            System.IO.File.Copy(pathFrom, PathToZip);
            Console.WriteLine($"Copy from {pathFrom} to {PathToZip}");
        }

        public string[] GetFiles() {
            string[] files = Directory.GetFiles(WorkDir, "*.kra", SearchOption.AllDirectories);
            
            Console.WriteLine("... searching ...");
            Console.WriteLine($"Found {files.Length} files into {WorkDir}:");
            foreach(string fileName in files)
                    Console.WriteLine(fileName);

            return files;
        }
    
        public void Help() {
            string helpTxt = $"[HELP]\r\nDefault paths for searching '*kta' files:\r\n -[ ] Ubuntu: {this.ubutuDefaultDir}\r\n -[x] Windows: {this.windowsDefaultDir}";
            Console.WriteLine(helpTxt);
        }

        public Stream GetImageStream(string toZip) 
        {
            using (var file = File.OpenRead(toZip))
            using(var zip = new ZipArchive(file, ZipArchiveMode.Read)) 
            {
                foreach(var entry in zip.Entries)
                {
                    var img = entry.Open();
                    if (entry.ToString() == TargetImage) {
                        using (MagickImage image = new(img))
                        {
                            var screenshot1 = image.Clone(0, 0, 1242, 2208);
                            screenshot1.Write(new FileInfo(@"C:\Temp\screen1.png"), MagickFormat.Png);
                        }
                        return img;
                    }
                }
            }

            return null;
        }

    }
}
