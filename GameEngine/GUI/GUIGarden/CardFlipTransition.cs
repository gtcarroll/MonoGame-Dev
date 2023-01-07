//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace EverythingUnder.GUI
//{
//    public class CardFlipAnimation : SpriteGroupAnimation
//    {
//        public CardFlipAnimation(CardNode cardNode, Point startPt, Point endPt,
//                                  bool isFlippingUp = true)
//            : base()
//        {
//            // calculate midPt
//            Point deltaPt = endPt - startPt;
//            Point midPt = new Point(startPt.X + (deltaPt.X / 2),
//                                    startPt.Y + (deltaPt.Y / 2));

//            if (isFlippingUp)
//            {
//                // begin at midPt, starting with zero width
//                Start = cardNode.Sprite.DefaultState.GetCopyAt(midPt);
//                FlattenWidth(Start);
//                Current = Start;

//                // move to endPt, expanding back to full width
//                Delta = GetDelta(Current,
//                                 cardNode.Sprite.DefaultState.GetCopyAt(endPt));
//            }
//            else
//            {
//                // begin at startPt, starting with full width
//                Start = cardNode.Sprite.DefaultState.GetCopyAt(startPt);
//                FlattenWidth(Start);
//                Current = Start;

//                // move to midPt, shrinking to zero width
//                Delta = GetDelta(Current,
//                                 cardNode.Sprite.DefaultState.GetCopyAt(midPt));
//            }

//            Delay = 0;
//            Duration = 128;

//            PercentComplete = 0;

//            TimingFunction = new FlipFunction(256, isFlippingUp);
//        }

//        private void FlattenWidth(SpriteGroupState spriteGroupState)
//        {
//            foreach (SpriteState spriteState in spriteGroupState.SpriteStates)
//            {
//                spriteState.Destination = new Rectangle(
//                    spriteState.Destination.Location.X,
//                    spriteState.Destination.Location.Y,
//                    0,
//                    spriteState.Destination.Size.Y);
//            }
//        }

//        public override void Update(GameTime time)
//        {
//            float deltaTime = time.ElapsedGameTime.Milliseconds;

//            TimingFunction.Update(time);
//            Current = CalcCurrentState(TimingFunction.AnimationPercent, Start, Delta);
//        }
//    }
//}

