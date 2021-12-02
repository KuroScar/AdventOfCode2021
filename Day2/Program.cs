using System;
using System.Collections.Generic;
using static System.Console;
using static Utils.InputLoader;

// Part 1
// IEnumerable<(Direction direction, int speed)> actions = Load<Direction, int>();
// List<(Direction direction, int totalSpeed)> totalSpeeds = actions.GroupBy(item => item.direction).Select(item => (item.Key, item.Sum(item => item.speed))).ToList();
// var multipliedResult = totalSpeeds.First(item => item.direction == Direction.Forward).totalSpeed
//                      * (totalSpeeds.First(item => item.direction == Direction.Down).totalSpeed
//                         - totalSpeeds.First(item => item.direction == Direction.Up).totalSpeed);
//
// WriteLine(multipliedResult);

IEnumerable<(Direction direction, int speed)> actions = Load<(Direction, int)>();

int aim = 0;
int depth = 0;
int position = 0;

foreach (var action in actions)
{
     switch (action.direction)
     {
         case Direction.Forward:
             position += action.speed;
             depth += aim * action.speed;
             break;
         case Direction.Up:
             aim -= action.speed;
             break;
         case Direction.Down:
             aim += action.speed;
             break;
         default:
             throw new ArgumentOutOfRangeException();
     }
}

WriteLine(position * depth);

public enum Direction
{
    Forward,
    Up,
    Down
}