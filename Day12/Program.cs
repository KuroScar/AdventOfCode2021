using System;
using System.Collections.Generic;
using System.Linq;

using MoreLinq.Extensions;

using static Utils.InputLoader;

Dictionary<string, List<string>> connections = new();
void AddConnection(string a, string b)
{
    if (!connections.ContainsKey(a))
    {
        connections[a] = new List<string>();
    }
    connections[a].Add(b);
}

List<string[]> input = Load().Select(item => item.Split("-")).ToList();

foreach (var connection in input)
{
    AddConnection(connection[0], connection[1]);
    AddConnection(connection[1], connection[0]);
}

List<List<string>> explorePaths = ExplorePaths("start", new List<string>());
Console.WriteLine(explorePaths.Count);


List<List<string>> ExplorePaths(string location, List<string> currentPath)
{
    currentPath.Add(location);
    if (location.ToLower() == "end")
    {
        return new List<List<string>>() { currentPath };
    }

    List<string> pathsToVisit = connections[location].Where(item => CanBeVisited(item, currentPath)).ToList();
    return pathsToVisit.SelectMany(path => ExplorePaths(path, currentPath.ToList())).ToList();
}

bool CanBeVisited(string nextLocation, List<string> currentPath)
{
    if (!char.IsLower(nextLocation[0]))
    {
        return true;
    }
    if (nextLocation == "start")
    {
        return false;
    }

    if (currentPath.Where(item => char.IsLower(item[0]))
                   .GroupBy(item => item)
                   .All(item => item.Count() < 2))
    {
        return true;
    }

    return !currentPath.Contains(nextLocation);
}