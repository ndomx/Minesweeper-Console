using System;

namespace minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            GameSetup();
            Board.Print();
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
    }
}
