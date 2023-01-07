using System;
using System.Collections.Generic;
using EverythingUnder.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class DeckSprite : SpriteGroup
    {
        private static readonly Point _defaultBasis = new Point(-64, -64);
        private static readonly Point _defaultDelta = new Point(0, 6);

        private static readonly Point _hoveredBasis = new Point(-64, -88);
        private static readonly Point _hoveredDelta = new Point(0, 9);

        private static readonly Point _dimensions = new Point(128, 128);

        private int _size;
        public int Size
        {
            get { return _size; }
            set
            {
                DefaultState = GetDefaultStyle();
                HoverState = GetHoverStyle();
            }
        }

        public DeckSprite(Point center) : base(center) { }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/1k/card-back")); // 256 x 256

            _size = 0;

            DefaultState = GetDefaultStyle();
            HoverState = GetHoverStyle();

            CurrentState = DefaultState.GetTranslatedCopy(Anchor);
        }

        private SpriteGroupState GetDefaultStyle()
        {
            return GetStyle(_defaultBasis, _defaultDelta);
        }

        private SpriteGroupState GetHoverStyle()
        {
            return GetStyle(_hoveredBasis, _hoveredDelta);
        }

        private SpriteGroupState GetStyle(Point basis, Point delta)
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            Point currPosition = new Point(basis.X, basis.Y + (_size - 1) * delta.Y);
            for (int i = 0; i < _size; i++)
            {
                spriteStates.Add(new SpriteState(Sprites[0], new Rectangle(currPosition, _dimensions)));
                currPosition -= delta;
            }

            return new SpriteGroupState(spriteStates, new Point(0, 0));
        }
    }
}