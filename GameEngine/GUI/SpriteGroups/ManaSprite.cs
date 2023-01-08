using System;
using System.Collections.Generic;
using EverythingUnder.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public class ManaSprite : SpriteGroup
    {
        private static readonly Point _defaultBasis = new Point(-64, 8);
        private static readonly Point _defaultDelta = new Point(0, 24);
        private static readonly Point _hoveredDelta = new Point(0, 36);

        private static readonly Point _dimensions = new Point(128, 56);

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

        public ManaSprite(Point center) : base(center) { }

        public override void LoadContent(ContentManager content)
        {
            Sprites.Add(content.Load<Texture2D>("Textures/1k/mana-tile")); // 128 x 56

            _size = 3;

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
            if (_size == 1)
            {
                List<SpriteState> spriteStates = new List<SpriteState>();

                Point hoverPosition = _defaultBasis - _defaultDelta;
                spriteStates.Add(
                    new SpriteState(Sprites[0], new Rectangle(hoverPosition, _dimensions)));

                return new SpriteGroupState(spriteStates, new Point(0, 0));
            }

            return GetStyle(_defaultBasis, _hoveredDelta);
        }

        private SpriteGroupState GetStyle(Point basis, Point delta)
        {
            List<SpriteState> spriteStates = new List<SpriteState>();

            Point currPosition = basis;
            for (int i = 0; i < _size; i++)
            {
                spriteStates.Add(new SpriteState(Sprites[0], new Rectangle(currPosition, _dimensions)));
                currPosition -= delta;
            }

            return new SpriteGroupState(spriteStates, new Point(0, 0));
        }
    }
}

