using System;

namespace Minesweeper
{
    class Program
    {
        static bool playerLose = false;
        static bool playerWin = false;

        static void Main(string[] args)
        {
            // GameSetup();
            Board.Initialize(5, 5, 0.1f);
            // Board.Print();
            // Board.Flip(0, 0, PlayerAction.DISCOVER);

            while (true)
            {
                PlayerTurn();
                if (playerLose)
                {
                    OnPlayerLose();
                    break;
                }

                if (playerWin)
                {
                    OnPlayerWin();
                    break;
                }
            }

            
        }

        static void GameSetup()
        {
            string input;
            int width, height;

            Console.Write("Number of rows: ");
            input = Console.ReadLine();
            if (!(int.TryParse(input, out height)))
            {
                throw new ArgumentException($"Invalid input: {input}");
            }

            Console.Write("Number of columns: ");
            input = Console.ReadLine();
            if (!(int.TryParse(input, out width)))
            {
                throw new ArgumentException($"Invalid input: {input}");
            }

            float bombDensity;
            Console.Write("Bomb density [0,1]: ");
            input = Console.ReadLine();
            if (!(float.TryParse(input, out bombDensity)))
            {
                throw new ArgumentException($"Invalid input: {input}");
            }

            if ((bombDensity < 0) || (bombDensity > 1))
            {
                throw new ArgumentOutOfRangeException("Bomb density out of range");
            }

            Board.Initialize(width, height, bombDensity);
        }

        static void PlayerTurn()
        {
            Board.Print();

            string input;
            int row, col;

            int remainingBombs = Board.BombCount - Board.MarkedBombs;
            Console.WriteLine("Remaining bombs: {0}", remainingBombs);
            Console.Write("Select row: ");
            input = Console.ReadLine();
            if (!(int.TryParse(input, out row)))
            {
                throw new ArgumentException($"Invalid input: {input}");
            }

            Console.Write("Select column: ");
            input = Console.ReadLine();
            if (!(int.TryParse(input, out col)))
            {
                throw new ArgumentException($"Invalid input: {input}");
            }

            PlayerAction action = GetActionInput();

            playerLose = Board.Flip(row, col, action);
            playerWin = (Board.RemainingBombs == 0);
        }

        static PlayerAction GetActionInput()
        {
            Console.Write("Select action (M: Mark, U: ?, else: Reveal): ");
            string input = Console.ReadLine();
            if (String.IsNullOrEmpty(input)) return PlayerAction.DISCOVER;

            switch (input.ToUpper()[0])
            {
                case 'M': return PlayerAction.MARK_BOMB;
                case 'U': return PlayerAction.MARK_UNDEFINED;
                default: return PlayerAction.DISCOVER;
            }
        }

        static void OnPlayerLose()
        {
            Board.FlipAll();
            Board.Print();
            Console.WriteLine("You Lost");
        }

        static void OnPlayerWin()
        {
            Board.FlipAll();
            Board.Print();
            Console.WriteLine("You win!");
        }
    }
}
