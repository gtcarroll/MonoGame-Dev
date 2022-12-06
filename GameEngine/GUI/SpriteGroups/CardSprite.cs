using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class CardSprite : SpriteGroup
    {
        public CardSprite(Point center) : base(center, 128f) { }
        public CardSprite(Card card, Point center) : base(center, 128f) { }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/card-banner"));
            Sprites.Add(content.Load<Texture2D>("Textures/simple-squashed-hex"));
            // conent.Load<Texture2D>("card-image-bg");
            // conent.Load<Texture2D>("card-image-fg");

            StyleStates.Add(SpriteStyle.Default, GetDefaultStyle());
            StyleStates.Add(SpriteStyle.Hover, GetHoverStyle());
            //StyleStates.Add(SpriteStyle.Highlight, GetHighlightStyle());

            DefaultState = StyleStates[SpriteStyle.Default];
            CurrentState = GetNextState();
        }

        public override void BeginTransition(GroupState endState)
        {
            TargetState = endState;
            Transition = new SpriteGroupTransition(CurrentState, endState,
                                                   128f, new SinusoidalFunction(128f));
        }

        public void BeginDrawAnimation(GroupState deckState, GroupState endState)
        {
            TargetState = endState;
            Transition = new CardDrawTransition(deckState, endState);
        }

        public override void BeginRepositionAnimation(GroupState endState)
        {
            TargetState = endState;
            Transition = new SpriteGroupTransition(CurrentState, endState,
                                                   640f, new BounceFunction(640f));
        }

        private GroupState GetDefaultStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, 30, 92, 20),
                                new Rectangle(0, Sprites[0].Height - 223, Sprites[0].Width, 223)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-64, -64, 128, 128)));

            return new GroupState(spriteStates, new Point(0,0));
        }

        private GroupState GetHoverStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, -120, 92, 170),
                                new Rectangle(0, Sprites[0].Height - 1893, Sprites[0].Width, 1893)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-50, -200, 100, 100)));

            return new GroupState(spriteStates, new Point(0, -75));
        }
    }
}

