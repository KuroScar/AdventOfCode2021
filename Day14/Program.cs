using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;

using static Utils.InputLoader;

List<string> lines = Load().ToList();

string start = lines.First();
Dictionary<Key, Combination> paths = new();
ForEachExtension.ForEach(lines.Skip(2).Select(item => new Combination(item)), item => item.Register(paths));

List<char> result = start.ToList();
IEnumerable<Key> allCombinations = result.Zip(result.Skip(1)).Select(item => new Key { A = item.First, B = item.Second });

Dictionary<char, long> finalDist = result.GroupBy(item => item).ToDictionary(item => item.Key, item => item.LongCount());
foreach (Key combination in allCombinations)
{
    if (paths.ContainsKey(combination))
    {
        Dictionary<char, long> distribution = paths[combination].GetDistribution(40);
        foreach (KeyValuePair<char, long> pair in distribution)
        {
            if (finalDist.ContainsKey(pair.Key))
            {
                finalDist[pair.Key] += pair.Value;
            } else
            {
                finalDist[pair.Key] = pair.Value;
            }
        }
    }
}
List<KeyValuePair<char,long>> orderByDescending = finalDist.OrderByDescending(item => item.Value).ToList();
orderByDescending.ForEach(item => Console.WriteLine($"{item.Key}: {item.Value}"));
List<long> orderedCount = orderByDescending.Select(item => item.Value).ToList();
Console.WriteLine(orderedCount.First() - orderedCount.Last());

record Key
{
    public char A { get; set; }

    public char B { get; set; }
}

class Combination
{
    public Combination(string config)
    {
        string[] strings = config.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        AdditionalChar = strings[1][0];
        Key = new Key() { A = strings[0][0], B = strings[0][1] };
        Left = new Key() { A = Key.A, B = AdditionalChar };
        Right = new Key() { A = AdditionalChar, B = Key.B };
    }

    public void Register(Dictionary<Key, Combination> paths)
    {
        paths.Add(Key, this);
        Paths = paths;
    }

    public Dictionary<Key, Combination> Paths { get; private set; }

    public Key Key { get; }

    public Key Left { get; }

    public Key Right { get; }

    public char AdditionalChar { get; }

    public List<(int level, Dictionary<char, long> result)> Cache { get; }= new();

    public Dictionary<char, long> GetDistribution(int level)
    {
        if (level < 1)
        {
            return new();;
        }

        Dictionary<char, long> cacheHit = Cache.Where(item => item.level == level).Select(item => item.result).FirstOrDefault();
        if (cacheHit != null)
        {
            return cacheHit;
        }

        Dictionary<char, long> result = new() { [AdditionalChar] = 1 };
        if (level > 1)
        {
            MergeDistribution(Right, level, result);
            MergeDistribution(Left, level, result);
        }

        Cache.Add((level, result));
        return result;
    }

    private void MergeDistribution(Key key, int level, Dictionary<char, long> result)
    {
        if (!Paths.ContainsKey(key))
        {
            return;
        }

        Dictionary<char, long> distribution = Paths[key].GetDistribution(level - 1);
        foreach (KeyValuePair<char, long> pair in distribution)
        {
            if (result.ContainsKey(pair.Key))
            {
                result[pair.Key] += pair.Value;
            } else
            {
                result[pair.Key] = pair.Value;
            }
        }
    }
}