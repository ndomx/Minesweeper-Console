using System;
using System.Text;

namespace minesweeper
{
    public static class Board
    {
        private static Random r = new Random();
        private static ConsoleColor DefaultBackground = Console.BackgroundColor;
        private static ConsoleColor DefaultForeground = Console.ForegroundColor;

        private static int width, height;
        private static float bombDensity;

        private static SpaceBase[,] spaces;

        public static void Initialize(int w, int h, float bombs)
        {
            width = w;
            height = h;
            bombDensity = bombs;

            GenerateBoard();
        }

        public static void Print()
        {
            int lineSize = (4 * width) + 1;
            string bounds = new String('-', lineSize);

            Console.WriteLine(bounds);
            for (int y = 0; y < height; y++)
            {
                Console.Write("|");
                for (int x = 0; x < width; x++)
                {
                    SpaceBase s = spaces[y, x];
                    Console.BackgroundColor = s.GetBackgroundColor();
                    Console.Write(" {0} ", s);
                    Console.BackgroundColor = DefaultBackground;
                    Console.Write("|");
                }

                Console.WriteLine();
                Console.WriteLine(bounds);
            }
        }

        public static bool Flip(int row, int col)
        {
            if ((row < 0) || (row >= height))
            {
                throw new ArgumentOutOfRangeException("Row out of range");
            }

            if ((col < 0) || (col >= width))
            {
                throw new ArgumentOutOfRangeException("Column out of range");
            }

            spaces[row, col].IsHidden = false;
            return spaces[row, col].Type == SpaceType.BOMB;
        }

        public static void FlipAll()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    spaces[y, x].IsHidden = false;
                }
            }
        }

        private static void GenerateBoard()
        {
            spaces = new SpaceBase[height, width];
            PlaceBombs();
            FillSpaces();
        }

        private static void PlaceBombs()
        {
            int bombCount = (int)(width * height * bombDensity);
            int new_x, new_y;
            while (bombCount > 0)
            {
                new_x = r.Next(width);
                new_y = r.Next(height);
                if (spaces[new_y, new_x]?.Type == SpaceType.BOMB)
                {
                    continue;
                }

                spaces[new_y, new_x] = new BombSpace();
                bombCount--;
            }
        }

        private static void FillSpaces()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (spaces[y, x]?.Type == SpaceType.BOMB)
                    {
                        continue;
                    }

                    spaces[y, x] = new SafeSpace();
                    ((SafeSpace)spaces[y, x]).SorroundingBombs = CountBombs(x, y);
                }
            }
        }

        private static int CountBombs(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if ((i < 0) || (i >= width)) continue;
                    if ((j < 0) || (j >= height)) continue;
                    if ((i == x) && (j == y)) continue;

                    if (spaces[j, i]?.Type == SpaceType.BOMB)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}