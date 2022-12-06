using System;
using Microsoft.Xna.Framework;

using EverythingUnder.Combat;

namespace EverythingUnder.GUI
{
    public class CursorTransition
    {
        // speed constants
        private const float MinSpeed = 2f;
        private const float MaxSpeed = 12f;

        // cursor state
        public Point Current;
        public float Duration;

        // transition state
        public bool IsAnimating;
        public float PercentComplete;
        public float SinusoidalPercent;

        // transition parameters
        public Point Start;
        public Point Delta;
        public float Distance;
        public float Speed;

        public CursorTransition(Point position, float duration)
        {
            Current = position;
            Duration = duration;

            IsAnimating = false;
            PercentComplete = 0f;
            SinusoidalPercent = 0f;
        }

        //public void StartTransitionTo(SelectableSprite sprite)
        //{
        //    // set transition parameters
        //    Start = Current;
        //    Delta = sprite.Position - Start
        //            + new Point(sprite.Dimension.X / 2,
        //                        sprite.Dimension.Y / 2);
        //    Distance = MathF.Sqrt(Delta.X * Delta.X + Delta.Y * Delta.Y);
        //    Speed = MathHelper.Clamp(Distance / Duration, MinSpeed, MaxSpeed);

        //    // set initial transition state
        //    IsAnimating = true;
        //    PercentComplete = 0f;
        //}

        public void StartTransitionTo(SpriteGroup spriteGroup)
        {
            // set transition parameters
            Duration = spriteGroup.TransitionDuration;

            Start = Current;
            Delta = spriteGroup.TargetState.Center - Start;
            Distance = MathF.Sqrt(Delta.X * Delta.X + Delta.Y * Delta.Y);
            Speed = MathHelper.Clamp(Distance / Duration, MinSpeed, MaxSpeed);

            // set initial transition state
            IsAnimating = true;
            PercentComplete = 0f;
        }

        public void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            // update transition state
            PercentComplete += deltaTime * Speed / Distance;
            if (PercentComplete >= 1f) IsAnimating = false;
            PercentComplete = MathHelper.Clamp(PercentComplete, 0f, 1f);

            // sinusoidal movement
            SinusoidalPercent = 1 - (MathF.Cos(PercentComplete * MathHelper.Pi) + 1) / 2f;

            // update cursor state
            Point scaledDelta = new Point(
                (int)(Delta.X * SinusoidalPercent),//PercentComplete),
                (int)(Delta.Y * SinusoidalPercent));//PercentComplete));
            Current = Start + scaledDelta;
        }
    }
}

