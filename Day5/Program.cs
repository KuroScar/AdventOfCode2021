using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using static Utils.InputLoader;

var input = Load<(string start, string direction, string stop)>().ToList();
var parsedInput = input.Select(line => new Line(GetPoint(line.start), GetPoint(line.stop))); // Part1: .Where(item => item.Pitch == 0)

IEnumerable<Point> dangerPoints = parsedInput.SelectMany(item => item.GetPointsOnLine()).ToList();
int dangerZonesAmount = dangerPoints.GroupBy(item => item).Count(item => item.Count() > 1);

Console.WriteLine(dangerZonesAmount);

Point GetPoint(string segment)
{
    List<int> vals = segment.Split(",").Select(int.Parse).ToList();
    return new Point(vals.ElementAt(0), vals.ElementAt(1));
}

public class Line
{
    public Point Start { get; }

    public Point End { get; }

    public int Pitch { get; set; }

    private int Width => End.X - Start.X;

    private int Height => End.Y - Start.Y;

    public Line(Point start, Point end)
    {
        Start = new Point(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y));
        End = new Point(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y));

        CalcPitch(start, end);
    }

    private void CalcPitch(Point start, Point end)
    {
        Pitch = 0;
        int pitchNom = (end.X - start.X);
        if (pitchNom != 0)
        {
            Pitch = (end.Y - start.Y) / pitchNom;
        }
    }

    public IEnumerable<Point> GetPointsOnLine()
    {
        if (Pitch != 0)
        {
            return Enumerable.Range(0, Width + 1).Select(i => new Point(Start.X + (Pitch == 1 ? i : Width - i), Start.Y + i ));
        }

        if (Width == 0)
        {
            return Enumerable.Range(Start.Y, Height + 1).Select(y => new Point(Start.X, y));
        }
        return Enumerable.Range(Start.X, Width + 1).Select(x => new Point(x, Start.Y));
    }
}