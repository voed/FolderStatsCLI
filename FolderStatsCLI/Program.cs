using System;
using System.Collections.Generic;
using System.Linq;
//using System.IO;
using System.Threading.Tasks;
using Delimon.Win32.IO;

namespace FolderStatsCLI
{
    class Program
    {
        static List<string> list = new List<string>();
        static void Main(string[] args)
        {
            sizes szs = new sizes();
            Console.WriteLine("enter path");
            string path = Console.ReadLine();
            Console.WriteLine("Reading all files from " + path);
            GetAllFiles(path);
            long i = 0;
            string temp;
            foreach (string file in list)
            {
                long len = new FileInfo(file).Length;
                if (len == 0)
                {
                    szs.x0++;
                }
                else if (len <= 512)
                {
                    szs.x512++;
                }
                else if (len <= 1024)
                {
                    szs.x1k++;
                }
                else if (len <= 2048)
                {
                    szs.x2k++;
                }
                else if (len <= 4096)
                {
                    szs.x4k++;
                }
                else if (len <= 8192)
                {
                    szs.x8k++;
                }
                else if (len <= 16384)
                {
                    szs.x16k++;
                }
                else if (len <= 32768)
                {
                    szs.x32k++;
                }

                i++;
                if ((i % 100) == 0)
                {
                    Console.WriteLine(percent(i) + "% complete (file) " + i + " of " + list.Count);
                    temp = String.Format("| 0b:{0} | 512b:{1} | 1kb:{2} | 2kb:{3} | 4kb:{4} | 8kb:{5} | 16kb:{6} | 32kb:{7}",
                        szs.x0, szs.x512, szs.x1k, szs.x2k, szs.x4k, szs.x8k, szs.x16k, szs.x32k);
                    Console.WriteLine(temp);
                }
            }
            long total_files = szs.x0 + szs.x512 + szs.x1k + szs.x2k + szs.x4k + szs.x8k + szs.x16k + szs.x32k;
            long not_tested = list.Count - total_files;

            string output = "\nStats for folder " + path + " Done. \n=======================================================\n" +
                "0 bytes:      " + szs.x0       + " (" + percent(szs.x0)     + "%)\n" +
                "1 to 512 b:   " + szs.x512     + " (" + percent(szs.x512)   + "%)\n" +
                "513 to 1kb:   " + szs.x1k      + " (" + percent(szs.x1k)    + "%)\n" +
                "1kb to 2kb:   " + szs.x2k      + " (" + percent(szs.x2k)    + "%)\n" +
                "2kb to 4kb:   " + szs.x4k      + " (" + percent(szs.x4k)    + "%)\n" +
                "4kb to 8kb:   " + szs.x8k      + " (" + percent(szs.x8k)    + "%)\n" +
                "8kb to 16kb:  " + szs.x16k     + " (" + percent(szs.x16k)   + "%)\n" +
                "16kb to 32kb: " + szs.x32k     + " (" + percent(szs.x32k)   + "%)\n" +
                "not tested:   " + not_tested   + " (" + percent(not_tested) + "%)\n"; 

             Console.WriteLine(output);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"log.txt", true))
            {
                file.Write(output);
            }
            Console.ReadKey();

        }
        
        static double percent(long count)
        {
            double result = ((double)count * 100) / list.Count;
            return Math.Round(result, 2);
        }

        public static void GetAllFiles(String folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                list.Add(file);
            }
            foreach (string subDir in Directory.GetDirectories(folder))
            {
                try
                {
                    GetAllFiles(subDir);
                    Console.WriteLine("Reading subfolder " + subDir);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    public class sizes
    {
        public long x0
        {
            get; set;
        }
        public long x512
        {
            get; set;
        }
        public long x1k
        {
            get; set;
        }
        public long x2k
        {
            get; set;
        }
        public long x4k
        {
            get; set;
        }
        public long x8k
        {
            get; set;
        }
        public long x16k
        {
            get; set;
        }
        public long x32k
        {
            get; set;
        }
    }
}
