using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public abstract class SpriteGroup
    {
        #region Fields

        /// <summary>
        /// List of sprites used in this SpriteGroup.
        /// </summary>
        protected List<Texture2D> Sprites;

        /// <summary>
        /// Transition used to interpolate between two SpriteGroupStates.
        /// </summary>
        protected SpriteGroupTransition Transition;

        #endregion

        #region Properties

        /// <summary>
        /// Current state of this SpriteGroup.
        /// </summary>
        public SpriteGroupState CurrentState
        {
            get { return _currentState; }
            protected set { _currentState = value; }
        }
        private SpriteGroupState _currentState;

        /// <summary>
        /// Default state of this SpriteGroup, used when unhovered.
        /// </summary>
        public SpriteGroupState DefaultState
        {
            get { return _defaultState; }
            protected set { _defaultState = value; }
        }
        private SpriteGroupState _defaultState;

        /// <summary>
        /// Hover state of this SpriteGroup, used when hovered.
        /// </summary>
        public SpriteGroupState HoverState
        {
            get { return _hoverState; }
            protected set { _hoverState = value; }
        }
        private SpriteGroupState _hoverState;

        /// <summary>
        /// Hover state of this SpriteGroup. When set to true/false,
        /// automatically transitions CurrentState to hovered/default state.
        /// </summary>
        public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                _isHovered = value;
                SpriteGroupState targetState = _isHovered ? HoverState
                                                          : DefaultState;
                BeginHoverAnimation(targetState.GetTranslatedCopy(_anchor));
            }
        }
        private bool _isHovered;

        /// <summary>
        /// Position of this SpriteGroup. When set, automatically transitions
        /// CurrentState to updated position.
        /// </summary>
        public Point Anchor
        {
            get { return _anchor; }
            set
            {
                _anchor = value;
                BeginRepositionAnimation(DefaultState.GetTranslatedCopy(_anchor));
            }
        }
        private Point _anchor;

        #endregion

        #region Constructors

        public SpriteGroup(Point anchor)
        {
            Sprites = new List<Texture2D>();
            _anchor = anchor;
        }

        #endregion

        #region Loading Methods

        /// <summary>
        /// Loads content required to render this SpriteGroup, such as textures.
        /// </summary>
        /// <param name="content">This game's ContentManager object.</param>
        public abstract void LoadContent(ContentManager content);

        #endregion

        #region Game Loop Hooks

        /// <summary>
        /// Updates this SpriteGroup's state.
        /// </summary>
        /// <param name="time">This game's GameTime object.</param>
        public virtual void Update(GameTime time)
        {
            if (Transition != null && Transition.IsAnimating)
            {
                Transition.Update(time);
                CurrentState = Transition.Current;
            }
        }

        /// <summary>
        /// Draws this SpriteGroup to the provided SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object to draw to.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentState != null)
            {
                CurrentState.Draw(spriteBatch);
            }
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Begins a hover/unhover animation that transitions between
        /// CurrentState and the provided target state.
        /// </summary>
        /// <param name="targetState">Target state of animation.</param>
        protected virtual void BeginHoverAnimation(SpriteGroupState targetState)
        {
            Transition = new SpriteGroupTransition(CurrentState,
                                                   targetState, 128f);
        }

        /// <summary>
        /// Begins a repositioning animation that transitions between
        /// CurrentState and the provided target state.
        /// </summary>
        /// <param name="targetState">Target state of animation.</param>
        protected virtual void BeginRepositionAnimation(SpriteGroupState targetState)
        {
            Transition = new SpriteGroupTransition(CurrentState,
                                                   targetState, 256f);
        }

        /// <summary>
        /// Creates and returns a SpriteGroupState used to draw a highlighted
        /// border around this SpriteGroup.
        /// </summary>
        /// <param name="thickness">Width (pixels) of higlighted border.</param>
        /// <returns>SpriteGroupState used to draw highlighted border.</returns>
        public virtual SpriteGroupState GetHighlightState(int thickness = 5)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            foreach (SpriteState spriteState in CurrentState.SpriteStates)
            {
                Rectangle borderRect = new Rectangle(
                    spriteState.Destination.Location.X - thickness,
                    spriteState.Destination.Location.Y - thickness,
                    spriteState.Destination.Size.X + 2 * thickness,
                    spriteState.Destination.Size.Y + 2 * thickness);

                newStates.Add(new SpriteState(spriteState.Sprite, borderRect,
                                              spriteState.Source));
            }

            return new SpriteGroupState(newStates, CurrentState.Center);
        }
    }

    #endregion
}

