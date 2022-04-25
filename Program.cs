using System;

namespace minesweeper
{
    class Program
    {
        static bool playerLose = false;

        static void Main(string[] args)
        {
            GameSetup();
            while (!playerLose)
            {
                PlayerTurn();
            }

            Board.FlipAll();
            Board.Print();
            Console.WriteLine("You Lost");
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

            playerLose = Board.Flip(row, col);
        }
    }
}
