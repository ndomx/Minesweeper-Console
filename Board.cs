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
            for (int x = 0; x < width; x++)
            {
                Console.Write("|");
                for (int y = 0; y < height; y++)
                {
                    SpaceBase s = spaces[x,y];
                    Console.BackgroundColor = s.GetBackgroundColor();
                    Console.Write(" {0} ", s);
                    Console.BackgroundColor = DefaultBackground;
                    Console.Write("|");
                }

                Console.WriteLine();
                Console.WriteLine(bounds);
            }
        }

        private static void GenerateBoard()
        {
            spaces = new SpaceBase[width, height];
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
                if (spaces[new_x, new_y]?.Type == SpaceType.BOMB)
                {
                    continue;
                }

                spaces[new_x, new_y] = new BombSpace();
                bombCount--;
            }
        }

        private static void FillSpaces()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (spaces[x, y]?.Type == SpaceType.BOMB)
                    {
                        continue;
                    }

                    spaces[x, y] = new SafeSpace();
                    ((SafeSpace)spaces[x, y]).SorroundingBombs = CountBombs(x, y);
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

                    if (spaces[i, j]?.Type == SpaceType.BOMB)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}