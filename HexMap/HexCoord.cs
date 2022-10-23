using System;
namespace HexMap
{
    public struct HexCoord
    {
        public int Q;
        public int R;
        public int S { get { return -Q - R; } }

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }

        // negate
        public static HexCoord operator -(HexCoord a)
            => new HexCoord(-a.Q, -a.R);

        // add
        public static HexCoord operator +(HexCoord a, HexCoord b)
            => new HexCoord(a.Q + b.Q, a.R + b.R);

        // subtract
        public static HexCoord operator -(HexCoord a, HexCoord b)
            => new HexCoord(a.Q - b.Q, a.R - b.R);

        // multiply
        public static HexCoord operator *(HexCoord a, int b)
            => new HexCoord(a.Q * b, a.R * b);
        public static HexCoord operator *(int a, HexCoord b)
           => new HexCoord(b.Q * a, b.R * a);

        // divide
        public static HexCoord operator /(HexCoord a, int b)
            => new HexCoord(a.Q / b, a.R / b);
        public static HexCoord operator /(int a, HexCoord b)
           => new HexCoord(b.Q / a, b.R / a);

        public override string ToString()
        {
            return "(" + Q + ", " + R + ")";
        }
    }
}

