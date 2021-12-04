using System;
using System.Collections.Generic;
using System.Linq;

using MoreLinq;

public class BingoBoard
{
    private BingoNumber[][] BoardByRows { get; }
    public BingoBoard(IEnumerable<string> lines)
    {
        BoardByRows = lines.Select(item => item.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                               .Select(num => new BingoNumber(num, false))
                                               .ToArray())
                           .ToArray();

    }

    public void NewNumber(string number)
    {
        BoardByRows.SelectMany(item => item)
                   .Where(item => item.Number == number)
                   .ForEach(item => item.Marked = true);
    }

    public bool HasWon()
    {
        return BoardByRows.Concat(BoardByColumns)
                          .Any(item => item.All(numbers => numbers.Marked));
    }

    public IEnumerable<string> GetAllUnmarkedNumbers => BoardByRows.SelectMany(item => item)
                                                                   .Where(item => !item.Marked)
                                                                   .Select(item => item.Number);
    private IEnumerable<IEnumerable<BingoNumber>> BoardByColumns => Enumerable.Range(0, BoardByRows.Length)
                                                                              .Select(index => BoardByRows.Select(row => row[index]));

    record BingoNumber
    {
        public string Number { get; }

        public bool Marked { get; set; }

        public BingoNumber(string num, bool marked)
        {
            Number = num;
            Marked = marked;
        }
    }
}