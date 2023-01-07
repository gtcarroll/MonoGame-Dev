using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for flipping over a card (up or down)
    /// </summary>
    public class FlipFunction : TimingFunction
    {
        private int _yDelta;

        public FlipFunction(int duration, bool isFlipUp) : base(duration)
        {
            _yDelta = isFlipUp ? 0 : 1;
        }

        protected override float CalcAnimationPosition()
        {
            float cos = MathF.Cos(_animationPercent * MathHelper.Pi);

            return Math.Max(0, cos * -1 + _yDelta);
        }
    }
}