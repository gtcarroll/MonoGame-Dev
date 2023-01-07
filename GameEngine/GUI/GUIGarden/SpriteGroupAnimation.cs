using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class SpriteGroupAnimation
    {
        #region Properties

        /// <summary>
        /// SpriteGroup to be animated.
        /// </summary>
        public SpriteGroup Sprite;

        /// <summary>
        /// SpriteGroupState for the animation to target or move towards.
        /// </summary>
        public SpriteGroupState Target;

        /// <summary>
        /// SpriteGroupState of this animation's current position.
        /// Set to null until Begin() is called.
        /// </summary>
        public SpriteGroupState Current;

        /// <summary>
        /// SpriteGroupState of this animation's start position.
        /// Set to null until Begin() is called.
        /// </summary>
        public SpriteGroupState Start;

        /// <summary>
        /// SpriteGroupState representing the delta between this animation's
        /// Start and Target positions.
        /// Set to null until Begin() is called.
        /// </summary>
        public SpriteGroupState Delta;

        /// <summary>
        /// Time (in ms) until this animation should begin.
        /// </summary>
        public int Delay;

        /// <summary>
        /// Time (in ms) that this animation takes to complete once it begins.
        /// </summary>
        public int Duration;

        /// <summary>
        /// Optional TimingFunction used to determine the animation's position
        /// relative to its duration.
        /// If you choose not to use it, UpdateAnimation() must be overwritten
        /// with an alternative method of updating animation state.
        /// </summary>
        protected TimingFunction Timing;

        /// <summary>
        /// True if Begin() has been called,
        /// False otherwise.
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
            protected set { _isStarted = value; }
        }
        private bool _isStarted;

        /// <summary>
        /// True if animation is completed (no longer animating),
        /// False otherwise.
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            protected set { _isCompleted = value; }
        }
        private bool _isCompleted;

        #endregion

        #region Constructors

        public SpriteGroupAnimation(SpriteGroup sprite, SpriteGroupState target,
                                    TimingFunction timing, int delay = 0)
            : this(sprite, target, timing.Duration, delay)
        {
            Timing = timing;
        }
        public SpriteGroupAnimation(SpriteGroup sprite, SpriteGroupState target,
                                    int duration, int delay = 0)
        {
            Sprite = sprite;
            Target = target;

            Duration = duration;
            Delay = delay;

            _isStarted = false;
            _isCompleted = false;
        }

        #endregion

        #region Game Loop Hooks

        /// <summary>
        /// Updates animation if it has been started and is not yet completed.
        /// </summary>
        /// <param name="time">Game's time object</param>
        public void Update(GameTime time)
        {
            if (_isStarted && !_isCompleted)
            {
                UpdateAnimation(time);
            }
        }

        #endregion

        #region Virtual State Methods

        /// <summary>
        /// Primes this animation to begin, and initializes Current state.
        /// </summary>
        public virtual void Begin()
        {
            Current = Sprite.CurrentState;
            Start = Sprite.CurrentState;
            Delta = GetDelta(Start, Target);

            _isStarted = true;
        }

        /// <summary>
        /// Updates this animation's Current state and IsCompleted status.
        /// </summary>
        /// <param name="time">Game's time object</param>
        /// <exception cref="NotImplementedException">
        /// The default implementation assumes the value of Timing has been
        /// set and will throw an error if not. Please override this function
        /// if you wish to use an alternative animation method.
        /// </exception>
        protected virtual void UpdateAnimation(GameTime time)
        {
            if (Timing != null)
            {
                Timing.Update(time);

                Current = CalcCurrentState(Timing.AnimationPosition,
                                           Start, Delta);
                _isCompleted = Timing.IsCompleted;
            }
            else
            {
                throw new NotImplementedException(
                    "TimingFunction is null. If this is intentional, " +
                    "please override UpdateAnimation() to use an " +
                    "alternative method of updating animation state.");
            }
        }

        #endregion

        #region Non-Virtual State Methods

        /// <summary>
        /// Calculates a Delta state representing the difference between the
        /// provided start and end states.
        /// </summary>
        /// <param name="start">State representing animation start</param>
        /// <param name="end">State representing animation end</param>
        /// <returns>Delta between start and end states</returns>
        protected static SpriteGroupState GetDelta(SpriteGroupState start,
                                                   SpriteGroupState end)
        {
            List<SpriteState> deltaStates = new List<SpriteState>();

            for (int i = 0; i < start.SpriteStates.Count; i++)
            {
                Texture2D sprite = end.SpriteStates[i].Sprite;

                Rectangle startDest = start.SpriteStates[i].Destination;
                Rectangle endDest = end.SpriteStates[i].Destination;

                Rectangle deltaDest = new Rectangle(
                    endDest.Location - startDest.Location,
                    endDest.Size - startDest.Size);

                Rectangle? deltaSource = null;
                if (start.SpriteStates[i].Source != null &&
                    end.SpriteStates[i].Source != null)
                {
                    Rectangle startSource =
                        (Rectangle)start.SpriteStates[i].Source;
                    Rectangle endSource =
                        (Rectangle)end.SpriteStates[i].Source;

                    deltaSource = new Rectangle(
                        endSource.Location - startSource.Location,
                        endSource.Size - startSource.Size);
                }

                deltaStates.Add(new SpriteState(sprite, deltaDest,
                                                        deltaSource));
            }

            return new SpriteGroupState(deltaStates, end.Center - start.Center);
        }

        /// <summary>
        /// Calculate's the Current state based on the animation's position as
        /// a ratio of the Delta between the Start and Target states.
        /// </summary>
        /// <param name="animPos">Ratio of this animation's Delta</param>
        /// <param name="start">Start state to add scaled Delta to</param>
        /// <param name="delta">Delta between Start and Target states</param>
        /// <returns>This animation's Current SpriteGroupState</returns>
        protected SpriteGroupState CalcCurrentState(float animPos,
                                                    SpriteGroupState start,
                                                    SpriteGroupState delta)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            for (int i = 0; i < delta.SpriteStates.Count; i++)
            {
                Texture2D sprite = delta.SpriteStates[i].Sprite;

                Rectangle startDest = start.SpriteStates[i].Destination;
                Rectangle deltaDest = delta.SpriteStates[i].Destination;
                Rectangle scaledDest = ScaleRectangle(deltaDest, animPos);
                Rectangle currDest = new Rectangle(
                    startDest.Location + scaledDest.Location,
                    startDest.Size + scaledDest.Size);

                Rectangle? currSource = null;
                if (delta.SpriteStates[i].Source != null)
                {
                    Rectangle startSource =
                        (Rectangle)start.SpriteStates[i].Source;
                    Rectangle deltaSource =
                        (Rectangle)delta.SpriteStates[i].Source;
                    Rectangle scaledSource = ScaleRectangle(deltaSource,
                                                            animPos);
                    currSource = new Rectangle(
                        startSource.Location + scaledSource.Location,
                        startSource.Size + scaledSource.Size);
                }

                newStates.Add(new SpriteState(sprite, currDest, currSource));
            }

            Point scaledCenter = new Point(
                (int)(delta.Center.X * animPos),
                (int)(delta.Center.Y * animPos));

            return new SpriteGroupState(newStates, Start.Center + scaledCenter);
        }

        /// <summary>
        /// Helper method for scaling Rectangles by a provided scale value.
        /// </summary>
        /// <param name="rectangle">Rectangle to be scaled from</param>
        /// <param name="scale">Scale to multiply rectangle values by</param>
        /// <returns>New rectangle scaled by provided scale value</returns>
        protected Rectangle ScaleRectangle(Rectangle rectangle, float scale)
        {
            return new Rectangle(
                (int)(rectangle.Location.X * scale),
                (int)(rectangle.Location.Y * scale),
                (int)(rectangle.Size.X * scale),
                (int)(rectangle.Size.Y * scale));
        }
    }

    #endregion
}