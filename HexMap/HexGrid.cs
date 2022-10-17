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
        public double MaxNoise = 0;
        public double MinNoise = 0;
        public bool Normalize = false;

        public Polygon2D Hexagon { get; }

        public Vector2 BasisX { get { return _basisX; } }
        public Vector2 BasisY { get { return _basisY; } }
        //public Vector2 BasisZ { get { return _basisZ; } }

        public int Rows { get; }
        public int Cols { get; }
        public float Scale { get; }
        public float Stretch { get; set; }

        private SimplexNoise _simplexNoise;
        private HexTile[][] _tiles;

        private Vector2 _basisX;
        private Vector2 _basisY;
        //private Vector2 _basisZ;

        private int _time;

        public HexGrid() : this(10, 10, 10f) { }
        public HexGrid(int rows, int cols) : this(rows, cols, 10f) { }
        public HexGrid(int rows, int cols, float scale)
        {
            Hexagon = new Polygon2D(6, scale);
            Hexagon.Transform(Matrix.CreateRotationZ(MathHelper.TwoPi / 12));

            _basisX = scale * new Vector2(3f / 2f, MathF.Sqrt(3) / 2f);
            _basisY = scale * new Vector2(0, MathF.Sqrt(3));

            Rows = rows;
            Cols = cols;
            Scale = scale;
            Stretch = 5;

            _time = 0;
            _simplexNoise = new SimplexNoise();

            BuildHexGrid(Rows, Cols);
            Console.WriteLine("MinNoise = " + MinNoise);
            Console.WriteLine("MaxNoise = " + MaxNoise);
        }

        public Vector2 GetTranslation(int row, int col)
        {
            return (row - Rows / 2) * _basisX + (col - Cols / 2) * _basisY;
        }
        public HexTile GetTile(int row, int col)
        {
            return _tiles[row][col];
        }

        private void BuildHexGrid(int rows, int cols)
        {
            _tiles = new HexTile[rows][];

            for (int r = 0; r < rows; r++)
            {
                HexTile[] row = new HexTile[cols];
                for (int c = 0; c < cols; c++)
                {
                    row[c] = new HexTile(GetNoise(r, c, _time));
                    row[c].Color = GetNoiseRGB(r, c, _time);
                }
                _tiles[r] = row;
            }
        }
        public void Update()
        {
            _time++;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    _tiles[r][c].Noise = GetNoise(r, c, _time);
                    _tiles[r][c].Color = GetNoiseRGB(r, c, _time);
                }
            }
        }

        private float GetNoise(int row, int col, int time)
        {
            if (Stretch <= 0) Stretch = 1f;
            double noiseX = (col - Cols / 2) / Stretch;
            double noiseY = (row - Rows / 2) / Stretch;
            double noiseZ = _time / 20f;//(Stretch * 2);
            float noise = (float)_simplexNoise.Evaluate(noiseX, noiseY, noiseZ);

            if (noise > MaxNoise)
            {
                MaxNoise = noise;
            }
            else if (noise < MinNoise)
            {
                MinNoise = noise;
            }

            return Normalize ? NormalizeNoise(noise) : noise; ;
        }

        private Color GetNoiseRGB(int row, int col, int time)
        {
            if (Stretch <= 0) Stretch = 1f;
            double noiseX = (col - Cols / 2) / Stretch;
            double noiseY = (row - Rows / 2) / Stretch;
            double noiseZ = _time / 20f;//(Stretch * 2);
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

