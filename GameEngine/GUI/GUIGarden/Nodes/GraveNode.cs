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
        private int height = 128;

        public GraveNode(Point center) : base(center)
        {
            AddSprite(new GraveSprite(center));

            ScreenSpace = new Rectangle(center.X - width / 2,
                                        center.Y - height / 2,
                                        width, height);
        }
    }
}

