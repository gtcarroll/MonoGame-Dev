using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public sealed class Camera
    {
        public readonly static float MinZ = 1f;
        public readonly static float MaxZ = 2048f;

        public readonly static float MinZoom = 1;
        public readonly static float MaxZoom = 6;

        private Game _game;
        private Viewport _viewport;

        private Vector2 _position;
        private float _z;
        private float _baseZ;

        private float _aspectRatio;
        private float _fieldOfView;
        private float _zoom;

        private float _tiltX;
        private float _tiltY;
        private Vector2 _pan;

        private Matrix _view;
        private Matrix _proj;

        public Vector2 Position
        {
            get { return _position; }
        }
        public float Z
        {
            get { return _z; }
        }
        public float Zoom
        {
            get { return _zoom; }
        }
        public Matrix View
        {
            get { return _view; }
        }
        public Matrix Proj
        {
            get { return _proj; }
        }

        public Camera(Game game)
        {
            _game = game;
            _viewport = _game.GraphicsDevice.Viewport;

            _aspectRatio = (float)_viewport.AspectRatio;
            _fieldOfView = MathHelper.PiOver2;
            _zoom = 1;

            _position = new Vector2(0, 0);
            _baseZ = GetZFromHeight(_viewport.Height);
            //_z = _baseZ;

            Reset();
        }
        public void Reset()
        {
            _z = _baseZ;
            _tiltX = 0;
            _tiltY = 0;
            _pan = new Vector2(0, 0);
        }

        public void UpdateMatrices()
        {
            _view = Matrix.CreateLookAt(new Vector3(0f, 0f, _z), Vector3.Zero, Vector3.Up)
                * Matrix.CreateRotationX(_tiltX)
                * Matrix.CreateRotationY(_tiltY)
                * Matrix.CreateTranslation(new Vector3(_pan, 0f));
            _proj = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, MinZ, MaxZ);
        }

        public float GetZFromHeight(float height)
        {
            return (0.5f * height) / MathF.Tan(0.5f * _fieldOfView);
        }
        public float GetHeightFromZ()
        {
            return _z * MathF.Tan(0.5f * _fieldOfView) * 2f;
        }

        public void MoveZ(float amount)
        {
            _z += amount;
            _z = MathHelper.Clamp(_z, MinZ, MaxZ);
        }
        public void ResetZ()
        {
            _z = _baseZ;
        }

        public void Move(Vector2 amount)
        {
            _position += amount;
        }
        public void MoveTo(Vector2 position)
        {
            _position = position;
        }

        public void TiltX(float amount)
        {
            _tiltX += amount;
        }
        public void TiltY(float amount)
        {
            _tiltY += amount;
        }
        public void PanX(float amount)
        {
            _pan.X += amount;
        }
        public void PanY(float amount)
        {
            _pan.Y += amount;
        }

        public void ZoomIn()
        {
            _zoom++;
            UpdateZoom();
        }
        public void ZoomOut()
        {
            _zoom--;
            UpdateZoom();
        }
        public void SetZoom(float zoom)
        {
            _zoom = zoom;
            UpdateZoom();
        }
        private void UpdateZoom()
        {
            _zoom = MathHelper.Clamp(_zoom, MinZoom, MaxZoom);
            _z = _baseZ / _zoom;
        }

        public void GetExtents(out float width, out float height)
        {
            height = GetHeightFromZ();
            width = height * _aspectRatio;
        }

        public void GetExtents(out float left, out float right, out float bottom, out float top)
        {
            GetExtents(out float width, out float height);

            left = _position.X - (width / 2f);
            right = _position.X + (width / 2f);
            top = _position.Y - (height / 2f);
            bottom = _position.Y + (height / 2f);
        }

        public void GetExtents(out Vector2 min, out Vector2 max)
        {
            GetExtents(out float left, out float right, out float bottom, out float top);

            min = new Vector2(left, top);
            max = new Vector2(right, bottom);
        }
    }
}

