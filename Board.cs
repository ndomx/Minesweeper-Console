using System;
using Minesweeper.Spaces;

namespace Minesweeper
{
    public static class Board
    {
        public static int BombCount { get; private set; }
        public static int MarkedBombs { get; private set; }
        public static int RemainingBombs { get; private set; }

        private static Random r = new Random(56);
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

        public static bool Flip(int row, int col, PlayerAction action, bool byPlayer = true)
        {
            if ((row < 0) || (row >= height))
            {
                throw new ArgumentOutOfRangeException("Row out of range");
            }

            if ((col < 0) || (col >= width))
            {
                throw new ArgumentOutOfRangeException("Column out of range");
            }

            if (spaces[row, col].State != SpaceState.HIDDEN && !byPlayer)
            {
                return false;
            }

            switch (action)
            {
                case PlayerAction.DISCOVER: return OnSpaceDiscover(row, col, byPlayer);
                case PlayerAction.MARK_BOMB: return OnSpaceMarkedAsBomb(row, col);
                case PlayerAction.MARK_UNDEFINED: return OnSpaceMarkedUndefined(row, col);
                default: throw new ArgumentException("Invalid action");
            }
        }

        public static void FlipAll()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Flip(y, x, PlayerAction.DISCOVER, false);
                }
            }
        }

        private static bool OnSpaceDiscover(int row, int col, bool byPlayer)
        {
            UnmarkBomb(row, col);

            spaces[row, col].SetState(PlayerAction.DISCOVER, byPlayer);
            if (spaces[row, col].Type == SpaceType.BOMB)
            {
                return true;
            }

            SafeSpace s = (SafeSpace)spaces[row, col];
            if (s.SorroundingBombs == 0)
            {
                FlipAdjacent(row, col);
            }

            return false;
        }

        private static bool OnSpaceMarkedAsBomb(int row, int col)
        {
            MarkBomb(row, col);
            spaces[row, col].SetState(PlayerAction.MARK_BOMB, true);

            return false;
        }

        private static bool OnSpaceMarkedUndefined(int row, int col)
        {
            UnmarkBomb(row, col);
            spaces[row, col].SetState(PlayerAction.MARK_UNDEFINED, true);

            return false;
        }

        private static void FlipAdjacent(int row, int col)
        {
            for (int y = row - 1; y <= row + 1; y++)
            {
                if ((y < 0) || (y >= height)) continue;

                for (int x = col - 1; x <= col + 1; x++)
                {
                    if ((x < 0) || (x >= width)) continue;
                    if ((x == col) && (y == row)) continue;

                    Flip(y, x, PlayerAction.DISCOVER, false);
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
            BombCount = bombCount;
            RemainingBombs = bombCount;

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
                    ((SafeSpace)spaces[y, x]).SorroundingBombs = CountAdjacentBombs(x, y);
                }
            }
        }

        private static int CountAdjacentBombs(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                if ((i < 0) || (i >= width)) continue;

                for (int j = y - 1; j <= y + 1; j++)
                {
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

        private static void MarkBomb(int row, int col)
        {
            if (spaces[row, col].State == SpaceState.MARKED) return;

            MarkedBombs++;
            if (spaces[row, col].Type == SpaceType.BOMB)
            {
                RemainingBombs--;
            }
        }

        private static void UnmarkBomb(int row, int col)
        {
            if (spaces[row, col].State != SpaceState.MARKED) return;

            MarkedBombs--;
            if (spaces[row, col].Type == SpaceType.BOMB)
            {
                RemainingBombs++;
            }
        }
    }
}