using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class CardDrawTransition : SpriteGroupTransition
    {
        private GroupState _verticalDelta;
        private TimingFunction _verticalTransition;

        private GroupState _verticalTarget;

        private GroupState _horizontalDelta;
        private TimingFunction _horizontalTransition;

        public CardDrawTransition(GroupState start, GroupState end) : base(start, end, 128f, null)
        {
            Current = start;

            PercentComplete = 0f;

            Start = start;
            Delta = GetDelta(start, end);

            Point verticalPoint = new Point(start.Center.X, end.Center.Y);
            _verticalTarget = start.GetCopyAt(verticalPoint);

            _verticalDelta = GetDelta(start, _verticalTarget);
            _verticalTransition = new SinusoidalFunction(128f);

            _horizontalDelta = GetDelta(_verticalTarget, end);
            _horizontalTransition = new BounceFunction(640f);
        }

        public override void Update(GameTime time)
        {
            float deltaTime = time.ElapsedGameTime.Milliseconds;

            if (_verticalTransition.IsAnimating)
            {
                _verticalTransition.Update(time);
                Current = GetUpdatedGroupState(_verticalTransition.AnimationPosition, Start, _verticalDelta);
            }
            else if (_horizontalTransition.IsAnimating)
            {
                _horizontalTransition.Update(time);
                Current = GetUpdatedGroupState(_horizontalTransition.AnimationPosition, _verticalTarget, _horizontalDelta);
            }

            // update SpriteGroup state
            //Current = GetUpdatedGroupState(TimingFunction.AnimationPosition);
        }
    }
}

