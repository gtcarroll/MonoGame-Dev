using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class SplineCamera : Camera
    {
        // constants
        private const float StartOffset = 4f;

        // spline properties
        private CubicSpline3D _spline;
        private Vector3 _min;
        private Vector3 _max;

        // camera properties
        private float _speed;
        private float _yHome;

        public float T { get; set; }

        // animation properties
        private bool _isAnimating;
        private bool _isTargetAhead;
        private float _targetY;
        private float _animSpeedFactor;

        public bool IsAnimating
        {
            get { return _isAnimating; }
        }
        public bool IsReturned
        {
            get { return Y < _yHome + 0.1 && Y > _yHome - 0.1; }
        }

        public SplineCamera(Game game, Vector3[] points) : base(game)
        {
            LoadPoints(points);

            _speed = 1f / 360f;
            _yHome = _min.Y;

            // setup opening zoom shot
            T = _min.Y - StartOffset;
            Position = _spline.Eval3D(T);
            Return(1f);
        }

        public void LoadPoints(Vector3[] points)
        {
            _spline = new CubicSpline3D(points, 0, 0);
            _min = points[0];
            _max = points[points.Length - 1];
        }

        public void Update(GameTime time)
        {
            if (_isAnimating) UpdateAnimate(time);

            Position = _spline.Eval3D(T);

            if (T < _min.Y)
            {
                Position = new Vector3(_min.X, Y, Z + Math.Abs(_min.X - Position.X));
                Target = _min;
            }
            else if (T > _max.Y)
            {
                Position = new Vector3(_max.X, Y, Z - Math.Abs(_max.X - Position.X));
                Target = _max;
            }
            else
            {
                Target = null;
            }

            UpdateMatrices();
        }

        public void Return(float speedFactor = 2f)
        {
            AnimateTo(_yHome, speedFactor);
        }

        public void AnimateTo(float targetY, float speedFactor = 1)
        {
            _isAnimating = true;
            _isTargetAhead = targetY > T;
            _targetY = targetY;
            _animSpeedFactor = speedFactor;

            _yHome = targetY;
        }

        public void UpdateAnimate(GameTime time)
        {
            if (_isTargetAhead)
            {
                MoveForward(time, _animSpeedFactor);
                if (T >= _targetY) { EndAnimate(); }
            }
            else
            {
                MoveBackward(time, _animSpeedFactor);
                if (T <= _targetY) { EndAnimate(); }
            }
        }

        private void EndAnimate()
        {
            T = _targetY;
            _isAnimating = false;
        }

        public void MoveForward(GameTime time, float speedFactor = 1)
        {
            T += GetMoveAmount(time, speedFactor);
        }

        public void MoveBackward(GameTime time, float speedFactor = 1)
        {
            T -= GetMoveAmount(time, speedFactor);
        }

        private float GetMoveAmount(GameTime time, float speedFactor = 1)
        {
            Vector2 tangent = _spline.GetSlopeVector(T);
            float yComponent = Math.Max(tangent.Y, 0.25f);
            return yComponent * (_speed * speedFactor) * time.ElapsedGameTime.Milliseconds;
        }

        public void JumpTo(float y)
        {
            T = y;
        }
    }
}

