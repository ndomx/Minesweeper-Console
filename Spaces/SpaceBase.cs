using System;

namespace minesweeper
{
    public abstract class SpaceBase
    {
        public readonly SpaceType Type;

        public bool IsHidden { get; set; }

        protected abstract string UncoveredText { get; }

        public abstract ConsoleColor GetBackgroundColor();

        public SpaceBase(SpaceType @type)
        {
            Type = @type;
            IsHidden = true;
        }

        public override string ToString()
        {
            return IsHidden ? " " : UncoveredText;
            // return UncoveredText;
        }
    }
}