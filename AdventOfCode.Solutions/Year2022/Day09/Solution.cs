using System.Drawing;

namespace AdventOfCode.Solutions.Year2022.Day09;

class Solution : SolutionBase
{
    public Solution() : base(09, 2022, "") { }

    protected override string SolvePartOne()
    {
        Point head = new Point(0, 0);
        Point tail = new Point(0, 0);
        Dictionary<Point, int> tailPoints = new();
        List<string> lines = Input.SplitByNewline().ToList();

        for (int i = 0; i < lines.Count; i++)
        {
            string[] sp = lines[i].Split(' ');
            string direction = sp[0];               //[0] = Direction character
            int movementValue = int.Parse(sp[1]);   //[1] = Distance value

            //Move Head one step at a time
            //Update the tail after each step
            while (movementValue != 0)
            {
                //X = LEFT RIGHT
                //Y = UP DOWN
                switch (direction)
                {
                    case "U":
                        head.Y++;
                        break;
                    case "D":
                        head.Y--;
                        break;
                    case "L":
                        head.X--;
                        break;
                    case "R":
                        head.X++;
                        break;
                }
                movementValue--;
                UpdateTail(head, ref tail);
                //Check to see if we can add the current tail point
                //If so, add it, if not, update it
                if (tailPoints.TryAdd(tail, 1) == false)    //TryAdd will add if successful
                    tailPoints[tail]++;                     //Update if not successful

            }
        }

        //Attempt 1: 9129 (too high) - bug in UpdateTail - copy/paste error, after examining that X (column) was the same, was subtracting X values instead of Y values to determine if negative
        //Attempt 2: 6337 (correct)
        return tailPoints.Count.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<Point> knots = new List<Point>();
        Dictionary<Point, int> tailPoints = new();

        //Add 10 knots in total - the Head + 9
        for (int i = 0; i < 10; i++)
        {
            knots.Add(new Point(0, 0));
        }

        List<string> lines = Input.SplitByNewline().ToList();

        for (int i = 0; i < lines.Count; i++)
        {
            string[] sp = lines[i].Split(' ');
            string direction = sp[0];               //[0] = Direction character
            int movementValue = int.Parse(sp[1]);   //[1] = Distance value

            //Move Head one step at a time
            //Update the tail after each step
            while (movementValue != 0)
            {
                Point head = knots[0];
                //X = LEFT RIGHT
                //Y = UP DOWN
                switch (direction)
                {
                    case "U":
                        head.Y++;
                        break;
                    case "D":
                        head.Y--;
                        break;
                    case "L":
                        head.X--;
                        break;
                    case "R":
                        head.X++;
                        break;
                }
                knots[0] = head;    //Assign the head back to the list
                movementValue--;

                for (int j = 0; j < knots.Count - 1; j++)
                {
                    Point currHead = knots[j];
                    Point nextKnot = knots[j + 1];
                    UpdateTail(currHead, ref nextKnot);
                    knots[j + 1] = nextKnot;    //Assign it back to the list
                }

                //Check to see if we can add the current tail point
                //If so, add it, if not, update it
                if (tailPoints.TryAdd(knots.Last(), 1) == false)    //TryAdd will add if successful
                    tailPoints[knots.Last()]++;                     //Update if not successful

            }
        }

        //Attempt 1: 2455 (correct!)
        return tailPoints.Count.ToString();
    }

    private void UpdateTail(Point head, ref Point tail)
    {
        if (IsAdjacent(head, tail) == false)
        {
            //If both the head and tail row and columns values are not the same, move the tail diagonally
            if (tail.X != head.X && tail.Y != head.Y)
            {
                //Move on Y axis first
                if (head.Y - tail.Y < 0)
                    tail.Y--;   //  If head is BELOW tail (head - tail = negative), go down (-Y)
                else
                    tail.Y++;   //  If head is ABOVE tail (head - tail = positive), go up (+Y)

                if (head.X - tail.X < 0)
                    tail.X--;   //  If head is to the LEFT of tail (head - tail = negative), go left (-X)
                else
                    tail.X++;   //  If head is to the RIGHT of tail (head - tail = positive), go right (+X)

            }
            //If head and tail are in the same column (X)
            else if (tail.X == head.X)
            {
                if (head.Y - tail.Y < 0)
                    tail.Y--;   //  If head is BELOW tail (head - tail = negative), go down (-Y)
                else
                    tail.Y++;   //  If head is ABOVE tail (head - tail = positive), go up (+Y)
            }

            //If head and tail are in the same row (Y)
            else if (tail.Y == head.Y)
            {
                if (head.X - tail.X < 0)
                    tail.X--;   //  If head is to the LEFT of tail (head - tail = negative), go left (-X)
                else
                    tail.X++;   //  If head is to the RIGHT of tail (head - tail = positive), go right (+X)
            }
            else
            {
                Console.WriteLine("Uh oh");
            }
        }
    }

    private bool IsAdjacent(Point head, Point tail)
    {
        //Top-Left      = -X, +Y
        //Top-Center    =  0, +Y
        //Top-Right     = +X, +Y
        //Mid-Left      = -X, 0
        //Center        =  0, 0
        //Mid-Right     = +X, 0
        //Bot-Left      = -X, -Y
        //Bot-Center    =  0, -Y
        //Bot-Right     = +X, -Y

        return (tail.X - 1 == head.X && tail.Y + 1 == head.Y)       //Top Left
            || (tail.X == head.X && tail.Y + 1 == head.Y)           //Top Center
            || (tail.X + 1 == head.X && tail.Y + 1 == head.Y)       //Top Right
            || (tail.X - 1 == head.X && tail.Y == head.Y)           //Mid Left
            || (tail.X == head.X && tail.Y == head.Y)               //Center (Overlapping)
            || (tail.X + 1 == head.X && tail.Y == head.Y)           //Mid Right
            || (tail.X - 1 == head.X && tail.Y - 1 == head.Y)       //Bot Left
            || (tail.X == head.X && tail.Y - 1 == head.Y)           //Bot Center
            || (tail.X + 1 == head.X && tail.Y - 1 == head.Y);      //Bot Right
    }

    public enum DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

}
