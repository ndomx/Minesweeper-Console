using System;

namespace Minesweeper.Spaces
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
            if (State != SpaceState.DISCOVERED) return ConsoleColor.Black;
            else if (Detonated) return ConsoleColor.DarkMagenta;
            else return ConsoleColor.Red;
        }

        public override void SetState(PlayerAction action, bool byPlayer = true)
        {
            if (action == PlayerAction.DISCOVER)
            {
                Detonated = byPlayer;
            }
            
            base.SetState(action, byPlayer);
        }
    }
}