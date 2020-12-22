using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DuplicateFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            // DuplicateFinder v0.1 ——— @rezamoradix

            if (args.Length <= 0) Environment.Exit(0);

            var files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories);
            Lookup<string, string> items = (Lookup<string, string>)files.ToLookup(f => GetMD5Hash(f));
            
            foreach (var file in items)
            {
                if (file.Count() > 1)
                {
                    var fenum = file.GetEnumerator();
                    Console.WriteLine($"[{file.Key}]");

                    for (int i = 0; i < file.Count(); i++)
                    {
                        fenum.MoveNext();
                        if (i != 0 && args.Length > 1 && args[1] == "-delete")
                        {
                            File.Delete(fenum.Current);
                            Console.WriteLine($"    -> (Deleted) {fenum.Current}");
                        }
                        else
                        {
                            Console.WriteLine($"    -> {fenum.Current}");
                        }
                    }

                }
            }
        }

        static string GetMD5Hash(string path)
        {
            using (var reader = File.OpenRead(path))
            {
                byte[] buffer = new byte[4096];
                reader.Read(buffer, 0, 4096);

                return MD5.HashData(buffer).Select(b => b.ToString("x2")).Aggregate((a, b) => a + b);
            }
        }
    }
}
