using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Combat
{
    public class CursorTransition
    {
        // speed constants
        private const float MinSpeed = 3f;
        private const float MaxSpeed = 12f;

        // cursor state
        public Point Current;
        public float Duration;

        // transition state
        public bool IsAnimating;
        public float PercentComplete;

        // transition parameters
        public Point Start;
        public Point Delta;
        public float Distance;
        public float Speed;

        public CursorTransition(Point position, float duration)
        {
            Current = position;
            Duration = duration;

            IsAnimating = false;
            PercentComplete = 0f;
        }

        public void StartTransitionTo(SelectableSprite sprite)
        {
            // set transition parameters
            Start = Current;
            Delta = sprite.Position - Start
                    + new Point(sprite.Dimension.X / 2,
                                sprite.Dimension.Y / 2);
            Distance = MathF.Sqrt(Delta.X * Delta.X + Delta.Y * Delta.Y);
            Speed = MathHelper.Clamp(Distance / Duration, MinSpeed, MaxSpeed);

            // set initial transition state
            IsAnimating = true;
            PercentComplete = 0f;
        }

        public void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            // update transition state
            PercentComplete += deltaTime * Speed / Distance;
            if (PercentComplete >= 1f) IsAnimating = false;
            PercentComplete = MathHelper.Clamp(PercentComplete, 0f, 1f);

            // update cursor state
            Point scaledDelta = new Point(
                (int)(Delta.X * PercentComplete),
                (int)(Delta.Y * PercentComplete));
            Current = Start + scaledDelta;
        }
    }

    public class HighlightCursor
    {
        // cursor state
        private Point _position;
        private Color _color;
        private int _thickness;

        // highlighted sprites
        private HighlightSprite _curr;
        private HighlightSprite _prev;
        private HighlightSprite _cursor;

        // transition state
        private CursorTransition _transition;

        public HighlightCursor(GameManager game)
            : this(game, Color.CornflowerBlue, 10) { }

        public HighlightCursor(GameManager game, Color color, int thickness)
        {
            // set initial highlighted sprites
            // TODO: move load out of constructor
            Texture2D cursor = game.Content.Load<Texture2D>("simple-circle");
            _curr = new HighlightSprite(cursor, new Point(100));
            _prev = new HighlightSprite(cursor, new Point(100));
            _cursor = new HighlightSprite(cursor, new Point(100));

            // set highlight parameters
            _color = color;
            _thickness = thickness;

            // set initial highlight state
            _position = new Point(0, 0);
            _transition = new CursorTransition(_position, 100f);
        }

        public void AnimateTo(SelectableSprite sprite)
        {
            // begin transition
            _transition.StartTransitionTo(sprite);

            // update sprites
            _prev = _curr;
            _curr = sprite.GetHighlight();

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

                _prev.Scale = Math.Max(
                                1f - (1.5f * dist2Start / _prev.Dimension.X),
                                0f); 
                _curr.Scale = Math.Max(
                                1f - (1.5f * dist2End / _curr.Dimension.X),
                                0f); 

                // scale and stretch cursor
                float pct2Mid = Math.Abs(_transition.PercentComplete - 0.5f);

                _cursor.Scale = Math.Max(pct2Mid * 2, 0.3f);
                _cursor.Stretch = (_transition.Speed * 2f) -
                                  (_transition.Speed * 2f - 1) *
                                  2 * Math.Max(pct2Mid, 0.1f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _curr.Draw(spriteBatch, _position, _color);

            if (_transition.IsAnimating)
            {
                _prev.Draw(spriteBatch, _position, _color);
            }

            _cursor.Draw(spriteBatch, _position, _color);
        }
    }
}

