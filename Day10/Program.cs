
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using static Utils.InputLoader;

Dictionary<char, int> syntaxErrorScore = new()
{
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 },
};
Dictionary<char, int> syntaxIncompleteScore = new()
{
        { ')', 1 },
        { ']', 2 },
        { '}', 3 },
        { '>', 4 },
};

List<(char open, char close)> validCombinations = new()
{
        ('(', ')'), ('[', ']'), ('{', '}'), ('<', '>'),
};


// Part 1
IEnumerable<char> syntaxFails = Load().Select(GetFirstSyntaxError).Where(item => item.HasValue).Select(item => item.Value);
Console.WriteLine(syntaxFails.Select(item => syntaxErrorScore[item]).Sum());

char? GetFirstSyntaxError(string input)
{
    Stack<char> state = new();

    foreach (char c in input)
    {
        if (validCombinations.Any(item => item.open == c))
        {
            state.Push(c);
        } else
        {
            bool worked = state.TryPop(out char pop);
            if (!worked || !validCombinations.Any(item => item.open == pop && item.close == c))
            {
                return c;
            }
        }
    }

    return null;
}




// Part 2
IEnumerable<string> missingSyntax = Load().Select(GetMissingSyntax).Where(item => !string.IsNullOrWhiteSpace(item));
List<long> scores = missingSyntax.Select(CalculateScore).OrderByDescending(item => item).ToList();
Console.WriteLine(scores[scores.Count / 2]);

long CalculateScore(string missingElements)
{
    long sum = 0;
    foreach (char c in missingElements)
    {
        sum = sum * 5 + syntaxIncompleteScore[c];
    }
    return sum;
}

string GetMissingSyntax(string input)
{
    Stack<char> state = new();

    foreach (char c in input)
    {
        if (validCombinations.Any(item => item.open == c))
        {
            state.Push(c);
        } else
        {
            bool worked = state.TryPop(out char pop);
            if (!worked || !validCombinations.Any(item => item.open == pop && item.close == c))
            {
                return null;
            }
        }
    }
    if (state.Count == 0)
    {
        return "";
    }

    return new string(state.Select(open => validCombinations.FirstOrDefault(item => item.open == open).close).ToArray());
}