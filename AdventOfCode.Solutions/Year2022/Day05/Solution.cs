using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2022.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2022, "") { }

    private void ParseInput(out List<string> stackConfig, out List<string> instructions, out Dictionary<int, Stack<char>> stacks)
    {
        List<string> lines = Input.SplitByNewline().ToList();
        stackConfig = new();
        instructions = new();
        stacks = new();

        //The first 9 lines is the initial stack configuration
        stackConfig = lines.GetRange(0, 9);
        instructions = lines.GetRange(9, lines.Count - stackConfig.Count);
        //Remove the last line that indicates the column for the stack, we don't need it
        stackConfig.RemoveAt(8);

        //Initialize the Dictionary
        for (int i = 0; i < 9; i++)
        {
            stacks.Add(i, new());
        }

        //Parse the characters of the stack
        foreach (string item in stackConfig)
        {
            //Input is nicely separated by 4 characters
            List<string> splits = item.SplitEvery(4);
            //Remove the brackets '[' ']' and trim the whitespace
            splits = splits.Select(x => x.Replace("[", "").Replace("]", "").Trim()).ToList();
            for (int i = 0; i < splits.Count; i++)
            {
                //Check for whitespace and only add non empty values
                if (string.IsNullOrEmpty(splits[i]) == false)
                {
                    char c = char.Parse(splits[i]);
                    stacks[i].Push(c);
                }
            }
        }

        //Once all the stacks are populated in the Dictionary, reverse them to get the proper input order (bottom goes on top, top goes on bottom)
        for (int i = 0; i < stacks.Count; i++)
        {
            Stack<char> temp = new();
            //Pop'm to the temp stack
            while (stacks[i].Count != 0)
                temp.Push(stacks[i].Pop());

            //Re-allocate temp stack to OG stack - voila, reversed
            stacks[i] = temp;
        }

    }

    private void MoveCrate(Dictionary<int, Stack<char>> stacks, int quantity, int fromStack, int toStack)
    {
        for (int i = 0; i < quantity; i++)
        {
            stacks[toStack].Push(stacks[fromStack].Pop());
        }
    }

    private void MoveCrateKeepOrder(Dictionary<int, Stack<char>> stacks, int quantity, int fromStack, int toStack)
    {
        Stack<char>
            temp = new(),
            temp2 = new();

        //Pop'm
        for (int i = 0; i < quantity; i++)
        {
            temp.Push(stacks[fromStack].Pop());
        }

        //Reverse'm & Push
        while (temp.Count != 0)
            stacks[toStack].Push(temp.Pop());
    }

    protected override string SolvePartOne()
    {
        List<string> instructions = new();
        List<string> stackConfig = new();
        Dictionary<int, Stack<char>> stacks = new();
        ParseInput(out stackConfig, out instructions, out stacks);

        foreach (string item in instructions)
        {
            //Remove unnecessary words from string, make double spaces into single space for splitting purposes
            string inst = item.ToLower().Replace("move", "").Replace("from", "").Replace("to", "").Replace("  ", " ").Trim();
            //Split it to get the numbers
            List<string> instNums = inst.Split(" ").ToList();
            //Pass the numeric parameters to MoveCrate but because the Dictionary is zero based and the Input starts at 1
            //pass the value minus one to pop/push from the proper stacks
            MoveCrate(stacks, int.Parse(instNums[0]), int.Parse(instNums[1]) - 1, int.Parse(instNums[2]) - 1);
        }


        //Attempt 1: PWPWHGFZS (incorrect) - removed use of tempMov in MoveCrate and just pushed directly what was popped
        //Attempt 2: FWSHSPJWM (correct)
        return string.Concat(stacks.Select(x => x.Value.Pop()));
    }

    protected override string SolvePartTwo()
    {
        List<string> instructions = new();
        List<string> stackConfig = new();
        Dictionary<int, Stack<char>> stacks = new();
        ParseInput(out stackConfig, out instructions, out stacks);

        foreach (string item in instructions)
        {
            //Remove unnecessary words from string, make double spaces into single space for splitting purposes
            string inst = item.ToLower().Replace("move", "").Replace("from", "").Replace("to", "").Replace("  ", " ").Trim();
            //Split it to get the numbers
            List<string> instNums = inst.Split(" ").ToList();
            //Pass the numeric parameters to MoveCrate but because the Dictionary is zero based and the Input starts at 1
            //pass the value minus one to pop/push from the proper stacks
            MoveCrateKeepOrder(stacks, int.Parse(instNums[0]), int.Parse(instNums[1]) - 1, int.Parse(instNums[2]) - 1);
        }

        //Ironically made MoveCrateKeepOrder which re-implemented use of temp stack to reverse pop order
        //Attempt 1: PWPWHGFZS (from part 1!)
        return string.Concat(stacks.Select(x => x.Value.Pop()));
    }
}
