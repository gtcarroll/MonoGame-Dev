//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using HexMap;
//using HexMap.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace HexMap
//{
//    public struct OldHexCoord
//    {
//        public int X;
//        public int Y;

//        public OldHexCoord(int x, int y) {
//            X = x;
//            Y = y;
//        }

//        public int Z()
//        {
//            return -X - Y;
//        }

//        public static OldHexCoord operator +(OldHexCoord a)
//            => a;
//        public static OldHexCoord operator -(OldHexCoord a)
//            => new OldHexCoord(-a.X, -a.Y);

//        public static OldHexCoord operator +(OldHexCoord a, OldHexCoord b)
//            => new OldHexCoord(a.X + b.X, a.Y + b.Y);
//        public static OldHexCoord operator -(OldHexCoord a, OldHexCoord b)
//            => a + (-b);

//        public static OldHexCoord operator *(OldHexCoord a, int b)
//            => new OldHexCoord(a.X * b, a.Y * b);
//        public static OldHexCoord operator *(int a, OldHexCoord b)
//           => new OldHexCoord(b.X * a, b.Y * a);
//    }

//    public class OldHexGrid
//    {
//        public bool Normalize = false;
//        public bool IsOddOffset { get { return _isOddOffset; } }

//        public Polygon2D Hexagon { get; }

//        public Vector2 BasisX { get { return _basisX; } }
//        public Vector2 BasisY { get { return _basisY; } }

//        public int Cols { get; }
//        public int Rows { get; }
//        public int MinCols { get; }
//        public int MinRows { get; }
//        public float Scale { get; }
//        public float Stretch { get; set; }

//        private HexTile[][] _tiles;

//        private Vector2 _basisX;
//        private Vector2 _basisY;

//        private SimplexNoise _simplexNoise;
//        private float _time;
//        private bool _isOddOffset;

//        public OldHexGrid() : this(11, 10, 10f) { }
//        public OldHexGrid(int cols, int rows) : this(cols, rows, 10f) { }
//        public OldHexGrid(int cols, int rows, float scale)
//        {
//            Hexagon = new Polygon2D(6, scale);
//            Hexagon.Transform(Matrix.CreateRotationZ(MathHelper.TwoPi / 12));

//            // must have an odd number of cols
//            if (cols % 2 == 0) cols++;
//            _isOddOffset = (cols / 2) % 2 == 0;

//            Cols = cols;
//            Rows = rows;
//            MinCols = -cols / 2;
//            MinRows = 0;
//            Scale = scale;
//            Stretch = 5;

//            Console.WriteLine(_isOddOffset);

//            _time = 0f;
//            _simplexNoise = new SimplexNoise();
//            _basisX = scale * new Vector2(3f / 2f, MathF.Sqrt(3) / 2f);
//            _basisY = scale * new Vector2(0, MathF.Sqrt(3));

//            BuildHexGrid(Cols, Rows);
//        }

//        public OldHexCoord Offset2Axial(OldHexCoord offsetCoords)
//        {
//            return Offset2Axial(offsetCoords.X, offsetCoords.Y);
//        }
//        public OldHexCoord Offset2Axial(int col, int row)
//        {
//            int offset = _isOddOffset ? -(col & 1) : col & 1;
//            int x = col;
//            int y = row - (col + offset) / 2;
//            return new OldHexCoord(x, y);
//        }

//        public OldHexCoord Axial2Offset(OldHexCoord axialCoords)
//        {
//            return Axial2Offset(axialCoords.X, axialCoords.Y);
//        }
//        public OldHexCoord Axial2Offset(int x, int y)
//        {
//            int offset = _isOddOffset ? -(x & 1) : x & 1;
//            int col = x;
//            int row = y + (x + offset) / 2;
//            return new OldHexCoord(col, row);
//        }

//        public OldHexCoord CenterCoords(OldHexCoord offsetCoords)
//        {
//            return CenterCoords((int)offsetCoords.X, (int)offsetCoords.Y);
//        }
//        public OldHexCoord CenterCoords(int col, int row)
//        {
//            int centeredCol = col - (Cols / 2);

//            return new OldHexCoord(centeredCol, row);
//        }

//        public OldHexCoord UncenterCoords(OldHexCoord offsetCoords)
//        {
//            return UncenterCoords(offsetCoords.X, offsetCoords.Y);
//        }
//        public OldHexCoord UncenterCoords(int col, int row)
//        {
//            int uncenteredCol = col + (Cols / 2);

//            return new OldHexCoord(uncenteredCol, row);
//        }

//        public Vector2 GetTranslation(OldHexCoord offsetCoords)
//        {
//            return GetTranslation(offsetCoords.X, offsetCoords.Y);
//        }
//        public Vector2 GetTranslation(int col, int row)
//        {
//            OldHexCoord axialCoords = Offset2Axial(CenterCoords(col, row));
//            //if (col < 10 && row < 10)
//            //{
//            //    Console.WriteLine("(" + col + ", " + row + ") -> " + "(" + axialCoords.X + ", " + axialCoords.Y + ")");

//            //}
//            return (axialCoords.X) * _basisX + (axialCoords.Y) * _basisY;
//        }

//        public Vector2 GetTranslationAxial(int q, int r)
//        {
//            return (q) * _basisX + (r) * _basisY;
//        }

//        public HexTile GetTile(OldHexCoord offsetCoords)
//        {
//            return GetTile(offsetCoords.X, offsetCoords.Y);
//        }
//        public HexTile GetTile(int col, int row)
//        {
//            return GetTileUncentered(UncenterCoords(col, row));
//        }

//        private HexTile GetTileUncentered(OldHexCoord offsetCoords)
//        {
//            return GetTileUncentered(offsetCoords.X, offsetCoords.Y);
//        }
//        private HexTile GetTileUncentered(int col, int row)
//        {
//#if DEBUG
//            CheckBounds(col, row);
//#endif
//            return _tiles[col][row];
//        }

//        public void SetColor(int col, int row, Color color)
//        {
//            OldHexCoord coords = UncenterCoords(col, row);
//            if (coords.Y >= 0)
//            {
//                _tiles[coords.X][coords.Y].Color = color;
//            }
//        }

//        public void SetHeight(int col, int row, float height)
//        {
//            OldHexCoord coords = new OldHexCoord(col, row);
//            SetHeight(coords, height);
//        }
//        public void SetHeight(OldHexCoord coords, float height)
//        {
//            coords = UncenterCoords(coords);
//            if (coords.Y >= 0)
//            {
//                _tiles[coords.X][coords.Y].Height = height;
//                _tiles[coords.X][coords.Y].IsStamped = true;
//            }
//        }
//        public void Flatten(int col, int row)
//        {
//            SetHeight(col, row, 0);
//        }

//        public void Update(GameTime gameTime)
//        {
//            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
//            for (int c = 0; c < Cols; c++)
//            {
//                for (int r = 0; r < Rows; r++)
//                {
//                    HexTile tile = _tiles[c][r];
//                    if (!tile.IsStamped)
//                    {
//                        tile.Height = GetNoise(c, r, _time);
//                        tile.Color = GetNoiseRGB(c, r, _time);
//                    }
//                }
//            }

//            SetColor(0, 0, Color.Yellow);
//            SetHeight(0, 0, 0);
//            SetColor(0, 1, Color.Blue);
//            SetHeight(0, 1, 0);
//            SetColor(1, 0, Color.Green);
//            SetHeight(1, 0, 0);
//            SetColor(-1, 0, Color.Green);
//            SetHeight(-1, 0, 0);
//        }

//        private void CheckBounds(int col, int row)
//        {
//            if (col < 0 || col > Cols)
//            {
//                throw new ArgumentOutOfRangeException("col");
//            }
//            else if (row < 0 || row > Rows)
//            {
//                throw new ArgumentOutOfRangeException("row");
//            }
//        }

//        private void BuildHexGrid(int cols, int rows)
//        {
//            _tiles = new HexTile[cols][];

//            for (int c = 0; c < cols; c++)
//            {
//                HexTile[] col = new HexTile[rows];
//                for (int r = 0; r < rows; r++)
//                {
//                    col[r] = new HexTile(GetNoise(c, r, _time));
//                    col[r].Color = GetNoiseRGB(c, r, _time);
//                }
//                _tiles[c] = col;
//            }
//        }

//        private float GetNoise(int col, int row, float time)
//        {
//            if (Stretch <= 0) Stretch = 1f;
//            double noiseX = (col - Cols / 2) / Stretch;
//            double noiseY = (row - Rows / 2) / Stretch;
//            double noiseZ = _time / 10f;
//            float noise = (float)_simplexNoise.Evaluate(noiseX, noiseY, noiseZ);

//            return Normalize ? NormalizeNoise(noise) : noise; ;
//        }

//        private Color GetNoiseRGB(int col, int row, float time)
//        {
//            if (Stretch <= 0) Stretch = 1f;
//            double noiseX = (col - Cols / 2) / Stretch;
//            double noiseY = (row - Rows / 2) / Stretch;
//            double noiseZ = _time;
//            float[] rgbs = new float[3];
//            for (int i = 0; i < 3; i++)
//            {
//                float noise = 0.5f * (float)_simplexNoise.Evaluate(noiseX, noiseY, noiseZ, (double)(i / Stretch));
//                rgbs[i] = Normalize ? NormalizeNoise(noise) : noise;
//            }
//            return new Color(rgbs[0], rgbs[1], rgbs[2]);
//        }

//        private float NormalizeNoise(float noise)
//        {
//            return (noise + 0.9f) / 1.8f;
//        }

//        public void Stamp(OldHexCoord offsetCoords, float height)
//        {
//            SetHeight(offsetCoords, height);
//            SetColor(offsetCoords.X, offsetCoords.Y, Color.White);
//        }

//        public void StampCircle(OldHexCoord center, int radius, float height)
//        {
//            OldHexCoord[] coords = GetCircle(radius, (center.X & 1) == 1);

//            for (int i = 0; i < coords.Length - 1; i++)
//            {
//                Stamp(center + coords[i], height);
//            }

//        }

//        public OldHexCoord[] GetCircle(int radius, bool isOddCenter)
//        {
//            int circleArea = IncrementingSum(radius) * 6 + 1;
//            OldHexCoord[] coords = new OldHexCoord[circleArea];
//            int coordsIndex = 0;

//            while (radius >= 0)
//            {
//                for (int x = -radius ; x <= radius; x++)
//                {
//                    if (Math.Abs(x) == radius)
//                    {
//                        for (int y = -(radius + 1) / 2; y <= radius / 2; y++)
//                        {
//                            coords[coordsIndex++] = new OldHexCoord(x, y);
//                        }
//                    }
//                    else
//                    {
//                        int oddOffset = isOddCenter ? 0 : (x & 1);
//                        coords[coordsIndex++] = new OldHexCoord(x, radius - oddOffset);
//                        coords[coordsIndex++] = new OldHexCoord(x, -radius);
//                    }
//                }

//                radius--;
//            }

//            return coords;
//        }

//        private int IncrementingSum(int n)
//        {
//            int sum = 0;
//            for (int i = 1; i <= n; i++)
//            {
//                sum += i;
//            }
//            return sum;
//        }
//    }
//}

