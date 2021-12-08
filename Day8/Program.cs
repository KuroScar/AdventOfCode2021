using System;
using System.Collections.Generic;
using System.Linq;

using static Utils.InputLoader;

List<string[]> inputOutput = Load().Select(item => item.Split("|")).ToList();

Dictionary<string, char> correctMapping = new()
{
        { "bc", '1' },
        { "abdeg", '2' },
        { "abcdg", '3' },
        { "bcfg", '4' },
        { "acdfg", '5' },
        { "acdefg", '6' },
        { "abc", '7' },
        { "abcdefg", '8' },
        { "abcdfg", '9' },
        { "abcdef", '0' },
};

int sum = 0;
foreach (string[] input in inputOutput)
{
    var nums = input.First().Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var output = input.Last().Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    Dictionary<char, char> mappingSeg = FigureOutMapping(nums);

    string number = new (output.Select(num => num.Select(c => mappingSeg[c]).OrderBy(c => c))
                               .Select(num => correctMapping[new string(num.ToArray())])
                               .ToArray());
    sum += int.Parse(number);
}

Console.WriteLine(sum);

Dictionary<char, char> FigureOutMapping(string[] numbers)
{
    Dictionary<char, char> dictionary = new();
    Dictionary<int, string> numToSeg = new();

    numToSeg[1] = numbers.First(item => item.Length == 2);
    numToSeg[7] = numbers.First(item => item.Length == 3);
    numToSeg[4] = numbers.First(item => item.Length == 4);
    numToSeg[8] = numbers.First(item => item.Length == 7);

    dictionary['a'] = numToSeg[7].Except(numToSeg[1]).Single(); // 7 and 1 are different in the A segment

    List<char> knownSegs9 = new() { dictionary['a'] };
    numToSeg[9] = numbers.Where(nums => nums.Length == 6) // 9 has 6 segments
                         .Where(nums => numToSeg[4].All(nums.Contains)) // contains all of 4 segments
                         .Single(nums => knownSegs9.All(nums.Contains)); // plus a segment

    dictionary['d'] = numToSeg[9].Except(knownSegs9).Except(numToSeg[4]).Single(); // 9 - 4 - a => d
    dictionary['e'] = numToSeg[8].Except(numToSeg[9]).Single(); // 8 - 9 => e

    List<char> knownSegs3 = new() { dictionary['a'], dictionary['d'] };
    numToSeg[3] = numbers.Where(nums => nums.Length == 5) // 3 has 5 segments
                         .Where(nums => numToSeg[1].All(nums.Contains)).Single(nums => knownSegs3.All(nums.Contains));

    dictionary['g'] = numToSeg[3].Except(knownSegs3).Except(numToSeg[1]).Single(); // 3 - a - d - 1 => g

    List<char> knownSegs2 = new() { dictionary['a'], dictionary['d'], dictionary['g'], dictionary['e'] };
    numToSeg[2] = numbers.Where(nums => nums.Length == 5) // 5 has 5 segments
                         .Single(nums => knownSegs2.All(nums.Contains));

    dictionary['b'] = numToSeg[2].Except(knownSegs2).Single(); // 2 - a - d - g - e => f
    dictionary['f'] = numToSeg[8].Except(numToSeg[2]).Except(numToSeg[1]).Single(); // 8 - 2 - 1 => f
    dictionary['c'] = numToSeg[1].Single(num => num != dictionary['b']); // 1 - b => c

    return dictionary.ToDictionary(item => item.Value, item => item.Key);
}