using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class HighlightCursor
    {
        // cursor state
        private Point _position;
        private Color _color;
        private int _thickness;

        // highlighted sprites
        //private HighlightSprite _curr;
        //private HighlightSprite _prev;
        private HighlightSprite _cursor;
        private GroupState _curr;
        private GroupState _prev;
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
            Texture2D cursor = game.Content.Load<Texture2D>("Textures/simple-circle");
            _curr = null; //new HighlightSprite(cursor, new Point(100));
            _prev = null; // new HighlightSprite(cursor, new Point(100));
            _cursor = new HighlightSprite(cursor, new Point(100));

            // set highlight parameters
            _color = color;
            _thickness = thickness;

            // set initial highlight state
            _position = new Point(0, 0);
            _transition = new CursorTransition(_position, 100f);
        }

        //public void AnimateTo(SelectableSprite sprite)
        //{
        //    // begin transition
        //    _transition.StartTransitionTo(sprite);

        //    // update sprites
        //    _prev = _curr;
        //    _curr = sprite.GetHighlight();

        //    // rotate cursor
        //    _cursor.RotateTo(_transition.Delta);
        //}

        public void AnimateTo(SpriteGroup spriteGroup)
        {
            // begin transition
            _transition.StartTransitionTo(spriteGroup);
            _wasAnimatingLastFrame = true;

            // update sprites
            _prev = _curr;
            _target = spriteGroup;
            //_curr = _target.GetHighlightState(_thickness);

            // rotate cursor
            _cursor.RotateTo(_transition.Delta);
        }

        public void Update(GameTime time)
        {
            if (_transition.IsAnimating)
            {
                _transition.Update(time);

                // update position
                _position = _transition.Current;

                // scale sprites
                float dist2Start = _transition.Distance *
                                   _transition.PercentComplete;
                float dist2End = _transition.Distance - dist2Start;

                //_prev.Scale = Math.Max(
                //                1f - (1.5f * dist2Start / _prev.Dimension.X),
                //                0f);
                //_curr.Scale = Math.Max(
                //                1f - (1.5f * dist2End / _curr.Dimension.X),
                //                0f);

                _curr = _target.GetHighlightState(_thickness);

                // scale and stretch cursor
                float pct2Mid = Math.Abs(_transition.PercentComplete - 0.5f);

                _cursor.Scale = Math.Max(pct2Mid * 2, 0.3f);
                _cursor.Stretch = (_transition.Speed * 2f) -
                                  (_transition.Speed * 2f - 1) *
                                  2 * Math.Max(pct2Mid, 0.1f);
            }
            else if (_target != null && _target.Transition.IsAnimating)
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

            if (_transition.IsAnimating)
            {
                _cursor.Draw(spriteBatch, _position, _color);
            }

        }
    }
}

