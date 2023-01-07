using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for sinusoidal movement between start and end position
    /// </summary>
    public class SineFunction : TimingFunction
    {
        public SineFunction(int duration) : base(duration) { }

        protected override float CalcAnimationPosition()
        {
            return MathF.Sin(_animationPercent * MathHelper.Pi);
        }
    }
}

