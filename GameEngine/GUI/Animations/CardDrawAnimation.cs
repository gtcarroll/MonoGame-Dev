using Microsoft.Xna.Framework;

namespace EverythingUnder.GUI
{
    public class CardDrawAnimation : SpriteGroupAnimation
    {
        private SpriteGroup _startSprite;

        private SpriteGroupState _verticalDelta;
        private TimingFunction _verticalTransition;

        private SpriteGroupState _verticalTarget;

        private SpriteGroupState _horizontalDelta;
        private TimingFunction _horizontalTransition;

        public CardDrawAnimation(SpriteGroup sprite, SpriteGroupState target,
                                 SpriteGroup startSprite)
            : base(sprite, target, 256)
        {
            _startSprite = startSprite;

            _verticalTransition = new FlipFunction(256, true);
            _horizontalTransition = new BounceFunction(640);
        }

        public override void Begin()
        {
            IsStarted = true;

            // calc vertical point above start
            Point startingPt = _startSprite.CurrentState.Center;
            Point verticalPoint = new Point(startingPt.X, Target.Center.Y);

            // calc midpt of vertical animation
            Point midPoint = startingPt + verticalPoint;
            midPoint = new Point(midPoint.X / 2, midPoint.Y / 2);

            // set starting SpriteGroupStates
            Current = Sprite.GetVerticallyFlattenedState().GetCopyAt(midPoint);
            Start = Current;

            // calc animation deltas
            _verticalTarget = Sprite.DefaultState.GetCopyAt(verticalPoint);

            _verticalDelta = GetDelta(Start, _verticalTarget);
            _horizontalDelta = GetDelta(_verticalTarget, Target);
        }

        protected override void UpdateAnimation(GameTime time)
        {
            if (!_verticalTransition.IsCompleted)
            {
                _verticalTransition.Update(time);
                Current = CalcCurrentState(_verticalTransition.AnimationPosition, Start, _verticalDelta);
            }
            else if (!_horizontalTransition.IsCompleted)
            {
                _horizontalTransition.Update(time);
                Current = CalcCurrentState(_horizontalTransition.AnimationPosition, _verticalTarget, _horizontalDelta);
            }
            else
            {
                IsCompleted = true;
            }
        }
    }
}

