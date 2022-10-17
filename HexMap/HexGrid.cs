using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexMap;
using HexMap.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexMap.HexMap
{
    public class HexGrid
    {
        public bool Normalize = false;

        public Polygon2D Hexagon { get; }

        public Vector2 BasisX { get { return _basisX; } }
        public Vector2 BasisY { get { return _basisY; } }

        public int Cols { get; }
        public int Rows { get; }
        public float Scale { get; }
        public float Stretch { get; set; }

        private HexTile[][] _tiles;

        private Vector2 _basisX;
        private Vector2 _basisY;

        private SimplexNoise _simplexNoise;
        private float _time;

        public HexGrid() : this(10, 10, 10f) { }
        public HexGrid(int cols, int rows) : this(cols, rows, 10f) { }
        public HexGrid(int cols, int rows, float scale)
        {
            Hexagon = new Polygon2D(6, scale);
            Hexagon.Transform(Matrix.CreateRotationZ(MathHelper.TwoPi / 12));

            Cols = cols;
            Rows = rows;
            Scale = scale;
            Stretch = 5;

            _time = 0f;
            _simplexNoise = new SimplexNoise();
            _basisX = scale * new Vector2(3f / 2f, MathF.Sqrt(3) / 2f);
            _basisY = scale * new Vector2(0, MathF.Sqrt(3));

            BuildHexGrid(Cols, Rows);
        }

        public Vector2 Offset2Axial(Vector2 offsetCoords)
        {
            return Offset2Axial((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public Vector2 Offset2Axial(int col, int row)
        {
            int x = col;
            int y = row - (col - (col & 1)) / 2;
            return new Vector2(x, y);
        }

        public Vector2 Axial2Offset(Vector2 axialCoords)
        {
            return Axial2Offset((int)axialCoords.X, (int)axialCoords.Y);
        }
        public Vector2 Axial2Offset(int x, int y)
        {
            int col = x;
            int row = y + (x - (x & 1)) / 2;
            return new Vector2(col, row);
        }

        public Vector2 CenterCoords(Vector2 offsetCoords)
        {
            return CenterCoords((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public Vector2 CenterCoords(int col, int row)
        {
            int centeredCol = col - (Cols / 2);

            return new Vector2(centeredCol, row);
        }

        public Vector2 UncenterCoords(Vector2 offsetCoords)
        {
            return UncenterCoords((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public Vector2 UncenterCoords(int col, int row)
        {
            int uncenteredCol = col + (Cols / 2);

            return new Vector2(uncenteredCol, row);
        }

        public Vector2 GetTranslation(Vector2 offsetCoords)
        {
            return GetTranslation((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public Vector2 GetTranslation(int col, int row)
        {
            Vector2 axialCoords = Offset2Axial(CenterCoords(col, row));
            return (axialCoords.X) * _basisX + (axialCoords.Y) * _basisY;
        }

        public HexTile GetTile(Vector2 offsetCoords)
        {
            return GetTile((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public HexTile GetTile(int col, int row)
        {
            return _tiles[col][row];
        }

        public HexTile GetTileCentered(Vector2 offsetCoords)
        {
            return GetTile(UncenterCoords(offsetCoords));
        }
        public HexTile GetTileCentered(int col, int row)
        {
            return GetTile(UncenterCoords(col, row));
        }

        public void Update(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {
                    _tiles[c][r].Height = GetNoise(c, r, _time);
                    _tiles[c][r].Color = GetNoiseRGB(c, r, _time);
                }
            }
        }

        private void BuildHexGrid(int cols, int rows)
        {
            _tiles = new HexTile[cols][];

            for (int c = 0; c < cols; c++)
            {
                HexTile[] col = new HexTile[rows];
                for (int r = 0; r < rows; r++)
                {
                    col[r] = new HexTile(GetNoise(c, r, _time));
                    col[r].Color = GetNoiseRGB(c, r, _time);
                }
                _tiles[c] = col;
            }
        }

        private float GetNoise(int col, int row, float time)
        {
            if (Stretch <= 0) Stretch = 1f;
            double noiseX = (col - Cols / 2) / Stretch;
            double noiseY = (row - Rows / 2) / Stretch;
            double noiseZ = _time;
            float noise = (float)_simplexNoise.Evaluate(noiseX, noiseY, noiseZ);

            return Normalize ? NormalizeNoise(noise) : noise; ;
        }

        private Color GetNoiseRGB(int col, int row, float time)
        {
            if (Stretch <= 0) Stretch = 1f;
            double noiseX = (col - Cols / 2) / Stretch;
            double noiseY = (row - Rows / 2) / Stretch;
            double noiseZ = _time;
            float[] rgbs = new float[3];
            for (int i = 0; i < 3; i++)
            {
                float noise = (float)_simplexNoise.Evaluate(noiseX, noiseY, noiseZ, (double)(i / Stretch));
                rgbs[i] = Normalize ? NormalizeNoise(noise) : noise;
            }
            return new Color(rgbs[0], rgbs[1], rgbs[2]);
        }

        private float NormalizeNoise(float noise)
        {
            return (noise + 0.9f) / 1.8f;
        }
    }
}

