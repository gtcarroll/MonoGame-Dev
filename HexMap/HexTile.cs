using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap
{
    public enum TileType
    {
        Default,
        Path,
        Wall
    }

    public struct HexTile
    {
        public float Height { get; set; }
        public Color Color { get; set; }
        public TileType Type { get; set; }

        public HexTile() : this(TileType.Default, 0) { }
        public HexTile(TileType type) : this(type, 0) { }
        public HexTile(TileType type, float height)
        {
            Type = type;

            if (type == TileType.Path)
            {
                Height = height + 0f;
                Color = Color.Black;
            }
            else if (type == TileType.Wall)
            {
                Height = height + 0.5f;
                Color = Color.Black;
            } else
            {
                Height = height + 1f;
                Color = Color.Black;
            }
        }
        public HexTile(float height, Color color, TileType type)
        {
            Height = height;
            Color = color;
            Type = type;
        }
    }
}

