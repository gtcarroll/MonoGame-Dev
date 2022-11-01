using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class SplineCamera : Camera
    {
        // spline properties
        private CubicSpline3D _spline;
        private Vector3 _min;
        private Vector3 _max;

        // camera properties
        private float _speed;
        private float _yHome;

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
            Position = _min;

            _speed = 1f / 320f;
            _yHome = Y;
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

            if (Y < _min.Y) { Target = _min; }
            else if (Y > _max.Y) { Target = _max; }
            else { Target = null; }

            Position = _spline.Eval3D(Y);
            UpdateMatrices();
        }

        public void Return()
        {
            AnimateTo(_yHome, 2);
        }

        public void AnimateTo(float targetY, float speedFactor = 1)
        {
            _isAnimating = true;
            _isTargetAhead = targetY > Y;
            _targetY = targetY;
            _animSpeedFactor = speedFactor;

            _yHome = targetY;
        }

        public void UpdateAnimate(GameTime time)
        {
            if (_isTargetAhead)
            {
                MoveForward(time, _animSpeedFactor);
                if (Y >= _targetY) { EndAnimate(); }
            }
            else
            {
                MoveBackward(time, _animSpeedFactor);
                if (Y <= _targetY) { EndAnimate(); }
            }
            
        }

        private void EndAnimate()
        {
            Y = _targetY;
            _isAnimating = false;
        }

        public void MoveForward(GameTime time, float speedFactor = 1)
        {
            Y += GetMoveAmount(time, speedFactor);
        }

        public void MoveBackward(GameTime time, float speedFactor = 1)
        {
            Y -= GetMoveAmount(time, speedFactor);
        }

        private float GetMoveAmount(GameTime time, float speedFactor = 1)
        {
            Vector2 tangent = _spline.GetSlopeVector(Y);
            return tangent.Y * (_speed * speedFactor) * time.ElapsedGameTime.Milliseconds;
        }

        public void JumpTo(float y)
        {
            Y = y;
        }
    }
}

