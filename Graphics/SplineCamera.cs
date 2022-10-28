using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class SplineCamera : Camera
    {
        private CubicSpline3D _spline;
        private Vector3 _min;
        private Vector3 _max;

        private float _speed;

        public SplineCamera(Game game, Vector3[] points) : this(game)
        {
            LoadPoints(points);
            Position = _min;
        }
        public SplineCamera(Game game) : base(game)
        {
            _speed = 1f / 320f;
        }

        public void LoadPoints(Vector3[] points)
        {
            _spline = new CubicSpline3D(points);
            _min = points[0];
            _max = points[points.Length - 1];
        }

        public void Update()
        {
            if (Y < _min.Y) { Target = _min + DefaultTarget; }
            else if (Y > _max.Y) { Target = _max + DefaultTarget; }
            else { Target = null; }

            Position = _spline.Eval3D(Y);
            UpdateMatrices();
        }

        public void MoveForward(GameTime time)
        {
            Vector2 tangent = _spline.GetSlopeVector(Y);
            Y += tangent.Y * _speed * time.ElapsedGameTime.Milliseconds;
        }

        public void MoveBackward(GameTime time)
        {
            Vector2 tangent = _spline.GetSlopeVector(Y);
            Y -= tangent.Y * _speed * time.ElapsedGameTime.Milliseconds;
        }

        public void MoveTo(float y)
        {
            Y = y;
        }
    }
}

