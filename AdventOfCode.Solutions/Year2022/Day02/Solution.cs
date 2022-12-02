using System.ComponentModel;

namespace AdventOfCode.Solutions.Year2022.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2022, "") { }

    enum OPPONENT_HAND
    {
        [Description("A")]
        ROCK = 1,
        [Description("B")]
        PAPER = 2,
        [Description("C")]
        SCISSORS = 3
    }

    enum PLAYER_HAND
    {
        [Description("X")]
        ROCK = 1,
        [Description("Y")]
        PAPER = 2,
        [Description("Z")]
        SCISSORS = 3
    }

    enum EXPECTED_PLAYER_OUTCOME
    {
        [Description("X")]
        LOSS = 1,
        [Description("Y")]
        DRAW = 2,
        [Description("Z")]
        WIN = 3
    }

    enum OUTCOME
    {
        LOSS = 0,
        DRAW = 3,
        WIN = 6
    }

    protected override string SolvePartOne()
    {
        var opponent = (Wins: 0, Losses: 0, Draws: 0, Score: 0);
        var player = (Wins: 0, Losses: 0, Draws: 0, Score: 0);

        List<string> lines = Input.SplitByNewline().ToList();
        foreach (string line in lines)
        {
            string[] hands = line.Split(' ');
            OPPONENT_HAND opponentHand;
            //Opponent hand is first
            opponentHand = 
                hands[0] == EnumUtils.GetDescription(OPPONENT_HAND.ROCK)
                ? OPPONENT_HAND.ROCK
                : hands[0] == EnumUtils.GetDescription(OPPONENT_HAND.PAPER)
                ? OPPONENT_HAND.PAPER
                : OPPONENT_HAND.SCISSORS;
            opponent.Score += (int)opponentHand;

            //Player hand is second
            PLAYER_HAND playerHand;
            playerHand =
                hands[1] == EnumUtils.GetDescription(PLAYER_HAND.ROCK)
                ? PLAYER_HAND.ROCK
                : hands[1] == EnumUtils.GetDescription(PLAYER_HAND.PAPER)
                ? PLAYER_HAND.PAPER
                : PLAYER_HAND.SCISSORS;
            player.Score += (int)playerHand;

            if (opponentHand == OPPONENT_HAND.ROCK && playerHand == PLAYER_HAND.ROCK ||
                opponentHand == OPPONENT_HAND.PAPER && playerHand == PLAYER_HAND.PAPER ||
                opponentHand == OPPONENT_HAND.SCISSORS && playerHand == PLAYER_HAND.SCISSORS)
            {
                //Draw
                opponent.Score += (int)OUTCOME.DRAW;
                opponent.Draws += 1;
                player.Score += (int)OUTCOME.DRAW;
                player.Draws += 1;

            }
            else if (
                opponentHand == OPPONENT_HAND.ROCK && playerHand == PLAYER_HAND.SCISSORS ||
                opponentHand == OPPONENT_HAND.PAPER && playerHand == PLAYER_HAND.ROCK ||
                opponentHand == OPPONENT_HAND.SCISSORS && playerHand == PLAYER_HAND.PAPER)
            {
                //Opponent Win
                opponent.Score += (int)OUTCOME.WIN;
                opponent.Wins += 1;
                player.Score += (int)OUTCOME.LOSS;
                player.Losses += 1;
            }
            else if (
                playerHand == PLAYER_HAND.ROCK && opponentHand == OPPONENT_HAND.SCISSORS ||
                playerHand == PLAYER_HAND.PAPER && opponentHand == OPPONENT_HAND.ROCK ||
                playerHand == PLAYER_HAND.SCISSORS && opponentHand == OPPONENT_HAND.PAPER)
            {
                //Player Win
                player.Score += (int)OUTCOME.WIN;
                player.Wins += 1;
                opponent.Score += (int)OUTCOME.LOSS;
                opponent.Losses += 1;
            }
        }

        Console.WriteLine($"Player :: Wins [{player.Wins}] Losses [{player.Losses}] Draws [{player.Draws}] Score [{player.Score}]");
        Console.WriteLine($"Opponent :: Wins [{opponent.Wins}] Losses [{opponent.Losses}] Draws [{opponent.Draws}] Score [{opponent.Score}]");

        //Attempt 1: 15937 (too high) - LOSS enum was set to 1 instead of 0
        //Attempt 2: 15523 (-414) - Correct

        return player.Score.ToString();
    }

    protected override string SolvePartTwo()
    {
        var opponent = (Wins: 0, Losses: 0, Draws: 0, Score: 0);
        var player = (Wins: 0, Losses: 0, Draws: 0, Score: 0);

        List<string> lines = Input.SplitByNewline().ToList();
        foreach (string line in lines)
        {
            string[] hands = line.Split(' ');

            OPPONENT_HAND opponentHand;
            //Opponent hand is first
            opponentHand =
                hands[0] == EnumUtils.GetDescription(OPPONENT_HAND.ROCK)
                ? OPPONENT_HAND.ROCK
                : hands[0] == EnumUtils.GetDescription(OPPONENT_HAND.PAPER)
                ? OPPONENT_HAND.PAPER
                : OPPONENT_HAND.SCISSORS;

            //Player outcome is second
            EXPECTED_PLAYER_OUTCOME expPlayerOutcome;
            expPlayerOutcome =
                hands[1] == EnumUtils.GetDescription(EXPECTED_PLAYER_OUTCOME.WIN)
                ? EXPECTED_PLAYER_OUTCOME.WIN
                : hands[1] == EnumUtils.GetDescription(EXPECTED_PLAYER_OUTCOME.DRAW)
                ? EXPECTED_PLAYER_OUTCOME.DRAW
                : EXPECTED_PLAYER_OUTCOME.LOSS;

            switch (expPlayerOutcome)
            {
                //Lose
                case EXPECTED_PLAYER_OUTCOME.LOSS:
                    opponent.Wins += 1;
                    player.Losses += 1;
                    opponent.Score = (int)OUTCOME.WIN + (int)opponentHand;
                    player.Score += (int)OUTCOME.LOSS;

                    //Determine hand to lose
                    switch (opponentHand)
                    {
                        case OPPONENT_HAND.ROCK:
                            //Scissors loses to Rock 
                            player.Score += (int)PLAYER_HAND.SCISSORS;
                            break;
                        case OPPONENT_HAND.PAPER:
                            //Rock loses to Paper 
                            player.Score += (int)PLAYER_HAND.ROCK;
                            break;
                        case OPPONENT_HAND.SCISSORS:
                            //Paper loses to Scissors
                            player.Score += (int)PLAYER_HAND.PAPER;
                            break;
                    }

                    break;

                //Draw
                case EXPECTED_PLAYER_OUTCOME.DRAW:
                    opponent.Draws += 1;
                    player.Draws += 1;
                    opponent.Score = (int)OUTCOME.DRAW + (int)opponentHand;
                    player.Score += (int)OUTCOME.DRAW;

                    //Determine hand to draw
                    switch (opponentHand)
                    {
                        case OPPONENT_HAND.ROCK:
                            //Rock - Rock 
                            player.Score += (int)PLAYER_HAND.ROCK;
                            break;
                        case OPPONENT_HAND.PAPER:
                            //Paper - Paper 
                            player.Score += (int)PLAYER_HAND.PAPER;
                            break;
                        case OPPONENT_HAND.SCISSORS:
                            //Scissors - Scissors
                            player.Score += (int)PLAYER_HAND.SCISSORS;
                            break;
                    }
                    break;

                //Win
                case EXPECTED_PLAYER_OUTCOME.WIN:
                    player.Wins += 1;
                    opponent.Losses += 1;
                    opponent.Score = (int)OUTCOME.LOSS + (int)opponentHand;
                    player.Score += (int)OUTCOME.WIN;

                    //Determine hand to win
                    switch (opponentHand)
                    {
                        case OPPONENT_HAND.ROCK:
                            //Paper beats Rock 
                            player.Score += (int)PLAYER_HAND.PAPER;
                            break;
                        case OPPONENT_HAND.PAPER:
                            //Scissors beats Paper 
                            player.Score += (int)PLAYER_HAND.SCISSORS;
                            break;
                        case OPPONENT_HAND.SCISSORS:
                            //Rock beats Scissors
                            player.Score += (int)PLAYER_HAND.ROCK;
                            break;
                    }
                    break;
            }


        }

        Console.WriteLine($"Player :: Wins [{player.Wins}] Losses [{player.Losses}] Draws [{player.Draws}] Score [{player.Score}]");
        Console.WriteLine($"Opponent :: Wins [{opponent.Wins}] Losses [{opponent.Losses}] Draws [{opponent.Draws}] Score [{opponent.Score}]");

        //Attempt 1: 15702
        return player.Score.ToString();
    }
}
