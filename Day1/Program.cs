using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static Utils.InputLoader;

// Part 1
List<long> input = Load(0).Select(long.Parse).ToList();
WriteLine(input.Zip(input.Skip(1), (l, l1) => l < l1).Count(item => item));

// Part 2
List<long> input1 = Load(1).Select(long.Parse).ToList();
input1 = input1.Zip(input1.Skip(1)).Zip(input1.Skip(2)).Select(item => item.Second + item.First.First + item.First.Second).ToList();
WriteLine(input1.Zip(input1.Skip(1), (l, l1) => l < l1).Count(item => item));