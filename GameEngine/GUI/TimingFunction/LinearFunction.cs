using System;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for linear movement between start and end position
    /// </summary>
    public class LinearFunction : TimingFunction
    {
        public LinearFunction(float duration) : base(duration) { }

        protected override float GetAnimationPosition()
        {
            return _animationPercent;
        }
    }
}