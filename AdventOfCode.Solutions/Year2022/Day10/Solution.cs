using Microsoft.Win32;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;

namespace AdventOfCode.Solutions.Year2022.Day10;

class Solution : SolutionBase
{
    public Solution() : base(10, 2022, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<int> cycleValues = new();
        int registerX = 1;

        foreach (string item in lines)
        {
            ProcessOperations(item, ref registerX, ref cycleValues);
        }

        //Attempt 1: 14160 (correct)
        return CalculateCycleSums(cycleValues);

    }
    protected override string SolvePartTwo()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<int> cycleValues = new();
        int registerX = 1;
        string print = "";

        for (int i = 0; i < lines.Count; i++)
        {
            ProcessOperations_P2(lines[i], ref print, ref registerX, ref cycleValues);
            if (print.Length >= 40)
            {
                string sub = print.Substring(0, 40);
                Console.WriteLine(sub);
                print = print.Substring(40, print.Length - 40);
            }
        }

        //Console printed: RJERPEFC - correct!
        return "RJERPEFC";
    }

    private void ProcessOperations(string item, ref int registerX, ref List<int> cycleValues)
    {

        bool
           addxUnblocked = false,
           addxExecuting = true;

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

    private void ProcessOperations_P2(string item, ref string print, ref int registerX, ref List<int> cycleValues)
    {
        bool addxUnblocked = false;

        if (item.Contains("noop"))
        {
            //noop - 1 cycle
            print += DrawSprite(registerX, cycleValues.Count);
            cycleValues.Add(registerX);
        }
        else
        {
            //addx - 2 cycles
            bool addxExecuting = true;
            while (addxExecuting)
            {
                if (addxUnblocked)
                {
                    //addx - Cycle 2
                    // 1. Parse addx value
                    // 2. Add current value of registerX to cycleValues list
                    // 3. Update registerX with parsed addx value
                    int opVal = int.Parse(item.Split(' ').Last());
                    print += DrawSprite(registerX, cycleValues.Count);
                    cycleValues.Add(registerX);
                    registerX += opVal;
                    addxUnblocked = false;
                    addxExecuting = false;
                }
                else
                {
                    //Addx Cycle 1
                    // 1. Add current value of registerX to cycleValues list
                    print += DrawSprite(registerX, cycleValues.Count);
                    cycleValues.Add(registerX);
                    addxUnblocked = true;
                }
            }
        }

    }

    private string DrawSprite(int registerX, int cycle)
    {
        int temp = cycle;
        if (cycle >= 41 && cycle <= 80) temp -= 40;
        else if (cycle >= 81 && cycle <= 120) temp -= 80;
        else if (cycle >= 121 && cycle <= 160) temp -= 120;
        else if (cycle >= 161 && cycle <= 200) temp -= 160;
        else if (cycle >= 201) temp -= 200;
        return ((registerX - 1 == temp) || (registerX == temp) || (registerX + 1 == temp)) ? "#" : ".";
    }

    private string CalculateCycleSums(List<int> cycleValues)
    {
        int cycleSums = 0;
        for (int i = 20; i < 221; i += 40)
        {
            int v = cycleValues[i - 1];
            int product = i * v;
            cycleSums += product;
        }
        return cycleSums.ToString();
    }


}
