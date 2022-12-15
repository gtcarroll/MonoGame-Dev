using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class HighlightCursor
    {
        Effect effect;

        // cursor state
        private Point _position;
        private Point _positionPrev;
        private Color _color;
        private int _thickness;

        // highlighted sprites
        //private HighlightSprite _curr;
        //private HighlightSprite _prev;
        private HighlightSprite _cursor;
        private HighlightSprite _cursorPrev;
        private SpriteGroupState _curr;
        private SpriteGroupState _prev;
        private SpriteGroup _target;

        // transition state
        private CursorTransition _transition;
        private bool _wasAnimatingLastFrame;
        public bool IsAnimating { get { return _transition.IsAnimating; } }

        public HighlightCursor(GameManager game)
            : this(game, new Color(112, 211, 87), 5) { }

        public HighlightCursor(GameManager game, Color color, int thickness)
        {
            // set initial highlighted sprites
            // TODO: move load out of constructor
            Texture2D cursor = game.Content.Load<Texture2D>("Textures/1k/circle");
            //effect = game.Content.Load<Effect>("MonoColorize");

            _curr = null;
            _prev = null;
            _cursor = new HighlightSprite(cursor, new Point(100));
            _cursorPrev = new HighlightSprite(cursor, new Point(100));

            // set highlight parameters
            _color = color;
            _thickness = thickness;

            // set initial highlight state
            _position = new Point(0, 0);
            _positionPrev = new Point(0, 0);
            _transition = new CursorTransition(_position, 100f);
        }

        public void AnimateTo(SpriteGroup spriteGroup)
        {
            // begin transition
            _transition.StartTransitionTo(spriteGroup);
            _wasAnimatingLastFrame = true;

            // update sprites
            _prev = _curr;
            _target = spriteGroup;

            // rotate cursor
            _cursor.RotateTo(_transition.Delta);
            _cursorPrev.Rotation = _cursor.Rotation;
        }

        public void Update(GameTime time)
        {
            if (_transition.IsAnimating)
            {
                _transition.Update(time);

                // update position
                _positionPrev = _position;
                _position = _transition.Current;

                // scale sprites
                float dist2Start = _transition.Distance *
                                   _transition.SinusoidalPercent;
                float dist2End = _transition.Distance - dist2Start;

                _curr = _target.GetHighlightState(_thickness);

                // scale and stretch cursor
                float pct2Mid = Math.Abs(_transition.SinusoidalPercent - 0.5f);

                _cursorPrev.Scale = _cursor.Scale;
                _cursorPrev.Stretch = _cursor.Stretch;

                _cursor.Scale = Math.Max(pct2Mid * 2, 0.3f);
                _cursor.Stretch = (_transition.Speed * 2f) -
                                  (_transition.Speed * 2f - 1) *
                                  2 * Math.Max(pct2Mid, 0.1f);
            }
            else if (_target != null) //&& _target.Transition.IsAnimating)
            {
                _curr = _target.GetHighlightState(_thickness);
            }
            else if (_wasAnimatingLastFrame)
            {
                _wasAnimatingLastFrame = false;
                _curr = _target.GetHighlightState(_thickness);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_curr != null && !_transition.IsAnimating)
            {
                _curr.Draw(spriteBatch, _color);
            }

            //if (_transition.IsAnimating)
            //{
            //    _cursorPrev.Draw(spriteBatch, _positionPrev, _color);
            //    _cursor.Draw(spriteBatch, _position, _color);
            //}

        }
    }
}

