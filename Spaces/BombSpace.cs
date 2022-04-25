using System;

namespace minesweeper
{
    public class BombSpace : SpaceBase
    {
        protected override string UncoveredText => "X";
        public BombSpace() : base(SpaceType.BOMB)
        {

        }

        public override ConsoleColor GetBackgroundColor()
        {
            return (IsHidden) ? ConsoleColor.Black : ConsoleColor.Red;
        }

    }
}