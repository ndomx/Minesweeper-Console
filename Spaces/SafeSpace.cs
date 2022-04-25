using System;

namespace Minesweeper.Spaces
{
    public class SafeSpace : SpaceBase
    {
        public int SorroundingBombs { get; set; }
        protected override string UncoveredText => SorroundingBombs + "";
        public override ConsoleColor GetBackgroundColor() => ConsoleColor.Black;

        public SafeSpace() : base(SpaceType.NORMAL)
        {
            SorroundingBombs = 0;
        }
    }
}