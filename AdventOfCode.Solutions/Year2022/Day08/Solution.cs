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
        SetDefaultVisibleTrees(TheGrid);    //99x99 = 392 edge trees
        MarkVisibleTrees(TheGrid);

        //Attempt 1: 197 (incorrect - too low)  - limit values for the for-loops not initialized in MarkVisibleTrees ==> was only counting two edges of trees
        //Attempt 2: 1428 (incorrect - too low) - changed default value of Visible for row and col (off-by-one)
        //Attempt 3: 7096 (incorrect - too high) - not checking values from edge to point, only surrounding points
        //Attempt 4: 1991 (incorrect) - re-evaluating CheckX methods for proper usage of X-Y axis' and for-loop limits. Removed setting 9 as the tallest tree as visible by default (another tallest 9 could be at the edge blocking another 9)
        //Attempt 5: 1733 (correct)
        return TheGrid.Where(x => x.Value.Visible).Count().ToString();
    }



    protected override string SolvePartTwo()
    {
        Dictionary<Point, Tree> TheGrid = new();
        LoadTheGrid(TheGrid);
        SetDefaultVisibleTrees(TheGrid);    //99x99 = 392 edge trees
        MarkVisibleTrees(TheGrid);

        int maxScenicScore = -1;
        foreach (Point item in TheGrid.Keys)
        {
            maxScenicScore = TheGrid[item].ScenicScore > maxScenicScore ? TheGrid[item].ScenicScore : maxScenicScore;
        }

        //Attempt 1: 284648 (correct)
        return maxScenicScore.ToString();
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
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.X.Equals(treeGrid.Select(x => x.Key.X).Max())).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }

        //the last column(col = 98)
        foreach (Point p in treeGrid.Select(x => x.Key).Where(x => x.Y.Equals(treeGrid.Select(x => x.Key.Y).Max())).ToList())
        {
            treeGrid[p].Visible = true; // set visibile to true by default
        }

    }

    private void MarkVisibleTrees(Dictionary<Point, Tree> treeGrid)
    {
        int largestX = treeGrid.Select(x => x.Key.X).Max();
        int largestY = treeGrid.Select(x => x.Key.Y).Max();

        for (int x = 0; x <= largestX; x++)
        {
            for (int y = 0; y <= largestY; y++)
            {
                Point current = new Point(x, y);

                //If the tree is already marked as visible, skip checking it
                if (treeGrid[current].Visible == false)
                {
                    bool left = CheckLeft(treeGrid, treeGrid[current]);
                    bool right = CheckRight(treeGrid, treeGrid[current]);
                    bool top = CheckTop(treeGrid, treeGrid[current]);
                    bool down = CheckDown(treeGrid, treeGrid[current]);
                    treeGrid[current].Visible = left || right || top || down;
                }
            }
        }
    }

    private bool CheckLeft(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = -1;
        bool blocked = false;

        //X-AXIS = UP DOWN :: Y-AXIS = LEFT RIGHT
        //Start Y at 0 and move from 0 to current Y location
        for (int i = current.Location.Y - 1; i >= 0; i--)
        {
            //X stays the same
            Point pt = new Point(current.Location.X, i);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;

            if (!blocked)
            {
                treeGrid[current.Location].TreesVisibleLeft++;
                blocked = treeGrid[pt].Height >= treeGrid[current.Location].Height;
            }
        }
        return current.Height > tallestFound;
    }

    private bool CheckRight(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = -1;
        bool blocked = false;
        int furthestPt = treeGrid.Select(x => x.Key.Y).Max();

        //X-AXIS = UP DOWN :: Y-AXIS = LEFT RIGHT
        //Start Y at current Y location + 1 and move to the end of row
        for (int i = current.Location.Y + 1; i <= furthestPt; i++)
        {
            //X stays the same
            Point pt = new Point(current.Location.X, i);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;

            if (!blocked)
            {
                treeGrid[current.Location].TreesVisibleRight++;
                blocked = treeGrid[pt].Height >= treeGrid[current.Location].Height;
            }
        }
        return current.Height > tallestFound;
    }

    private bool CheckTop(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = -1;
        bool blocked = false;
        //X-AXIS = UP DOWN :: Y-AXIS = LEFT RIGHT
        //Start X at 0 and move from 0 to current X location
        for (int i = current.Location.X - 1; i >= 0; i--)
        {
            //Y stays the same
            Point pt = new Point(i, current.Location.Y);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;

            if (!blocked)
            {
                treeGrid[current.Location].TreesVisibleTop++;
                blocked = treeGrid[pt].Height >= treeGrid[current.Location].Height;
            }
        }
        return current.Height > tallestFound;
    }

    private bool CheckDown(Dictionary<Point, Tree> treeGrid, Tree current)
    {
        int tallestFound = -1;
        bool blocked = false;
        int furthestPt = treeGrid.Select(x => x.Key.Y).Max();

        //X-AXIS = UP DOWN :: Y-AXIS = LEFT RIGHT
        //Start X at current X location + 1 and move to the bottom of column
        for (int i = current.Location.X + 1; i <= furthestPt; i++)
        {
            //Y stays the same
            Point pt = new Point(i, current.Location.Y);
            tallestFound = treeGrid[pt].Height > tallestFound ? treeGrid[pt].Height : tallestFound;

            if (!blocked)
            {
                treeGrid[current.Location].TreesVisibleBottom++;
                blocked = treeGrid[pt].Height >= treeGrid[current.Location].Height;
            }

        }
        return current.Height > tallestFound;
    }

    private class Tree
    {
        public Point Location { get; set; }
        public int Height { get; set; } = -1;
        public bool Visible { get; set; } = false;
        public int TreesVisibleLeft { get; set; } = 0;
        public int TreesVisibleRight { get; set; } = 0;
        public int TreesVisibleTop { get; set; } = 0;
        public int TreesVisibleBottom { get; set; } = 0;
        public int ScenicScore { get { return TreesVisibleLeft * TreesVisibleRight * TreesVisibleTop * TreesVisibleBottom; } }
    }
}
