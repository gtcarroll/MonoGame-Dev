using System;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    /// <summary>
    /// Abstract class for an animation timing function.
    ///
    /// Usage:
    ///   - Call Update() function to progress animation values.
    ///   - Use AnimationPosition to get current position as a ratio of the
    ///     delta between the start and end positions.
    ///   - Use IsAnimating to determine when animation is complete.
    /// </summary>
    public abstract class TimingFunction
    {
        #region Properties

        /// <summary>
        /// Percent of animation completed.
        /// Example:
        ///   - 0.0f: animation 0% completed
        ///   - 0.5f: animation 50% completed
        ///   - 1.0f: animation 100% completed
        /// </summary>
        protected float _animationPercent;
        public float AnimationPercent
        {
            get { return _animationPercent; }
        }

        /// <summary>
        /// Position as ratio of animation delta (end position - start position).
        /// Example:
        ///   - 0.0f: 0% of delta (start position)
        ///   - 0.5f: 50% of delta
        ///   - 1.0f: 100% of delta (end position)
        /// </summary>
        protected float _animationPosition;
        public float AnimationPosition
        {
            get { return _animationPosition; }
        }

        /// <summary>
        /// Total duration of animation in ms.
        /// </summary>
        protected float _duration;
        public float Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// True if animation is in progress,
        /// False if animation is completed.
        /// </summary>
        protected bool _isAnimating;
        public bool IsAnimating
        {
            get { return _isAnimating; }
        }

        #endregion

        #region Constructors

        public TimingFunction(float duration = 128f)
        {
            _animationPosition = 0f;
            _animationPercent = 0f;
            _duration = duration;
            _isAnimating = true;
        }

        #endregion

        #region Game Loop Hooks

        /// <summary>
        /// Updates animation progress.
        /// </summary>
        /// <param name="time">Game's time object</param>
        public virtual void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            // update animation percent
            _animationPercent += deltaTime / _duration;

            // toggle animation off if complete
            if (IsAnimationComplete()) _isAnimating = false;

            // clamp percent
            _animationPercent = MathHelper.Clamp(_animationPercent, 0f, 1f);

            // update animation position
            _animationPosition = GetAnimationPosition();
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Calculates the current animation position.
        /// </summary>
        /// <returns>Current animation position</returns>
        protected abstract float GetAnimationPosition();

        /// <summary>
        /// Determines whether this animation is completed.
        /// </summary>
        /// <returns>True if animation is completed, false otherwise</returns>
        protected virtual bool IsAnimationComplete()
        {
            return _animationPercent >= 1f;
        }

        #endregion
    }
}

