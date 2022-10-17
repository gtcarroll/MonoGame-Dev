using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexMap;
using HexMap.Graphics;

namespace HexMap.HexMap
{
    public class PerlinNoise
    {
        private Random _random;

        public PerlinNoise()
        {
            _random = new Random();
        }

        public float GetNoise1D(int x)
        {
            return (float)_random.NextDouble();
        }

        public float GetNoise2D(Vector2 v)
        {
            return GetNoise2D((int)v.X, (int)v.Y);
        }
        public float GetNoise2D(int x, int y)
        {
            return (float)_random.NextDouble();
        }

        public float GetNoise3D(Vector3 v)
        {
            return GetNoise3D((int)v.X, (int)v.Y, (int)v.Z);
        }
        public float GetNoise3D(Vector2 v, int t)
        {
            return GetNoise3D((int)v.X, (int)v.Y, t);
        }
        public float GetNoise3D(int x, int y, int z)
        {
            return (float)_random.NextDouble();
        }

        public float GetNoise4D(Vector3 v, int t)
        {
            return GetNoise4D((int)v.X, (int)v.Y, (int)v.Z, t);
        }
        public float GetNoise4D(int x, int y, int z, int t)
        {
            return (float)_random.NextDouble();
        }
    }
}

