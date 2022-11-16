using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class CardNode : GUINode
    {
        public CardNode(Point center) : base(center)
        {
            // set OnSelected event listener
            // load card image data?
            AddSprite(new CardSprite(center));
        }
    }
}

