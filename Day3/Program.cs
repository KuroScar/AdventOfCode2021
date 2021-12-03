
using System;
using System.Collections.Generic;
using System.Linq;

using static Utils.InputLoader;



SolvePart1(Load());
SolvePart2(Load().ToList());




void SolvePart2(List<string> lines)
{
    string oxyValue = SearchValue(lines.ToList(), true);
    string co2Value = SearchValue(lines.ToList(), false);

    Console.WriteLine(oxyValue);
    Console.WriteLine(co2Value);
    Console.WriteLine(Convert.ToInt32(oxyValue, 2) * Convert.ToInt32(co2Value, 2));
}

string SearchValue(List<string> searchSpace, bool searchForCommon)
{
    int binaryLength = searchSpace.First().Length;
    for (int i = 0; i < binaryLength; i++)
    {
        var grouped = searchSpace.GroupBy(item => item[i]);
        var ordered = searchForCommon ?
                              grouped.OrderByDescending(item => item.Count()).ThenByDescending(item => item.Key) :
                              grouped.OrderBy(item => item.Count()).ThenBy(item => item.Key);

        searchSpace = ordered.First().Select(item => item).ToList();
        if (searchSpace.Count == 1)
        {
            return searchSpace.First();
        }
    }
    throw new Exception("Failed to find one value");
}

void SolvePart1(IEnumerable<string> lines)
{
    IEnumerable<char> result = lines.SelectMany(item => item.Select((c, index) => (index, int.Parse(c.ToString()))))
                                    .GroupBy(item => item.index, el => el.Item2)
                                    .OrderBy(item => item.Key)
                                    .Select(item => item.Sum() > item.Count() / 2 ? '1' : '0');
    string higherLimit = new(result.ToArray());
    string lowerLimit = new(higherLimit.Select(item => item == '1' ? '0' : '1').ToArray());
    Console.WriteLine(Convert.ToInt32(higherLimit, 2) * Convert.ToInt32(lowerLimit, 2));
}