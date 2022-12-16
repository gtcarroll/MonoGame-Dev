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

        public CardNode(Point center, bool isBottomRow = false) : base(center)
        {
            AddSprite(new CardSprite(center));

            int topY = center.Y - height / 2;
            if (isBottomRow) topY += 10;
            ScreenSpace = new Rectangle(center.X - width / 2 - 2, topY,
                                        width + 4, height - 10);
        }
    }
}

