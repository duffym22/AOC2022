using System.Diagnostics;

namespace AdventOfCode.Solutions.Year2022.Day11;

class Solution : SolutionBase
{
    public Solution() : base(11, 2022, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByParagraph().ToList();
        List<Monkey> ruffians = GimmeDaMonkeys(lines);
        Stopwatch roundTimer = new Stopwatch();
        roundTimer.Start();
        //Go for 20 rounds
        for (int i = 0; i < 20; i++)
        {
            Stopwatch monkeyTmr = new Stopwatch();
            monkeyTmr.Start();
            //During each round, each monkey gets a turn
            foreach (Monkey currMoonk in ruffians)
            {
                currMoonk.GenerateWorry();
                currMoonk.ThrowItems(ref ruffians);
                Console.WriteLine($"Round {i} - Monkey {currMoonk.Number}: {monkeyTmr.Elapsed} ms");
                monkeyTmr.Restart();
            }
            Console.WriteLine($"Round {i}: {roundTimer.ElapsedMilliseconds} ms");
            roundTimer.Restart();
        }

        List<int> monkeyHoarders = ruffians.Select(x => x.CurrentItems.Count).OrderBy(y => y).ToList();
        monkeyHoarders.Reverse();
        Monkey m1 = (Monkey)ruffians.Select(x => x).Where(x => x.CurrentItems.Count == monkeyHoarders[0]).First();
        Monkey m2 = (Monkey)ruffians.Select(x => x).Where(x => x.CurrentItems.Count == monkeyHoarders[1]).First();

        //Attempt 1: 53100 (too low)
        return (m1.ItemsInspected * m2.ItemsInspected).ToString();
    }

    protected override string SolvePartTwo()
    {
        return "";
    }

    private List<Monkey> GimmeDaMonkeys(List<string> lines)
    {
        List<Monkey> BarrelOMonkeys = new List<Monkey>();
        //List of monkeys
        foreach (string item in lines)
        {
            List<string> munk = item.SplitByNewline().ToList();

            //Monkey details
            Monkey m = new Monkey();
            for (int i = 0; i < munk.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        //Ex. Monkey x:
                        m.Number = int.Parse(munk[i].Split(" ").Last().Replace(":", ""));
                        break;
                    case 1:
                        //Ex. Starting items: 79, 98
                        string[] start = munk[i].Split(":");
                        //Ex. 79, 98
                        string[] s = start[1].Split(",");
                        foreach (string thing in s)
                        {
                            m.CurrentItems.Add(int.Parse(thing));
                        }
                        break;
                    case 2:
                        //Operation: new = old * 19
                        //Ex. Operation: new = old * 19
                        string[] op = munk[i].Split("=");
                        //Ex. old * 19
                        if (op[1].Contains("* old"))
                        {
                            m.ExecuteOperation = OPERATION.SQUARE;
                        }
                        else if (op[1].Contains("+"))
                        {
                            m.ExecuteOperation = OPERATION.ADD;
                            m.OperationValue = int.Parse(op[1].Split("+").Last());
                        }
                        else
                        {
                            m.ExecuteOperation = OPERATION.MULTIPLY;
                            m.OperationValue = int.Parse(op[1].Split("*").Last());
                        }
                        break;
                    case 3:
                        //Test: divisible by 23
                        m.TestValue = int.Parse(munk[i].Split("by").Last());
                        break;
                    case 4:
                        //If true: throw to monkey 2
                        m.TrueMonkey = int.Parse(munk[i].Split("monkey").Last().Split(" ").Last());
                        break;
                    case 5:
                        //If false: throw to monkey 3
                        m.FalseMonkey = int.Parse(munk[i].Split("monkey").Last().Split(" ").Last());
                        break;
                }
            }
            BarrelOMonkeys.Add(m);
        }
        return BarrelOMonkeys;
    }
}

public class Monkey
{
    public int Number { get; set; } = -1;
    public int ItemsInspected { get; set; } = 0;
    public List<int> CurrentItems { get; set; } = new List<int>();
    public OPERATION ExecuteOperation { get; set; }
    public int OperationValue { get; set; } = -1;
    public int TestValue { get; set; } = -1;
    public int TrueMonkey { get; set; } = -1;
    public List<int> TrueItems { get; set; } = new List<int>();
    public int FalseMonkey { get; set; } = -1;
    public List<int> FalseItems { get; set; } = new List<int>();
    public Monkey() { }

    public void GenerateWorry()
    {
        foreach (int item in CurrentItems)
        {
            //Monkey Inspection
            ItemsInspected++;
            int newWorry = 0;
            switch (ExecuteOperation)
            {
                case OPERATION.ADD:
                    newWorry = item + OperationValue;
                    break;
                case OPERATION.MULTIPLY:
                    newWorry = item * OperationValue;
                    break;
                case OPERATION.SQUARE:
                    newWorry = item * item;
                    break;
            }

            //Post-Monkey Inspection, divide worry level by 3
            double div = newWorry / 3;
            newWorry = Convert.ToInt32(Math.Floor(div));

            //Check of modulus of TestValue is 0 (cleanly divisible)
            if (newWorry % TestValue == 0)
            {
                TrueItems.Add(newWorry);
            }
            else
                FalseItems.Add(newWorry);
        }
    }

    public void ThrowItems(ref List<Monkey> otherMonkeys)
    {
        //Throw true items
        Monkey tm = (Monkey)otherMonkeys.Where(x => x.Number == TrueMonkey).First();
        foreach (int throwItem in TrueItems)
        {
            tm.CurrentItems.Add(throwItem);
        }

        //Throw false items
        Monkey fm = (Monkey)otherMonkeys.Where(x => x.Number == FalseMonkey).First();
        foreach (int throwItem in FalseItems)
        {
            fm.CurrentItems.Add(throwItem);
        }

        CurrentItems = new();   //reset
        TrueItems = new();      //reset
        FalseItems = new();     //reset
    }
}

public enum OPERATION
{
    ADD,
    MULTIPLY,
    SQUARE
};
