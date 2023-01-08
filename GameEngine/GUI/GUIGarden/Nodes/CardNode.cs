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

        public CardNode(Point center, bool isBottomRow = false, bool card = true) : base(center)
        {
            if (card)// == null)
            {
                AddSprite(new CardSprite(center));
            }
            else
            {
                AddSprite(new CardBackSprite(center));
            }

            int topY = center.Y - height / 2;
            int screenY = height - 10;
            if (isBottomRow)
            {
                topY += 10;
                screenY += 20;
            }
            ScreenSpace = new Rectangle(center.X - width / 2 - 2, topY,
                                        width + 4, screenY);
        }
    }
}

