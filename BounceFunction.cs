using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class BounceFunction : ITimingFunction
    {
        private float _animationPosition;
        public float AnimationPosition
        {
            get { return _animationPosition; }
        }

        private float _duration;
        public float Duration
        {
            get { return _duration; }
        }

        private bool _isAnimating;
        public bool IsAnimating
        {
            get { return _isAnimating; }
        }

        private float _animationPercent;

        private float[] _bouncePoints = new float[] { 0.352f, 0.715f, 0.903f, 1f };

        public BounceFunction(float duration = 120f)
        {
            _duration = duration;
            _animationPosition = 0f;
            _isAnimating = true;

            _animationPercent = 0f;
        }

        public virtual void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            // update animation percent
            _animationPercent += deltaTime / _duration;

            // toggle animation off if complete
            if (_animationPercent >= 1f) _isAnimating = false;

            // clamp percent
            _animationPercent = MathHelper.Clamp(_animationPercent, 0f, 1f);

            // update animation position
            _animationPosition = GetAnimationPosition();
        }

        private float GetAnimationPosition()
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

