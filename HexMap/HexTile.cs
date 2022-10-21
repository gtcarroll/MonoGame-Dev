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

        public HexTile() : this(TileType.Default) { }
        public HexTile(TileType type)
        {
            Type = type;

            if (type == TileType.Path)
            {
                Height = 0f;
                Color = Color.DarkSeaGreen;
            }
            else if (type == TileType.Wall)
            {
                Height = 0.5f;
                Color = Color.Brown;
            } else
            {
                Height = 1f;
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

