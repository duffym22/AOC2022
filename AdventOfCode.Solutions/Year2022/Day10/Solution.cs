using Microsoft.Win32;
using System.Net.Http.Headers;

namespace AdventOfCode.Solutions.Year2022.Day10;

class Solution : SolutionBase
{
    public Solution() : base(10, 2022, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<int> cycleValues = new();

        int registerX = 1;

        bool
           addxUnblocked = false,
           addxExecuting = true;

        foreach (string item in lines)
        {
            if (item.Contains("noop"))
            {
                //noop - 1 cycle
                cycleValues.Add(registerX);
            }
            else
            {
                //addx - 2 cycles
                addxExecuting = true;
                while (addxExecuting)
                {
                    if (addxUnblocked)
                    {
                        int opVal = int.Parse(item.Split(' ').Last());
                        cycleValues.Add(registerX);
                        registerX += opVal;
                        addxUnblocked = false;
                        addxExecuting = false;
                    }
                    else
                    {
                        cycleValues.Add(registerX);
                        addxUnblocked = true;
                    }
                }
            }
        }

        int cycleSums = 0;
        for (int i = 20; i < 221; i += 40)
        {
            int v = cycleValues[i - 1];
            int product = i * v;
            cycleSums += product;
        }

        //Attempt 1: 14160 (correct)
        return cycleSums.ToString();

    }

    protected override string SolvePartTwo()
    {
        return "";
    }
}
