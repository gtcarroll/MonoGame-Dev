using System;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Timing function for a bouncing animation between start and end position
    /// </summary>
    public class BounceFunction : TimingFunction
    {
        /// <summary>
        /// Breakpoints for piecewise bounce functions
        /// </summary>
        private float[] _bouncePoints = new float[] { 0.352f, 0.715f, 0.903f, 1f };

        public BounceFunction(int duration) : base(duration) { }

        protected override float CalcAnimationPosition()
        {
            float x = _animationPercent;

            if (x < _bouncePoints[0])
            {
                return 8.094f * x * x;
            }
            else if (x < _bouncePoints[1])
            {
                return (7.564f * x * x) - (8.068f * x) + 2.901f;
            }
            else if (x < _bouncePoints[2])
            {
                return (7.081f * x * x) - (11.458f * x) + 5.573f;
            }
            else if (x < _bouncePoints[3])
            {
                return (6.643f * x * x) - (12.641f * x) + 6.998f;
            }
            else
            {
                return 1f;
            }
        }
    }
}