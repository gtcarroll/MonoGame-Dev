using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    public class LinearFunction : ITimingFunction
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

        public LinearFunction(float duration = 120f)
        {
            _duration = duration;
            _animationPosition = 0f;
            _isAnimating = true;
        }

        public virtual void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            // update animation position
            _animationPosition += deltaTime / _duration;

            // toggle animation off if complete
            if (_animationPosition >= 1f) _isAnimating = false;

            // clamp position
            _animationPosition = MathHelper.Clamp(_animationPosition, 0f, 1f);
        }
    }
}

