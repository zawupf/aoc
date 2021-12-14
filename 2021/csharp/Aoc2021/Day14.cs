namespace Aoc2021;

public class Day14 : IDay
{
    public override string Day { get; } = nameof(Day14)[3..];

    public override string Result1()
    {
        return MaxMinDiff(10, InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return MaxMinDiff(40, InputLines)
            .ToString(CultureInfo.InvariantCulture);
    }

    public static long MaxMinDiff(int n, IEnumerable<string> lines)
    {
        (string polymer, Dictionary<string, string> insertions) = Parse(lines);
        Dictionary<char, long> charCounts = Polymerize(polymer, insertions)
            .ElementAt(n - 1);
        long min = charCounts.MinBy(pair => pair.Value).Value;
        long max = charCounts.MaxBy(pair => pair.Value).Value;
        return max - min;
    }

    private static IEnumerable<Dictionary<char, long>> Polymerize(string polymer, Dictionary<string, string> insertions)
    {
        Dictionary<char, long> chars = polymer
            .GroupBy(c => c, (c, chars) => (c, count: chars.LongCount()))
            .ToDictionary(item => item.c, item => item.count);

        Dictionary<string, long> pairs = Enumerable.Range(0, polymer.Length - 1)
            .Select(i => polymer[i..(i + 2)])
            .GroupBy(pair => pair, (pair, pairs) => (pair, count: pairs.LongCount()))
            .ToDictionary(item => item.pair, item => item.count);

        while (true)
        {
            pairs = pairs
                .Aggregate(
                    new Dictionary<string, long>(), (currentPairs, kv) =>
                    {
                        (string pair, long count) = kv;
                        string c = insertions[pair];
                        IncrementCharBy(count, c[0], chars);
                        IncrementPairBy(count, pair[0] + c, currentPairs);
                        IncrementPairBy(count, c + pair[1], currentPairs);
                        IncrementPairBy(-count, pair, currentPairs);
                        return currentPairs;
                    }
                )
                .Aggregate(
                    pairs, (pairs, kv) =>
                    {
                        (string pair, long count) = kv;
                        IncrementPairBy(count, pair, pairs);
                        return pairs;
                    }
                );

            yield return chars;
        }

        static void IncrementPairBy(long count, string pair, Dictionary<string, long> chunks)
        {
            _ = chunks.TryAdd(pair, 0);
            chunks[pair] += count;
        }

        static void IncrementCharBy(long count, char c, Dictionary<char, long> chunks)
        {
            _ = chunks.TryAdd(c, 0);
            chunks[c] += count;
        }
    }

    private static (string polymer, Dictionary<string, string> insertions) Parse(IEnumerable<string> lines)
    {
        string polymer = lines.First();
        Dictionary<string, string> insertions = lines.Skip(2)
            .ToDictionary(line => line[..2], line => line[6..]);

        return (polymer, insertions);
    }
}
