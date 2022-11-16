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
        public CardSprite(Card card, Point center) : base(center)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/card-banner"));
            Sprites.Add(content.Load<Texture2D>("Textures/simple-squashed-hex"));
            // conent.Load<Texture2D>("card-image-bg");
            // conent.Load<Texture2D>("card-image-fg");

            StyleStates.Add(SpriteStyle.Default, GetDefaultStyle());
            StyleStates.Add(SpriteStyle.Hover, GetHoverStyle());
            //StyleStates.Add(SpriteStyle.Highlight, GetHighlightStyle());

            CurrentState = GetNextState();
        }

        private GroupState GetDefaultStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, 50, 92, 0),
                                new Rectangle(0, Sprites[0].Height, Sprites[0].Width, 0)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-50, -30, 100, 100)));

            return new GroupState(spriteStates, new Point(0,0));
        }

        private GroupState GetHoverStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-46, -100, 92, 150),
                                new Rectangle(0, Sprites[0].Height - 1670, Sprites[0].Width, 1670)));

            spriteStates.Add(
                new SpriteState(Sprites[1], new Rectangle(-50, -180, 100, 100)));

            return new GroupState(spriteStates, new Point(0, -75));
        }

        //private GroupState GetHighlightStyle(int border = 5)
        //{
        //    List<SpriteState> spriteStates = new List<SpriteState>();

        //    spriteStates.Add(
        //        new SpriteState(Sprites[0], new Rectangle(-46 - border,
        //                                                  -100 - border,
        //                                                  92 + 2 * border,
        //                                                  150 + 2 * border)));

        //    spriteStates.Add(
        //        new SpriteState(Sprites[1], new Rectangle(-50 - border,
        //                                                  -180 - border,
        //                                                  100 + 2 * border,
        //                                                  100 + 2 * border)));

        //    return new GroupState(spriteStates, new Point(0, -75));
        //}
    }
}

