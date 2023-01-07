using System;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for linear movement between start and end position
    /// </summary>
    public class LinearFunction : TimingFunction
    {
        public LinearFunction(int duration) : base(duration) { }

        protected override float CalcAnimationPosition()
        {
            return _animationPercent;
        }
    }
}