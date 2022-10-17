using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public static class Util
    {
        public static void ToggleFullScreen(GraphicsDeviceManager graphics)
        {
            graphics.HardwareModeSwitch = false;
            graphics.ToggleFullScreen();
        }

        public static Vector2 Transform2D(Vector2 position, FlatTransform transform)
        {
            return new Vector2(
                position.X * transform.CosScaleX - position.Y * transform.SinScaleY + transform.PosX,
                position.X * transform.SinScaleX + position.Y * transform.CosScaleY + transform.PosY);
        }

        public static float CrossProduct2D(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static T GetItem<T>(T[] array, int index)
        {
            int targetIndex = index % array.Length;
            if (index < 0) targetIndex += array.Length;
            return array[targetIndex];
        }

        public static T GetItem<T>(List<T> list, int index)
        {
            int targetIndex = index % list.Count;
            if (index < 0) targetIndex += list.Count;
            return list[targetIndex];
        }
    }
}

