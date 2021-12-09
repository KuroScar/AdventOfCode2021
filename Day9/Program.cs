using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using static Utils.InputLoader;

IEnumerable<string> input = Load();
int[][] array = input.Select(item => item.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
Caves caves = new (array);

// Part 1
int sum = caves.LowPointsValues().Select(item => item + 1).Sum();
Console.WriteLine(sum);

// Part2
int multiplyTop3BasinSize = caves.GetBasinSizes().OrderByDescending(item => item).Take(3).Aggregate(1, (x,y) => x * y);
Console.WriteLine(multiplyTop3BasinSize);


public class Caves
{
    public int Height { get; }

    public int Width { get; }

    public int[][] World { get; }

    public Caves(int[][] world)
    {
        Height = world.Length;
        Width = world[0].Length;
        World = world;
    }

    public IEnumerable<Point> LowPoints => Enumerable.Range(0, Height)
                                                     .SelectMany(y => Enumerable.Range(0, Width).Select(x => new Point(x, y)))
                                                     .Where(IsLowPoint);
    public IEnumerable<int> LowPointsValues() => LowPoints.Select(pos => World[pos.Y][pos.X]);

    public IEnumerable<int> GetBasinSizes() => LowPoints.Select(GetSizeOfBasin);

    private int GetSizeOfBasin(Point startPos)
    {
        Point start = new (startPos.X, startPos.Y);
        List<Point> totalPoints = ExploreBasin(start, new List<Point> { start });
        return totalPoints.Count;
    }

    private bool IsLowPoint(Point point)
    {
        List<Point> overflowConsidered = GetValidNeighbors(point);
        return overflowConsidered.Select(loc => World[loc.Y][loc.X]).All(item => World[point.Y][point.X] < item);
    }

    private List<Point> GetValidNeighbors(Point point)
    {
        int y = point.Y;
        int x = point.X;
        List<Point> neighbors = new()
        {
                new (x, y - 1),
                new (x - 1, y),
                new (x + 1, y),
                new (x, y + 1),
        };

        return neighbors.Where(IsValidPoint).ToList();
    }

    private List<Point> ExploreBasin(Point currentLoc, List<Point> exploredLoc)
    {
        List<Point> newNeighbors = GetValidNeighbors(currentLoc)
                                          .Where(item => World[item.Y][item.X] != 9)
                                          .Except(exploredLoc).ToList();
        exploredLoc.AddRange(newNeighbors);
        newNeighbors.ForEach(point => ExploreBasin(point, exploredLoc));
        return exploredLoc;
    }

    private bool IsValidPoint(Point point)
    {
        return point.Y < Height && point.Y >= 0 && point.X < Width && point.X >= 0;
    }
}