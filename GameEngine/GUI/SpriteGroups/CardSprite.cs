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
            Sprites.Add(content.Load<Texture2D>("Textures/1k/card-banner")); // 256 x 372
            Sprites.Add(content.Load<Texture2D>("Textures/1k/card-art_empty")); // 256 x 256
            Sprites.Add(content.Load<Texture2D>("Textures/1k/card-frame")); // 256 x 256

            DefaultState = GetDefaultStyle();
            HoverState = GetHoverStyle();

            CurrentState = DefaultState.GetTranslatedCopy(Anchor);
        }

        protected override void BeginHoverAnimation(SpriteGroupState targetState)
        {
            Animation = new SpriteGroupAnimation(this, targetState,
                                                 new CosineFunction(128));
        }

        protected override void BeginRepositionAnimation(SpriteGroupState targetState)
        {
            Animation = new SpriteGroupAnimation(this, targetState,
                                                 new BounceFunction(640));
        }

        public void BeginDrawAnimation(Point deckCenter, SpriteGroupState targetState)
        {
            Animation = new CardDrawAnimation(this, targetState, deckCenter);
        }

        private SpriteGroupState GetDefaultStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-64, 38, 128, 26),
                                new Rectangle(0, Sprites[0].Height - 52, Sprites[0].Width, 52)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-64, -64, 128, 128)));
            spriteStates.Add(
                new SpriteState(Sprites[2], new Rectangle(-64, -64, 128, 128)));

            return new SpriteGroupState(spriteStates, new Point(0,0));
        }

        private SpriteGroupState GetHoverStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-128, -308, 256, 372),
                                new Rectangle(0, 0, Sprites[0].Width, Sprites[0].Height)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-128, -512, 256, 256)));
            spriteStates.Add(
                new SpriteState(Sprites[2], new Rectangle(-128, -512, 256, 256)));

            return new SpriteGroupState(spriteStates, new Point(0, 0));
        }
    }
}

