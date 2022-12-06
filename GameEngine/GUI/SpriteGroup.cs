using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;

namespace EverythingUnder.GUI
{
    public enum SpriteStyle
    {
        Default,
        Hover,
        //Highlight
    }

    public abstract class SpriteGroup
    {
        public GroupState DefaultState;

        public List<Texture2D> Sprites;
        public GroupState CurrentState;
        public Dictionary<SpriteStyle, GroupState> StyleStates;

        public SpriteGroupTransition Transition;
        public float TransitionDuration;
        public GroupState TargetState;

        public bool IsHovered;

        private SpriteStyle _style;
        public SpriteStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                BeginTransition(GetNextState());
            }
        }

        private Point _anchor;
        public Point Anchor
        {
            get { return _anchor; }
            set
            {
                _anchor = value;
                BeginRepositionAnimation(DefaultState.GetTranslatedCopy(_anchor));
            }
        }

        public SpriteGroup(Point anchor, float transitionDuration = 120f)
        {
            Sprites = new List<Texture2D>();
            StyleStates = new Dictionary<SpriteStyle, GroupState>();

            IsHovered = false;

            _style = SpriteStyle.Default;
            _anchor = anchor;

            TransitionDuration = transitionDuration;
        }

        public virtual void BeginTransition(GroupState endState)
        {
            TargetState = endState;
            Transition = new SpriteGroupTransition(CurrentState, endState,
                                                   TransitionDuration);
        }

        public virtual void BeginRepositionAnimation(GroupState endState)
        {
            TargetState = endState;
            Transition = new SpriteGroupTransition(CurrentState, endState,
                                                   TransitionDuration);
        }

        public virtual void Update(GameTime time)
        {
            //float deltaTime = time.ElapsedGameTime.Milliseconds;
            //CurrentState.Rotation += deltaTime * 0.001f;

            //if (IsHovered && _style == SpriteStyle.Default)
            //{
            //    Style = SpriteStyle.Hover;
            //}
            //else if (!IsHovered && _style == SpriteStyle.Hover)
            //{
            //    Style = SpriteStyle.Default;
            //}

            if (Transition != null && Transition.IsAnimating)
            {
                Transition.Update(time);
                CurrentState = Transition.Current;
            }
        }

        public GroupState GetNextState()
        {
            return StyleStates[_style].GetTranslatedCopy(_anchor);
        }

        //public GroupState GetHighlight()
        //{
        //    return StyleStates[SpriteStyle.Highlight].GetTranslatedCopy(_center);
        //}

        public abstract void LoadContent(ContentManager content);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentState != null)
            {
                Draw(spriteBatch, CurrentState);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GroupState state)
        {
            state.Draw(spriteBatch);
        }

        public virtual GroupState GetBetweenState(GroupState prev, GroupState curr, float percentComplete)
        {
            throw new NotImplementedException();
        }

        public virtual GroupState GetHighlightState(int thickness = 5)
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

            return new GroupState(newStates, CurrentState.Center);
        }
    }
}

