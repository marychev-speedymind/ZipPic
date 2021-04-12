using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;


namespace ZipPic
{
    public class ZipPicDir
    {
        public readonly List<string> HomeDir = Directory.GetCurrentDirectory().Split('/').ToList();
        public readonly string TargetImage = "mergedimage.png";
        public string DefaultDir;
        public string TempDir;
        public string PathToZip;

        private string ubutuDefaultDir;
        private string windowsDefaultDir;
        private string workDir;
        

        public ZipPicDir() 
        {
            this.TempDir = $"{WorkDir}/ZipPic";
            this.ubutuDefaultDir = $"/{HomeDir[1]}/{HomeDir[2]}/Downloads";
            // todo:
            this.windowsDefaultDir = @"c:\";    
            this.DefaultDir = ubutuDefaultDir;
            this.workDir = ubutuDefaultDir;
            this.TempDir = $"{this.workDir}/ZipPic";
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
            string helpTxt = $"[HELP]\r\nDefault paths for searching '*kta' files:\r\n -[x] Ubuntu: {this.ubutuDefaultDir}\r\n -[ ] Windows: {this.windowsDefaultDir}";
            Console.WriteLine(helpTxt);
        }

        public void ExtractToImage() 
        {
            using(var file = File.OpenRead(PathToZip))
            using(var zip = new ZipArchive(file, ZipArchiveMode.Read)) 
            {
                foreach(var entry in zip.Entries)
                {
                    if (entry.ToString() == TargetImage) {
                        string destination = Path.GetFullPath(Path.Combine(TempDir, entry.FullName));
                        entry.ExtractToFile(destination);
                        return;
                    }
                }
            }
        }

    }
}
