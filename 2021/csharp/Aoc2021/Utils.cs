namespace Aoc2021;

public static class Utils
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
        string subpath = Path.Join("_inputs", $"Day{name}.txt");
        return FindFile(Directory.GetCurrentDirectory(), subpath);

        static string FindFile(string dir, string path)
        {
            string filepath = Path.Join(dir, path);
            return File.Exists(filepath)
                ? filepath
                : FindFile(Directory.GetParent(dir)?.FullName ?? "invalid-dir", path);
        }
    }

    public static IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length)
    {
        return length == 1
            ? list.Select(t => new T[] { t })
            : Permutations(list, length - 1)
                .SelectMany(
                    t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    public static IEnumerable<(T, T)> Combine2<T>(IEnumerable<T> entries)
    {
        (T head, IEnumerable<T> tail) = (entries.First(), entries.Skip(1));
        while (tail.Any())
        {
            foreach (T current in tail)
            {
                yield return (head, current);
            }
            (head, tail) = (tail.First(), tail.Skip(1));
        }
    }

    public static int ParseInt(string value)
    {
        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    public static long ParseLong(string value)
    {
        return long.Parse(value, CultureInfo.InvariantCulture);
    }

    public static bool ParseBool(string value)
    {
        return bool.Parse(value);
    }
}
