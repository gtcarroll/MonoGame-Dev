using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class SpriteGroupTransition
    {
        // SpriteGroup state
        public GroupState Current;
        public float Duration;

        // transition state
        public bool IsAnimating
        {
            get { return TimingFunction.IsAnimating; }
        }
        public float PercentComplete;

        // transition parameters
        public GroupState Start;
        public GroupState Delta;

        // timing function
        public TimingFunction TimingFunction;

        public SpriteGroupTransition(GroupState start, GroupState end) : this(start, end, 128f, null) { }
        public SpriteGroupTransition(GroupState start, GroupState end, float duration, TimingFunction timingFunction = null)
        {
            Current = start;
            Duration = duration;

            PercentComplete = 0f;

            Start = start;
            Delta = GetDelta(start, end);

            TimingFunction = timingFunction == null ? new LinearFunction(duration) : timingFunction;
        }

        public virtual void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            TimingFunction.Update(time);

            // update SpriteGroup state
            Current = GetUpdatedGroupState(TimingFunction.AnimationPosition, Start, Delta);
        }

        protected GroupState GetUpdatedGroupState(float percentComplete, GroupState start, GroupState delta)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            for (int i = 0; i < delta.SpriteStates.Count; i++)
            {
                Texture2D sprite = delta.SpriteStates[i].Sprite;

                Rectangle startDest = start.SpriteStates[i].Destination;
                Rectangle deltaDest = delta.SpriteStates[i].Destination;
                Rectangle scaledDest = ScaleRectangle(deltaDest,
                                                      percentComplete);
                Rectangle currDest = new Rectangle(
                    startDest.Location + scaledDest.Location,
                    startDest.Size + scaledDest.Size);

                Rectangle? currSource = null;
                if (delta.SpriteStates[i].Source != null)
                {
                    Rectangle startSource =
                        (Rectangle)start.SpriteStates[i].Source;
                    Rectangle deltaSource =
                        (Rectangle)delta.SpriteStates[i].Source;
                    Rectangle scaledSource = ScaleRectangle(deltaSource,
                                                            percentComplete);
                    currSource = new Rectangle(
                        startSource.Location + scaledSource.Location,
                        startSource.Size + scaledSource.Size);
                }

                newStates.Add(new SpriteState(sprite, currDest, currSource));
            }

            Point scaledCenter = new Point(
                (int)(delta.Center.X * percentComplete),
                (int)(delta.Center.Y * percentComplete));

            return new GroupState(newStates, Start.Center + scaledCenter);
        }

        protected GroupState GetDelta(GroupState start, GroupState end)
        {
            List<SpriteState> deltaStates = new List<SpriteState>();

            for (int i = 0; i < start.SpriteStates.Count; i++)
            {
                Texture2D sprite = end.SpriteStates[i].Sprite;

                Rectangle startDest = start.SpriteStates[i].Destination;
                Rectangle endDest = end.SpriteStates[i].Destination;

                Rectangle deltaDest = new Rectangle(
                    endDest.Location - startDest.Location,
                    endDest.Size - startDest.Size);

                Rectangle? deltaSource = null;
                if (start.SpriteStates[i].Source != null &&
                    end.SpriteStates[i].Source != null)
                {
                    Rectangle startSource =
                        (Rectangle)start.SpriteStates[i].Source;
                    Rectangle endSource =
                        (Rectangle)end.SpriteStates[i].Source;

                    deltaSource = new Rectangle(
                        endSource.Location - startSource.Location,
                        endSource.Size - startSource.Size);
                }

                deltaStates.Add(new SpriteState(sprite, deltaDest,
                                                        deltaSource));
            }

            return new GroupState(deltaStates, end.Center - start.Center);
        }

        protected Rectangle ScaleRectangle(Rectangle rectangle, float scale)
        {
            return new Rectangle(
                (int)(rectangle.Location.X * scale),
                (int)(rectangle.Location.Y * scale),
                (int)(rectangle.Size.X * scale),
                (int)(rectangle.Size.Y * scale));
        }
    }
}

