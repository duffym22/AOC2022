using System.Linq;

namespace AdventOfCode.Solutions.Year2022.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2022, "") { }

    Dictionary<int, int> elfCalorieCount = new Dictionary<int, int>();

    protected override string SolvePartOne()
    {
        try
        {
            
            //double newlines will be filtered out so this list will contain each elf's individual inventory of calories
            string[] working = Input.SplitByParagraph();                

            for (int i = 0; i < working.Length; i++)
            {
                //splitting on newline again will get the calorie value of the individual inventory item 
                //parse all values as int and output as a list
                List<int> splitCalorieValues = working[i].Split('\n').Where(x => string.IsNullOrEmpty(x) == false).Select(x => Int32.Parse(x)).ToList();
                //add individual elf ID (for later) and sum of all calorie values in the splitCalorieValues
                elfCalorieCount.Add(i, splitCalorieValues.Sum());
            }

            //return the max value (largest calorie) from the dictionary
            return elfCalorieCount.Values.Max().ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
        }

    }

    protected override string SolvePartTwo()
    {
        List<int> sortedElfCalories = elfCalorieCount.Values.ToList();
        sortedElfCalories.Sort();
        sortedElfCalories.Reverse();
        return sortedElfCalories.GetRange(0,3).Sum().ToString();
    }
}
