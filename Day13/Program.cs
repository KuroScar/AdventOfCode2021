using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using static Utils.InputLoader;

List<string> lines = Load().ToList();
List<Point> points = lines.TakeWhile(item => !string.IsNullOrWhiteSpace(item))
                                 .Select(item => item.Split(','))
                                 .Select(item => new Point(int.Parse(item[0].ToString()), int.Parse(item[1].ToString())))
                                 .ToList();

IEnumerable<(bool horizontal, int location)> folds = lines.Where(item => item.Contains("fold"))
                                                          .Select(item => item.Replace("fold along ", ""))
                                                          .Select(item => item.Split("="))
                                                          .Select(item => (horizontal: item[0] == "y", location: int.Parse(item[1])));

int maxWidth = points.Max(item => item.X) + 1;
int maxHeight = points.Max(item => item.Y) + 1;

List<List<bool>> world = Enumerable.Range(0, maxHeight).Select(item => new bool[maxWidth].ToList()).ToList();

points.ForEach(point => world[point.Y][point.X] = true);

foreach ((bool horizontal, int location) fold in folds)
{
    if (fold.horizontal)
    {
        int outOfCenter = fold.location * 2 == world.Count ? 1 : 0;
        world = world.Take(fold.location)
                     .Zip(world.Take(outOfCenter).Concat(world.TakeLast(fold.location - outOfCenter).Reverse()),
                          (a, b) => a.Zip(b, (b1, b2) => b1 || b2).ToList())
                     .ToList();
    } else
    {
        int outOfCenter = fold.location * 2 == world[0].Count ? 1 : 0;

        world = world.Select(item => item.Take(fold.location)
                                         .Zip(item.Take(outOfCenter).Concat(item.TakeLast(fold.location - outOfCenter).Reverse()),
                                                                            (b1, b2) => b1 || b2).ToList())
                     .ToList();
    }
}

world.Select(item => item).ToList().ForEach(item => Console.WriteLine(new string(item.Select(val => val ? 'X' : ' ').ToArray())));