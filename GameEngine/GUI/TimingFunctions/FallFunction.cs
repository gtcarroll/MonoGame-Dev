using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for sinusoidal movement between start and end position
    /// </summary>
    public class FallFunction : TimingFunction
    {
        private bool _isReversed;

        public FallFunction(int duration, bool isReversed = false) : base(duration)
        {
            _isReversed = isReversed;
        }

        protected override float CalcAnimationPosition()
        {
            float xSquared = _animationPercent * _animationPercent;
            return _isReversed ? -xSquared - 2 * _animationPercent
                               : xSquared;
        }
    }
}

