using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class GraveNode : GUINode
    {
        private int width = 128;
        private int height = 148;

        public GraveNode(Point center) : base(center)
        {
            AddSprite(new GraveSprite(center));

            ScreenSpace = new Rectangle(center.X - width / 2 - 2,
                                        center.Y - width / 2 + 10,
                                        width + 4, height - 10);
        }
    }
}

