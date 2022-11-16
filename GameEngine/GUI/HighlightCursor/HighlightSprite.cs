using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class HighlightSprite
    {
        public Texture2D Texture;
        public Point Dimension;
        public float Scale;

        public float Stretch;
        public float Rotation;

        public HighlightSprite(Texture2D texture, Point dimension)
            : this(texture, dimension, 1f) { }

        public HighlightSprite(Texture2D texture, Point dimension, float scale)
        {
            Texture = texture;
            Dimension = dimension;
            Scale = scale;
            Stretch = 1f;
            Rotation = 0f;
        }

        public void Draw(SpriteBatch spriteBatch, Point center, Color color)
        {
            Rectangle destination = GetRectangle(center);
             Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            spriteBatch.Draw(Texture, destination, null, color,
                             Rotation, origin, SpriteEffects.None, 0f);
        }

        public Rectangle GetRectangle(Point center)
        {
            int scaledX = (int)(Dimension.X * Scale * Stretch);
            int scaledY = (int)(Dimension.Y * Scale);

            Point newPosition = new Point(
                center.X - scaledX / 2,
                center.Y - scaledY / 2);
            Point newDimension = new Point(scaledX, scaledY);

            return new Rectangle(center, newDimension);
        }

        public void RotateTo(Point direction)
        {
            Rotation = MathF.Atan2(direction.Y, direction.X);
        }
    }
}

