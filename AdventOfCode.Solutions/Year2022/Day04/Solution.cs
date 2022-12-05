namespace AdventOfCode.Solutions.Year2022.Day04;

class Solution : SolutionBase
{
    List<ElfPair> elfPairs = new List<ElfPair>();

    public Solution() : base(04, 2022, "") { GetElves();}

    protected override string SolvePartOne()
    {
        //Attempt 1: 602 (correct)
        return elfPairs.Where(x => x.Within == true).Select(x => x).Count().ToString();
    }

    protected override string SolvePartTwo()
    {
        //Attempt 1: 891 (correct)
        return elfPairs.Where(x => x.Overlap == true).Select(x => x).Count().ToString();
    }

    public void GetElves()
    {
        //Split the Input
        List<string> lines = Input.SplitByNewline().ToList();

        foreach (string line in lines)
        {
            //Split on the comma to get the individual elf range
            List<string> elves = line.Split(',').ToList();
            Elf firstElf = new Elf(elves.FirstOrDefault());
            Elf secondElf = new Elf(elves.LastOrDefault());
            ElfPair pair = new ElfPair(firstElf, secondElf);
            elfPairs.Add(pair);
        }
    }

    class Elf
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Range { get { return End - Start; } }

        public Elf(string range)
        {
            List<string> sp = range.Split('-').ToList();
            int.TryParse(sp.FirstOrDefault(), out int sStart);
            int.TryParse(sp.LastOrDefault(), out int sEnd);
            Start = sStart;
            End = sEnd;
        }
    }

    class ElfPair
    {
        public Elf E1 { get; set; }
        public Elf E2 { get; set; }
        public bool Within { get { return (E1.Start >= E2.Start && E1.End <= E2.End || E2.Start >= E1.Start && E2.End <= E1.End); } }
        public bool Overlap { get { return (E1.Start <= E2.End && E2.Start <= E1.End); } }

        public ElfPair(Elf e1, Elf e2)
        {
            E1 = e1;
            E2 = e2;
        }
    }
}
