using System.Collections.Generic;
using System.IO;

namespace Aoc._2019
{
    public class Utils
    {
        public static IEnumerable<string> ReadInputLines(string name)
        {
            return File.ReadLines(FindInputFile(name));
        }

        public static string ReadInputText(string name)
        {
            return File.ReadAllText(FindInputFile(name)).Trim();
        }

        private static string FindInputFile(string name)
        {
            var subpath = Path.Join("inputs", $"Day{name}.txt");
            return FindFile(Directory.GetCurrentDirectory(), subpath);

            string FindFile(string dir, string path)
            {
                var filepath = Path.Join(dir, path);
                if (File.Exists(filepath))
                    return filepath;
                else
                    return FindFile(Directory.GetParent(dir).FullName, path);
            }
        }
    }
}
