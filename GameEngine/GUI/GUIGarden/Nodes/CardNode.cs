using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class CardNode : GUINode
    {
        private int width = 128;
        private int height = 128;

        public CardNode(Point center) : base(center)
        {
            AddSprite(new CardSprite(center));

            ScreenSpace = new Rectangle(center.X - width / 2,
                                        center.Y - height / 2,
                                        width, height);
        }
    }
}

