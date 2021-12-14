using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using MoreLinq.Extensions;

using static Utils.InputLoader;

Dictionary<Point, int> world = Load().SelectMany((item, y) => item.Select((c, x) =>( new Point(x, y) , int.Parse(c.ToString())))).ToArray().ToDictionary(item => item.Item1, item => item.Item2);

int Height = world.Keys.Max(item => item.Y) + 1;
int Width = world.Keys.Max(item => item.X) + 1;
int totalFlashes = 0;

foreach (var i in Enumerable.Range(0, 1000))
{
    world.ForEach(item => world[item.Key]++);
    world.Where(item => item.Value == 10).ToList().ForEach(item => Flash(item.Key));
    totalFlashes += world.Count(item => item.Value >= 10);

    if (world.All(item => item.Value >= 10))
    {
        Console.WriteLine("All flashed in step " + (i + 1));
        break;
    }

    world.Where(item => item.Value >= 10).ForEach(item => world[item.Key] = 0);
}

Console.WriteLine(totalFlashes);

void Flash(Point p)
{
    int y = p.Y;
    int x = p.X;
    List<Point> neighbors = new()
    {
            new(x - 1, y - 1),
            new(x, y - 1),
            new(x + 1, y - 1),
            new(x - 1, y),
            new(x + 1, y),
            new(x - 1, y + 1),
            new(x, y + 1),
            new(x + 1, y + 1),
    };

    neighbors = neighbors.Where(IsValidPoint).ToList();

    neighbors.ForEach(point =>
                      {
                          world[point]++;
                          if (world[point] == 10)
                          {
                              Flash(point);
                          }
                      });

}

bool IsValidPoint(Point point)
{
    return point.Y < Height && point.Y >= 0 && point.X < Width && point.X >= 0;
}