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
        public CardSprite(Point center) : base(center) { }
        public CardSprite(Card card, Point center) : base(center) { }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/card-banner"));
            Sprites.Add(content.Load<Texture2D>("Textures/simple-squashed-hex"));

            DefaultState = GetDefaultStyle();
            HoverState = GetHoverStyle();

            CurrentState = DefaultState.GetTranslatedCopy(Anchor);
        }

        protected override void BeginHoverAnimation(SpriteGroupState targetState)
        {
            Transition = new SpriteGroupTransition(CurrentState, targetState,
                                                   128f, new CosineFunction(128f));
        }

        protected override void BeginRepositionAnimation(SpriteGroupState targetState)
        {
            Transition = new SpriteGroupTransition(CurrentState, targetState,
                                                   640f, new BounceFunction(640f));
        }

        public void BeginDrawAnimation(SpriteGroupState deckState, SpriteGroupState targetState)
        {
            Transition = new CardDrawTransition(deckState, targetState);
        }

        private SpriteGroupState GetDefaultStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, 30, 92, 20),
                                new Rectangle(0, Sprites[0].Height - 223, Sprites[0].Width, 223)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-64, -64, 128, 128)));

            return new SpriteGroupState(spriteStates, new Point(0,0));
        }

        private SpriteGroupState GetHoverStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, -120, 92, 170),
                                new Rectangle(0, Sprites[0].Height - 1893, Sprites[0].Width, 1893)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-64, -200, 128, 128)));

            return new SpriteGroupState(spriteStates, new Point(0, -75));
        }
    }
}

