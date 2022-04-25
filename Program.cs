using System;

namespace minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Board.Initialize(10, 10, 0.5f);
            Board.Print();
        }
    }
}
