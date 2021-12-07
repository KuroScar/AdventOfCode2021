using System;
using System.Collections.Generic;
using System.Linq;

using static Utils.InputLoader;

List<int> input = Load().First().Split(',').Select(int.Parse).OrderBy(item => item).ToList();

int part1 = GetSumOfFuel(input, (location, target) => Math.Abs(location - target));
Console.WriteLine($"Part1: {part1}");

int part2 = GetSumOfFuel(input, (location, target) =>
                                {
                                    int distance = Math.Abs(location - target);
                                    return (distance * (distance + 1)) / 2;
                                });
Console.WriteLine($"Part2: {part2}");

int GetSumOfFuel(List<int> ints, Func<int, int, int> costs)
{
    int lastSum = int.MaxValue;
    foreach (int i in Enumerable.Range(ints.First(), ints.Last()))
    {
        int sum = ints.Sum(item => costs(item, i));
        if (lastSum < sum)
        {
            return lastSum;
        }
        lastSum = sum;
    }
    return lastSum;
}