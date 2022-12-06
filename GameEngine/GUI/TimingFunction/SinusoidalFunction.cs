using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for sinusoidal movement between start and end position
    /// </summary>
    public class SinusoidalFunction : TimingFunction
    {
        public SinusoidalFunction(float duration) : base(duration) { }

        protected override float GetAnimationPosition()
        {
            return 1 - (MathF.Cos(_animationPercent * MathHelper.Pi) + 1) / 2f;
        }
    }
}

