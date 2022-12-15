using System;
using System.Collections.Generic;
using EverythingUnder.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class GraveSprite : SpriteGroup
    {
        public GraveSprite(Point center) : base(center) { }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/1k/graveyard")); // 128 x 148

            DefaultState = GetDefaultStyle();
            HoverState = GetHoverStyle();

            CurrentState = DefaultState.GetTranslatedCopy(Anchor);
        }

        private SpriteGroupState GetDefaultStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-64, -64, 128, 148)));

            return new SpriteGroupState(spriteStates, new Point(0, 0));
        }

        private SpriteGroupState GetHoverStyle()
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            spriteStates.Add(
                new SpriteState(Sprites[0], new Rectangle(-64, -88, 128, 148)));

            return new SpriteGroupState(spriteStates, new Point(0, 0));
        }
    }
}