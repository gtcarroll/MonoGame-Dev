using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexMap;
using HexMap.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexMap.HexMap
{
    public struct HexCoord
    {
        public int X;
        public int Y;

        public HexCoord(int x, int y) {
            X = x;
            Y = y;
        }
    }

    public class HexGrid
    {
        public bool Normalize = false;
        public bool IsOddOffset { get { return _isOddOffset; } }

        public Polygon2D Hexagon { get; }

        public Vector2 BasisX { get { return _basisX; } }
        public Vector2 BasisY { get { return _basisY; } }

        public int Cols { get; }
        public int Rows { get; }
        public int MinCols { get; }
        public int MinRows { get; }
        public float Scale { get; }
        public float Stretch { get; set; }

        private HexTile[][] _tiles;

        private Vector2 _basisX;
        private Vector2 _basisY;

        private SimplexNoise _simplexNoise;
        private float _time;
        private bool _isOddOffset;

        public HexGrid() : this(11, 10, 10f) { }
        public HexGrid(int cols, int rows) : this(cols, rows, 10f) { }
        public HexGrid(int cols, int rows, float scale)
        {
            Hexagon = new Polygon2D(6, scale);
            Hexagon.Transform(Matrix.CreateRotationZ(MathHelper.TwoPi / 12));

            // must have an odd number of cols
            if (cols % 2 == 0) cols++;
            _isOddOffset = (cols / 2) % 2 == 0;

            Cols = cols;
            Rows = rows;
            MinCols = -cols / 2;
            MinRows = 0;
            Scale = scale;
            Stretch = 5;

            Console.WriteLine(_isOddOffset);

            _time = 0f;
            _simplexNoise = new SimplexNoise();
            _basisX = scale * new Vector2(3f / 2f, MathF.Sqrt(3) / 2f);
            _basisY = scale * new Vector2(0, MathF.Sqrt(3));

            BuildHexGrid(Cols, Rows);
        }

        public HexCoord Offset2Axial(HexCoord offsetCoords)
        {
            return Offset2Axial(offsetCoords.X, offsetCoords.Y);
        }
        public HexCoord Offset2Axial(int col, int row)
        {
            int offset = _isOddOffset ? -(col & 1) : col & 1;
            int x = col;
            int y = row - (col + offset) / 2;
            return new HexCoord(x, y);
        }

        public HexCoord Axial2Offset(HexCoord axialCoords)
        {
            return Axial2Offset(axialCoords.X, axialCoords.Y);
        }
        public HexCoord Axial2Offset(int x, int y)
        {
            int offset = _isOddOffset ? -(x & 1) : x & 1;
            int col = x;
            int row = y + (x + offset) / 2;
            return new HexCoord(col, row);
        }

        public HexCoord CenterCoords(HexCoord offsetCoords)
        {
            return CenterCoords((int)offsetCoords.X, (int)offsetCoords.Y);
        }
        public HexCoord CenterCoords(int col, int row)
        {
            int centeredCol = col - (Cols / 2);

            return new HexCoord(centeredCol, row);
        }

        public HexCoord UncenterCoords(HexCoord offsetCoords)
        {
            return UncenterCoords(offsetCoords.X, offsetCoords.Y);
        }
        public HexCoord UncenterCoords(int col, int row)
        {
            int uncenteredCol = col + (Cols / 2);

            return new HexCoord(uncenteredCol, row);
        }

        public Vector2 GetTranslation(HexCoord offsetCoords)
        {
            return GetTranslation(offsetCoords.X, offsetCoords.Y);
        }
        public Vector2 GetTranslation(int col, int row)
        {
            HexCoord axialCoords = Offset2Axial(CenterCoords(col, row));
            return (axialCoords.X) * _basisX + (axialCoords.Y) * _basisY;
        }

        public HexTile GetTile(HexCoord offsetCoords)
        {
            return GetTile(offsetCoords.X, offsetCoords.Y);
        }
        public HexTile GetTile(int col, int row)
        {
            return GetTileUncentered(UncenterCoords(col, row));
        }

        private HexTile GetTileUncentered(HexCoord offsetCoords)
        {
            return GetTileUncentered(offsetCoords.X, offsetCoords.Y);
        }
        private HexTile GetTileUncentered(int col, int row)
        {
#if DEBUG
            CheckBounds(col, row);
#endif
            return _tiles[col][row];
        }

        public void SetColor(int col, int row, Color color)
        {
            HexCoord coords = UncenterCoords(col, row);
            _tiles[coords.X][coords.Y].Color = color;
        }

        public void SetHeight(int col, int row, float height)
        {
            HexCoord coords = UncenterCoords(col, row);
            _tiles[coords.X][coords.Y].Height = height;
        }
        public void Flatten(int col, int row)
        {
            SetHeight(col, row, 0);
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

            SetColor(0, 0, Color.Yellow);
            SetHeight(0, 0, -1);
            SetColor(0, 1, Color.Blue);
            SetHeight(0, 1, -1);
            SetColor(1, 0, Color.Green);
            SetHeight(1, 0, -1);
            SetColor(-1, 0, Color.Green);
            SetHeight(-1, 0, -1);
        }

        private void CheckBounds(int col, int row)
        {
            if (col < 0 || col > Cols)
            {
                throw new ArgumentOutOfRangeException("col");
            }
            else if (row < 0 || row > Rows)
            {
                throw new ArgumentOutOfRangeException("row");
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

