using System;

namespace Minesweeper.Spaces
{
    public abstract class SpaceBase
    {
        public readonly SpaceType Type;

        public SpaceState State { get; set; }

        // public bool IsHidden { get; set; }

        protected abstract string UncoveredText { get; }

        public abstract ConsoleColor GetBackgroundColor();

        protected SpaceBase(SpaceType @type)
        {
            Type = @type;
            State = SpaceState.HIDDEN;
        }

        public override string ToString()
        {
            switch (State)
            {
                case SpaceState.HIDDEN: return " ";
                case SpaceState.MARKED: return "M";
                case SpaceState.UNDEFINED: return "?";
                case SpaceState.DISCOVERED: return UncoveredText;
                default: return " ";
            }
        }

        public virtual void SetState(PlayerAction action, bool byPlayer = true)
        {
            switch (action)
            {
                case PlayerAction.MARK_BOMB: State = SpaceState.MARKED; break;
                case PlayerAction.MARK_UNDEFINED: State = SpaceState.UNDEFINED; break;
                case PlayerAction.DISCOVER: State = SpaceState.DISCOVERED; break;
            }
        }
    }
}