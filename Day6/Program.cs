using System;
using System.Collections.Generic;
using System.Linq;

using static Utils.InputLoader;

List<long> initialState = Load().First().Split(",").Select(long.Parse).ToList();

long[] fishCount = new long[9];
initialState.ForEach(item => fishCount[item]++);

foreach (var _ in Enumerable.Range(0, 256))
{
    long newFish = fishCount[0];
    for (int i = 1; i < fishCount.Length; i++)
    {
        fishCount[i - 1] = fishCount[i];
    }
    fishCount[6] += newFish;
    fishCount[8] = newFish;
}

Console.WriteLine(fishCount.Sum());