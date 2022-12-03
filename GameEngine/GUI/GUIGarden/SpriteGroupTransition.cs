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
        public ITimingFunction TimingFunction;

        public SpriteGroupTransition(GroupState start, GroupState end, float duration, ITimingFunction timingFunction = null)
        {
            Current = start;
            Duration = duration;

            PercentComplete = 0f;

            Start = start;
            Delta = GetDelta(start, end);

            TimingFunction = timingFunction == null ? new LinearFunction(duration) : timingFunction;
        }

        public void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            //// update transition state
            //PercentComplete += deltaTime / Duration;
            //if (PercentComplete >= 1f) IsAnimating = false;
            //PercentComplete = MathHelper.Clamp(PercentComplete, 0f, 1f);

            TimingFunction.Update(time);

            // update SpriteGroup state
            //Current = GetUpdatedGroupState(PercentComplete);
            Current = GetUpdatedGroupState(TimingFunction.AnimationPosition);
        }

        private GroupState GetUpdatedGroupState(float percentComplete)
        {
            List<SpriteState> newStates = new List<SpriteState>();

            for (int i = 0; i < Delta.SpriteStates.Count; i++)
            {
                Texture2D sprite = Delta.SpriteStates[i].Sprite;

                Rectangle startDest = Start.SpriteStates[i].Destination;
                Rectangle deltaDest = Delta.SpriteStates[i].Destination;
                Rectangle scaledDest = ScaleRectangle(deltaDest,
                                                      percentComplete);
                Rectangle currDest = new Rectangle(
                    startDest.Location + scaledDest.Location,
                    startDest.Size + scaledDest.Size);

                Rectangle? currSource = null;
                if (Delta.SpriteStates[i].Source != null)
                {
                    Rectangle startSource =
                        (Rectangle)Start.SpriteStates[i].Source;
                    Rectangle deltaSource =
                        (Rectangle)Delta.SpriteStates[i].Source;
                    Rectangle scaledSource = ScaleRectangle(deltaSource,
                                                            percentComplete);
                    currSource = new Rectangle(
                        startSource.Location + scaledSource.Location,
                        startSource.Size + scaledSource.Size);
                }

                newStates.Add(new SpriteState(sprite, currDest, currSource));
            }

            Point scaledCenter = new Point(
                (int)(Delta.Center.X * percentComplete),
                (int)(Delta.Center.Y * percentComplete));

            return new GroupState(newStates, Start.Center + scaledCenter);
        }

        private GroupState GetDelta(GroupState start, GroupState end)
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

        private Rectangle ScaleRectangle(Rectangle rectangle, float scale)
        {
            return new Rectangle(
                (int)(rectangle.Location.X * scale),
                (int)(rectangle.Location.Y * scale),
                (int)(rectangle.Size.X * scale),
                (int)(rectangle.Size.Y * scale));
        }
    }
}

