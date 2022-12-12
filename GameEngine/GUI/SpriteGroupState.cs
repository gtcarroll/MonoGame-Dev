using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;

namespace EverythingUnder.GUI
{
    public class SpriteState
    {
        public Texture2D Sprite;
        public Rectangle Destination;
        public Rectangle? Source;
        public Vector2 Origin;

        public SpriteState(Texture2D sprite, Rectangle destination, Rectangle? source = null)
        {
            Sprite = sprite;
            Destination = destination;
            Source = source;

            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }

    public class SpriteGroupState
    {
        public List<SpriteState> SpriteStates;

        public Color Color;
        public Point Center;
        //public float Rotation;
        //public SpriteEffects SpriteEffects;

        public SpriteGroupState(List<SpriteState> spriteStates, Point center)
            : this(spriteStates, Color.White, center) { }

        public SpriteGroupState(List<SpriteState> spriteStates, Color color,
                                                          Point center)
        {
            SpriteStates = spriteStates;

            Color = color;
            Center = center;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            foreach (SpriteState state in SpriteStates)
            {
                spriteBatch.Draw(state.Sprite, state.Destination, state.Source,
                                 color);
                //spriteBatch.Draw(state.Sprite, state.Destination, null, Color.White,
                //                 Rotation, state.Origin, SpriteEffects, 0f);
            }
        }

        public SpriteGroupState GetTransformedCopy(Point translate, Vector2 scale)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            foreach(SpriteState state in SpriteStates)
            {
                Point destLocation = new Point(
                    (int)(state.Destination.Location.X * scale.X) + translate.X,
                    (int)(state.Destination.Location.Y * scale.Y) + translate.Y);
                Point destSize = new Point(
                    (int)(state.Destination.Size.X * scale.X),
                    (int)(state.Destination.Size.Y * scale.Y));
                Rectangle newDestination = new Rectangle(destLocation, destSize);

                //Rectangle? newSource = null;
                //if (state.Source != null)
                //{
                //    Point sourceLocation = new Point(
                //        (int)(state.Destination.Location.X * scale.X) + translate.X,
                //        (int)(state.Destination.Location.Y * scale.Y) + translate.Y);
                //    Point sourceSize = new Point(
                //        (int)(state.Destination.Size.X * scale.X),
                //        (int)(state.Destination.Size.Y * scale.Y));
                //    newSource = new Rectangle(destLocation, destSize);
                //}

                newStates.Add(new SpriteState(state.Sprite, newDestination, state.Source));
            }

            return new SpriteGroupState(newStates, Color, Center + translate);
        }

        public SpriteGroupState GetCopyAt(Point destination)
        {
            return GetTranslatedCopy(destination - Center);
        }

        public SpriteGroupState GetTranslatedCopy(Point translate)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            foreach(SpriteState state in SpriteStates)
            {
                Rectangle newDestination = state.Destination;
                newDestination.Location += translate;

                newStates.Add(new SpriteState(state.Sprite, newDestination, state.Source));
            }

            return new SpriteGroupState(newStates, Color, Center + translate);
        }
    }
}

