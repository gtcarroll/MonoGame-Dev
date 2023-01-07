using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    public class AnimationQueue
    {
        #region Fields

        /// <summary>
        /// Current time until the longest animation in the queue ends.
        /// </summary>
        private int _queueEnd;

        /// <summary>
        /// Additional delay to append to animations. When set with BeginFrame,
        /// places animations relative to the current queue's end.
        /// </summary>
        private int _frameStart;

        /// <summary>
        /// Queue of SpriteGroups and the Animations to play after their delay.
        /// </summary>
        private Dictionary<SpriteGroupAnimation, SpriteGroup> _queued;

        #endregion

        #region Constructors

        public AnimationQueue()
        {
            _queueEnd = 0;
            _frameStart = 0;

            _queued = new Dictionary<SpriteGroupAnimation, SpriteGroup>();
        }

        #endregion

        #region Game Loop Hooks
        /// <summary>
        /// Updates all queued animation delays.
        /// </summary>
        /// <param name="time">Game's time object</param>
        public void Update(GameTime time)
        {
            int elapsed = time.ElapsedGameTime.Milliseconds;

            foreach (KeyValuePair<SpriteGroupAnimation, SpriteGroup> entry in _queued)
            {
                entry.Key.Delay -= elapsed;
                if (entry.Key.Delay <= 0)
                {
                    _queued.Remove(entry.Key);

                    entry.Value.Animation = entry.Key;
                }
            }
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Sets future animation delays to be relative to the end of the
        /// current queue.
        ///
        /// Ex: If current queue will end at 500ms, Add(anim, 100ms) would
        ///     place anim at 600ms in the queue.
        /// </summary>
        public void BeginFrame()
        {
            _frameStart = _queueEnd;
        }

        /// <summary>
        /// Unsets future animation delay increases from BeginFrame.
        /// </summary>
        public void EndFrame()
        {
            _frameStart = 0;
        }

        /// <summary>
        /// Adds an animation to the queue with an optional delay.
        /// </summary>
        /// <param name="animation">Animation to add to the queue</param>
        /// <param name="delay">Optional delay in ms</param>
        public void Add(SpriteGroup sprite, SpriteGroupAnimation animation)
        {
            // update delay and add to queue
            animation.Delay += _frameStart;
            _queued.Add(animation, sprite);

            // update _queueEnd
            int animEnd = animation.Delay + animation.Duration;
            if (animEnd > _queueEnd)
            {
                _queueEnd = animEnd;
            }
        }

        /// <summary>
        /// Returns true if sprite is in the animation queue, false otherwise.
        /// </summary>
        /// <param name="sprite">SpriteGroup object</param>
        /// <returns>True if sprite is in queue, false otherwise</returns>
        public bool Contains(SpriteGroup sprite)
        {
            return _queued.ContainsValue(sprite);
        }

        #endregion
    }
}

