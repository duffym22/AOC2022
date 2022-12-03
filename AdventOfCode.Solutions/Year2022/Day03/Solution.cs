using System.Net;

namespace AdventOfCode.Solutions.Year2022.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2022, "") { }

    protected override string SolvePartOne()
    {
        //Split the input by newline
        List<string> lines = Input.SplitByNewline().ToList();
        List<char> priorityItems = new List<char>();

        foreach (string line in lines)
        {
            //Take the line, split it in half to get the two compartments
            int half = line.Length / 2;
            string compartmentOne = line.Substring(0, half);
            string compartmentTwo = line.Substring(half, half);

            //Check each compartment for characters
            priorityItems.Add(compartmentOne.Intersect(compartmentTwo).ToList().FirstOrDefault());
        }

        int prioritySum = 0;
        foreach (char item in priorityItems)
        {
            //Uppercase A-Z goes from 65d - 90d     :: want range to be 27-52
            //Lowercase a-z goes from 97d - 122d    :: want range to be 1-26
            //Determine if upper or lower case
            //Uppercase - Subtract 38 to bring it in the proper range (A = 65d ==> 65-38 = 27)
            //Lowercase - Subtract 96 to bring it in the proper range (a = 97d ==> 97-96 = 1)
            prioritySum += char.IsAsciiLetterUpper(item) ? (int)item - 38 : (int)item - 96;
        }

        //Attempt 1: 7917 (correct)
        return prioritySum.ToString();
    }

    protected override string SolvePartTwo()
    {
        //Split the input by newline
        List<string> lines = Input.SplitByNewline().ToList();
        List<char> priorityBadges = new List<char>();

        //Increment by 3 for the elf groups
        for (int i = 0; i < lines.Count; i += 3)
        {
            //Check each compartment for characters
            priorityBadges.Add(lines[i].Intersect(lines[i + 1]).Intersect(lines[i + 2]).ToList().FirstOrDefault());
        }

        int prioritySum = 0;
        foreach (char item in priorityBadges)
        {
            //Uppercase A-Z goes from 65d - 90d     :: want range to be 27-52
            //Lowercase a-z goes from 97d - 122d    :: want range to be 1-26
            //Determine if upper or lower case
            //Uppercase - Subtract 38 to bring it in the proper range (A = 65d ==> 65-38 = 27)
            //Lowercase - Subtract 96 to bring it in the proper range (a = 97d ==> 97-96 = 1)
            prioritySum += char.IsAsciiLetterUpper(item) ? (int)item - 38 : (int)item - 96;
        }

        //Attempt 1: 2585 (correct)
        return prioritySum.ToString();
    }
}
