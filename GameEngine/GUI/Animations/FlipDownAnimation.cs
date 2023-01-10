using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    public class FlipDownAnimation : SpriteGroupAnimation
    {
        private Point _targetPt;

        public FlipDownAnimation(SpriteGroup sprite, Point targetPt)
            : base(sprite, null, new FlipFunction(256, false))
		{
            _targetPt = targetPt;
		}

        public override void Begin()
        {
            IsStarted = true;

            // calc midpt of vertical animation
            Point midPoint = Sprite.CurrentState.Center + _targetPt;
            midPoint = new Point(midPoint.X / 2, midPoint.Y / 2);

            // set starting SpriteGroupStates
            Start = Sprite.CurrentState;
            Current = Start;

            // set target SpriteGroupState
            Target = Sprite.GetVerticallyFlattenedState().GetCopyAt(midPoint);

            // calc animation delta
            Delta = GetDelta(Start, Target);
        }
    }
}

