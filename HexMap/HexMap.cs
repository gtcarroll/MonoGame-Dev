using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexMap;
using HexMap.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexMap
{
    public class HexMap
    {
        public Polygon2D Hexagon;

        public Dictionary<HexCoord, HexTile> Tiles { get { return _tiles; } }

        private Vector2 _basisQ;
        private Vector2 _basisR;

        private Dictionary<HexCoord, HexTile> _tiles;

        public HexMap()
        {
            Hexagon = new Polygon2D(6);
            Hexagon.Transform(Matrix.CreateRotationZ(MathHelper.TwoPi / 12));

            _tiles = new Dictionary<HexCoord, HexTile>();

            _basisQ = new Vector2(3f / 2f, MathF.Sqrt(3) / 2f);
            _basisR = new Vector2(0, MathF.Sqrt(3));

            SetTiles(HexShapes.EventNode());
            SetTiles(HexShapes.Path(HexPath.Up));
        }
        public HexMap(int cols, int rows) : base()
        {
            SetTiles(new Dictionary<HexCoord, HexTile>());
        }

        // returns the HexTile at the given HexCoord (if it exists)
        public HexTile GetTile(HexCoord coord)
        {
            return _tiles[coord];
        }

        // returns the position vector of the given HexCoord
        public Vector2 GetTranslation(HexCoord coord)
        {
            Vector2 result = coord.Q * _basisQ + coord.R * _basisR;
            Console.WriteLine(result.X + ", " + result.Y);
            return coord.Q * _basisQ + coord.R * _basisR;
        }

        // stores the given HexTile at the given HexCoord
        public void SetTile(HexCoord coord, HexTile tile)
        {
            if (_tiles.ContainsKey(coord))
            {
                // don't overwrite paths
                if (_tiles[coord].Type != TileType.Path)
                {
                    _tiles[coord] = tile;
                }
            }
            else
            {
                _tiles.Add(coord, tile);
            }
        }
        // stores the given HexCoord, HexTile pairs in the HexMap
        public void SetTiles(IReadOnlyDictionary<HexCoord, HexTile> tiles)
        {
            foreach (KeyValuePair<HexCoord, HexTile> entry in tiles)
            {
                HexCoord coord = entry.Key;
                HexTile tile = entry.Value;

                SetTile(coord, tile);
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        
    }
}

