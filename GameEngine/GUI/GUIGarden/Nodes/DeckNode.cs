using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class DeckNode : GUINode
    {
        private int width = 128;
        private int height = 128;

        public DeckNode(Point center, bool isBottomRow = false) : base(center)
        {
            AddSprite(new CardBackSprite(center));

            int topY = center.Y - height / 2;
            int hitboxH = height - 10;
            if (isBottomRow)
            {
                topY += 10;
                hitboxH += 20;
            }
            ScreenSpace = new Rectangle(center.X - width / 2 - 2, topY,
                                        0, 0); //width + 4, hitboxH);
        }
    }
}

