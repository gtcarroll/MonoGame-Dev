using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class CardDrawAnimation : SpriteGroupAnimation
    {
        private SpriteGroupState _verticalDelta;
        private TimingFunction _verticalTransition;

        private Point _midPoint;
        private SpriteGroupState _verticalTarget;

        private SpriteGroupState _horizontalDelta;
        private TimingFunction _horizontalTransition;

        public CardDrawAnimation(SpriteGroup sprite, SpriteGroupState target,
                                 Point midPoint)
            : base(sprite, target, 256 + 640)
        {
            _midPoint = midPoint;

            _verticalTransition = new FlipFunction(256, true);
            _horizontalTransition = new BounceFunction(640);
        }

        public override void Begin()
        {
            IsStarted = true;

            //TODO: calc midpoint and use that instead of _deckCenter
            Current =
                Sprite.GetVerticallyFlattenedState().GetCopyAt(_midPoint);
            Start = Current;

            Point verticalPoint = new Point(Start.Center.X, Target.Center.Y);
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
            } else
            {
                IsCompleted = true;
            }
        }
    }
}

