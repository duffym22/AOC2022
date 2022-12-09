using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Solutions.Year2022.Day08;

class Solution : SolutionBase
{
    public Solution() : base(08, 2022, "") { }

    protected override string SolvePartOne()
    {
        Dictionary<Point, Tree> TheGrid = new();
        LoadTheGrid(TheGrid);
        SetDefaultVisibleTrees(TheGrid);    //98x98 = 342 perimeter trees + 425 tallest trees (height = 9) :: 817
        MarkVisibleTrees(TheGrid);

        //Attempt 1: 197 (incorrect - too low)  - limit values for the for-loops not initialized in MarkVisibleTrees ==> was only counting two edges of trees
        //Attempt 2: 1428 (incorrect - too low) - changed default value of Visible for row and col (off-by-one)
        //Attempt 3: 7096 (incorrect - too high) - not checking values from edge to point, only surrounding points
        return TheGrid.Where(x => x.Value.Visible).Count().ToString();
    }



    protected override string SolvePartTwo()
    {
        return "";
    }

    private void LoadTheGrid(Dictionary<Point, Tree> grid)
    {
        List<string> lines = Input.SplitByNewline().ToList();
        for (int rowCtr = 0; rowCtr < lines.Count; rowCtr++)
        {
            //Split the current line into individual numbers
            List<string> trees = lines[rowCtr].SplitEvery(1);
            for (int colCtr = 0; colCtr < trees.Count; colCtr++)
            {
                Point temp = new Point(rowCtr, colCtr);
                grid.Add(temp, new Tree() { Location = temp, Height = int.Parse(trees[colCtr]), Visible = false }); ;
            }
        }
    }

    private void MarkVisibleTrees(Dictionary<Point, Tree> treeGrid)
    {
        int largestX = treeGrid.Select(x => x.Key.X).Max();
        int largestY = treeGrid.Select(x => x.Key.Y).Max();

        for (int y = 0; y < largestX; y++)
        {
            for (int x = 0; x < largestY; x++)
            {
                Point current = new Point(x, y);

                //If the tree is already marked as visible, skip checking it
                if (treeGrid[current].Visible == false)
                {
                    treeGrid[current].Visible = (CheckLeft(treeGrid, treeGrid[current]) || CheckRight(treeGrid, treeGrid[current]) || CheckUp(treeGrid, treeGrid[current]) || CheckDown(treeGrid, treeGrid[current]));
                }
            }
        }
    }

    private void SetDefaultVisibleTrees(Dictionary<Point, Tree> treeGrid)
    {
        //For trees in the first row (row = 0)
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.X.Equals(0)).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }
        //the first column (col = 0)
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.Y.Equals(0)).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }
        //the last row(row = 99)
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.X.Equals(98)).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }
        //the last column(col = 98)
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.Y.Equals(98)).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }
        //Any tree whose height is already a 9 (tallest)
        foreach (Tree p in treeGrid.Select(x => x.Value).Where(x => x.Height.Equals(9)).ToList())
        {
            p.Visible = true; // set visibile to true by default
        }
    }

    private bool CheckLeft(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = current.Height;
        for (int i = 0; i < current.Location.X; i++)
        {
            Point pt = new Point(i, current.Location.Y);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;
        }
        return current.Height > tallestFound;
    }

    private bool CheckRight(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = current.Height;
        for (int i = current.Location.X + 1; i < 99; i++)
        {
            Point pt = new Point(i, current.Location.Y);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;
        }
        return current.Height > tallestFound;
    }

    private bool CheckUp(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = current.Height;

        for (int i = 0; i < current.Location.Y; i++)
        {
            Point pt = new Point(current.Location.X, i);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;
        }
        return current.Height > tallestFound;
    }

    private bool CheckDown(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = current.Height;

        for (int i = current.Location.Y + 1; i < 99; i++)
        {
            Point pt = new Point(current.Location.X, i);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;

        }
        return current.Height > tallestFound;
    }

    private class Tree
    {
        public Point Location { get; set; }
        public int Height { get; set; }
        public bool Visible { get; set; }
    }
}
