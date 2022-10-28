using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using EverythingUnder.Graphics;

namespace EverythingUnder.Input
{
    public sealed class FlatMouse
    {
        private static readonly Lazy<FlatMouse> Lazy = new Lazy<FlatMouse>(() => new FlatMouse());

        public static FlatMouse Instance
        {
            get { return Lazy.Value; }
        }

        private MouseState _prevState;
        private MouseState _currState;

        public Point WindowPosition
        {
            get { return _currState.Position; }
        }

        public FlatMouse()
        {
            _prevState = Mouse.GetState();
            _currState = _prevState;
        }

        public void Update()
        {
            _prevState = _currState;
            _currState = Mouse.GetState();
        }

        public bool IsLeftDown()
        {
            return _currState.LeftButton == ButtonState.Pressed;
        }
        public bool IsLeftClicked()
        {
            return _currState.LeftButton == ButtonState.Pressed && _prevState.LeftButton == ButtonState.Released;
        }

        public bool IsRightDown()
        {
            return _currState.RightButton == ButtonState.Pressed;
        }
        public bool IsRightClicked()
        {
            return _currState.RightButton == ButtonState.Pressed && _prevState.RightButton == ButtonState.Released;
        }

        public bool IsMiddleDown()
        {
            return _currState.MiddleButton == ButtonState.Pressed;
        }
        public bool IsMiddleClicked()
        {
            return _currState.MiddleButton == ButtonState.Pressed && _prevState.MiddleButton == ButtonState.Released;
        }

        //public Vector2 GetScreenPosition()
        //{
        //    Rectangle screenDestinationRectangle = screen.CalculateDestinationRectangle();

        //    Point windowPosition = this.WindowPosition;

        //    float sx = windowPosition.X - screenDestinationRectangle.X;
        //    float sy = windowPosition.Y - screenDestinationRectangle.Y;

        //    sx /= (float)screenDestinationRectangle.Width;
        //    sy /= (float)screenDestinationRectangle.Height;

        //    sx *= (float)screen.Width;
        //    sy *= (float)screen.Height;

        //    return new Vector2(sx, sy);
        //}
    }
}

