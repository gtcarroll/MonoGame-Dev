using System;

namespace EverythingUnder.Levels
{
    public struct HexCoord
    {
        #region Properties

        public int Q;
        public int R;
        public int S { get { return -Q - R; } }

        #endregion

        #region Constructors

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }

        #endregion

        #region Operator Overloads

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

        #endregion

        #region ToString

        public override string ToString()
        {
            return "(" + Q + ", " + R + ")";
        }

        #endregion
    }
}

