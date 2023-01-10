using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    public class FlipUpAnimation : SpriteGroupAnimation
    {
        private SpriteGroup _startSprite;

        public FlipUpAnimation(SpriteGroup sprite, SpriteGroupState target,
                                 SpriteGroup startSprite)
            : base(sprite, target, new FlipFunction(256, true))
		{
            _startSprite = startSprite;
		}

        public override void Begin()
        {
            IsStarted = true;

            // calc midpt of vertical animation
            Point midPoint = _startSprite.CurrentState.Center + Target.Center;
            midPoint = new Point(midPoint.X / 2, midPoint.Y / 2);

            // set starting SpriteGroupStates
            Start = Sprite.GetVerticallyFlattenedState().GetCopyAt(midPoint);
            Current = Start;

            // calc animation delta
            Delta = GetDelta(Start, Target);
        }
    }
}

