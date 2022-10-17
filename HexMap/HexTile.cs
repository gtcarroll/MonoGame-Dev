using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.HexMap
{
    public struct HexTile
    {
        public float Height { get; set; }
        public Color Color { get; set; }

        public HexTile() : this(0) { }
        public HexTile(float noise)
        {
            Height = noise;
            Color = Color.White;
        }
    }
}

