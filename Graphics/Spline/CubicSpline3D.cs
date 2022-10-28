// Wrapper class for CubicSpline that provides Vector3 positions as output
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public class CubicSpline3D
    {
        // CubicSpline being wrapped
        private CubicSpline spline;

        // Variables used for calculating Z value
        private float z0;
        private float dz;

        private float y0;
        private float dy;

        public CubicSpline3D(Vector3[] points, float startSlope = float.NaN, float endSlope = float.NaN, bool debug = false)//float[] x, float[] y, float z0, float dz, float startSlope = float.NaN, float endSlope = float.NaN, bool debug = false)
        {
            this.z0 = points[0].Z;
            this.dz = points[points.Length - 1].Z - z0;

            this.y0 = points[0].Y;
            this.dy = points[points.Length - 1].Y - y0;

            this.spline = new CubicSpline(GetYs(points), GetXs(points), startSlope, endSlope, debug);
        }

        private float[] GetYs(Vector3[] points)
        {
            float[] ys = new float[points.Length];

            for (int i = 0; i < ys.Length; i++)
            {
                ys[i] = points[i].Y;
            }

            return ys;
        }
        private float[] GetXs(Vector3[] points)
        {
            float[] xs = new float[points.Length];

            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = points[i].X;
            }

            return xs;
        }

        public Vector3 Eval3D(float y, bool debug = false)
        {
            return Eval3D(new float[] { y }, debug)[0];
        }

        /// <summary>
        /// Evaluate the spline at the specified y coordinates.
        /// This can extrapolate off the ends of the splines.
        /// You must provide Y's in ascending order.
        /// The spline must already be computed before calling this, meaning you must have already called Fit() or FitAndEval().
        /// </summary>
        /// <param name="ys">Input. Y coordinates to evaluate the fitted curve at.</param>
        /// <param name="debug">Turn on console output. Default is false.</param>
        /// <returns>The computed Vector3 position for each y.</returns>
        public Vector3[] Eval3D(float[] ys, bool debug = false)
        {
            Vector3[] positions = new Vector3[ys.Length];
            float[] xs = spline.Eval(ys, debug);

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3(xs[i], ys[i], EvalZ(ys[i]));
            }

            return positions;
        }

        /// <summary>
        /// Evaluate the z value for a specified y value.
        /// </summary>
        /// <param name="y">The y value.</param>
        /// <returns>The z value.</returns>
        private float EvalZ(float y)
        {
            float percentLength = (y - y0) / dy;
            return z0 + (percentLength * dz);
        }
    }
}
