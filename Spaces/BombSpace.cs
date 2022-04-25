using System;

namespace minesweeper
{
    public class BombSpace : SpaceBase
    {
        public bool Detonated { get; set; }

        protected override string UncoveredText => "*";

        public BombSpace() : base(SpaceType.BOMB)
        {
            Detonated = false;
        }

        public override ConsoleColor GetBackgroundColor()
        {
            if (IsHidden) return ConsoleColor.Black;
            else if (Detonated) return ConsoleColor.Red;
            else return ConsoleColor.DarkRed;
        }

        public override bool Flip(bool byPlayer)
        {
            Detonated = byPlayer;
            return base.Flip(byPlayer);
        }
    }
}