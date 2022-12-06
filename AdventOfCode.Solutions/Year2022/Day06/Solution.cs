using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2022.Day06;

class Solution : SolutionBase
{
    public Solution() : base(06, 2022, "") { }

    protected override string SolvePartOne()
    {
        List<string> buffer = Input.SplitEvery(1).ToList();
        List<char> last4Chars = new List<char>();
        int idx = 0;

        for (int i = 0; i < buffer.Count; i++)
        {
            if (i < 4)
            {
                last4Chars.Add((char.Parse(buffer[i])));
            }
            else
            {
                last4Chars.RemoveAt(0);
                last4Chars.Add(char.Parse(buffer[i]));
                var query = last4Chars.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (query.Count == 0)
                {
                    idx = i + 1;
                    break;
                }
            }

        }

        //Attempt 1: 1793 (incorrect) - off by one error - my array starts at 0 not 1
        //Attempt 2: 1794 (correct)
        return idx.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> buffer = Input.SplitEvery(1).ToList();
        List<char> last4Chars = new List<char>();
        int idx = 0;

        for (int i = 0; i < buffer.Count; i++)
        {
            if (i < 14)
            {
                last4Chars.Add((char.Parse(buffer[i])));
            }
            else
            {
                last4Chars.RemoveAt(0);
                last4Chars.Add(char.Parse(buffer[i]));
                var query = last4Chars.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (query.Count == 0)
                {
                    idx = i + 1;
                    break;
                }
            }
        }

        //Attempt 1: 2851 (correct)
        return idx.ToString();
    }
}
