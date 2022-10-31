using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class Camera
    {
        // Directional vector camera faces if no target is set
        public static readonly Vector3 DefaultTarget =
            new Vector3(0f, 1f, -1f);//-MathF.Sqrt(3f));

        // Draw plane bounds
        private static readonly float NearPlane = .1f;
        private static readonly float FarPlane = 1024f;

        // Camera initialization values
        private readonly float _aspectRatio;
        private readonly float _fieldOfView;

        // Camera state vectors
        private Vector3 _position;
        private Vector3? _target;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }
        public float Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }
        public Vector3? Target
        {
            get { return _target - DefaultTarget; }
            set { _target = value + DefaultTarget; }
        }

        // Matrices for 3D rendering
        private Matrix _view;
        private Matrix _projection;

        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }

        public Camera(Game game)
        {
            _aspectRatio = (float)game.GraphicsDevice.Viewport.AspectRatio;
            _fieldOfView = MathHelper.PiOver2;

            Reset();
            UpdateMatrices();
        }

        public void Reset()
        {
            _position = Vector3.Zero;
            _target = null;
        }

        public void UpdateMatrices()
        {
            Vector3 target = _target == null
                ? _position + DefaultTarget
                : (Vector3)_target;

            _view = Matrix.CreateLookAt(_position, target, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, NearPlane, FarPlane);
        }
    }
}

