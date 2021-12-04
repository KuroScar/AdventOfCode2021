using System;
using System.Collections.Generic;
using System.Linq;

using MoreLinq;

using static Utils.InputLoader;

List<string> lines = Load().Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
List<string> bingoNumbers = lines.First().Split(',').ToList();

var bingoBoards = lines.Skip(1).Batch(5).Select(board => new BingoBoard(board)).ToList();


SolvePart1(bingoNumbers, bingoBoards);
SolvePart2(bingoNumbers, bingoBoards);


void SolvePart2(List<string> list, List<BingoBoard> boards)
{
    foreach (string number in list)
    {
        boards.ForEach(board => board.NewNumber(number));
        List<BingoBoard> wonBoards = boards.Where(item => item.HasWon()).ToList();
        boards = boards.Except(wonBoards).ToList();
        if (!boards.Any())
        {
            int sumUnmarkedNumbers = wonBoards.First().GetAllUnmarkedNumbers.Select(int.Parse).Sum();
            Console.WriteLine($"{sumUnmarkedNumbers} * {number} = {sumUnmarkedNumbers * int.Parse(number)}");
            return;
        }
    }
}

void SolvePart1(List<string> list, List<BingoBoard> bingoBoards1)
{
    foreach (string number in list)
    {
        bingoBoards1.ForEach(board => board.NewNumber(number));
        BingoBoard wonBoard = bingoBoards1.FirstOrDefault(item => item.HasWon());

        if (wonBoard != null)
        {
            int sumUnmarkedNumbers = wonBoard.GetAllUnmarkedNumbers.Select(int.Parse).Sum();
            Console.WriteLine($"{sumUnmarkedNumbers} * {number} = {sumUnmarkedNumbers * int.Parse(number)}");
            return;
        }
    }
}