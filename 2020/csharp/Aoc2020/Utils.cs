using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Aoc2020
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
            var subpath = Path.Join("_inputs", $"Day{name}.txt");
            return FindFile(Directory.GetCurrentDirectory(), subpath);

            static string FindFile(string dir, string path)
            {
                var filepath = Path.Join(dir, path);
                return File.Exists(filepath)
                    ? filepath
                    : FindFile(Directory.GetParent(dir).FullName, path);
            }
        }

        static public IEnumerable<(T, T)> Combine2<T>(IEnumerable<T> entries)
        {
            var (head, tail) = (entries.First(), entries.Skip(1));
            while (tail.Any())
            {
                foreach (var current in tail)
                {
                    yield return (head, current);
                }
                (head, tail) = (tail.First(), tail.Skip(1));
            }
        }
    }
}
