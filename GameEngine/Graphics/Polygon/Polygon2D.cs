using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class Polygon2D
    {
        public Vector2[] Vertices { get; }
        public int[] Triangles { get; }

        private Vector2[] _originalVertices;

        public Polygon2D(int sides) : this(sides, 1f) { }
        public Polygon2D(int sides, float radius) : this(CalculateVertices(sides, radius)) { }
        public Polygon2D(Vector2[] vertices) : this(vertices, CalculateTriangles(vertices)) { }
        private Polygon2D(Vector2[] vertices, int[] triangles)
        {
            _originalVertices = vertices;
            Vertices = new Vector2[_originalVertices.Length];
            _originalVertices.CopyTo(Vertices, 0);
            Triangles = triangles;
        }

        public void Transform(Matrix transform)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vector2.Transform(_originalVertices[i], transform);
            }
        }

        public static Vector2[] CalculateVertices(int sides, float radius)
        {
            Vector2[] vertices = new Vector2[sides];

            // define rotation values
            float rotation = MathHelper.TwoPi / (float)sides;
            float sin = MathF.Sin(-rotation);
            float cos = MathF.Cos(-rotation);

            // add first vertex
            float prevX = 0f;
            float prevY = radius;
            vertices[0] = new Vector2(prevX, prevY);

            // calculate and add remaining vertices
            for (int i = 1; i < sides; i++)
            {
                float x = cos * prevX - sin * prevY;
                float y = sin * prevX + cos * prevY;
                vertices[i] = new Vector2(x, y);

                prevX = x;
                prevY = y;
            }

            return vertices;
        }
        public static int[] CalculateTriangles(Vector2[] vertices, bool isConvex = false)
        {
            // build index list
            List<int> indexList = new List<int>();
            for (int i = 0; i < vertices.Length; i++)
            {
                indexList.Add(i);
            }

            // build triangles list
            int triangleIndex = 0;
            int[] triangles = new int[(vertices.Length - 2) * 3];
            while (indexList.Count > 3)
            {
                // test each remaining vertex for valid triangle
                for (int i = 1; i <= indexList.Count; i++)
                {
                    int a = CyclicIndex(indexList, i - 1);
                    int b = indexList[i];
                    int c = CyclicIndex(indexList, i + 1);

                    Vector2 vA = vertices[a];
                    Vector2 vB = vertices[b];
                    Vector2 vC = vertices[c];

                    // if test triangle is a valid ear (or shape is convex)
                    if (isConvex || IsTriangleAnEar(vertices, i, vA, vB, vC))
                    {
                        // add triangle indices to triangle list
                        triangles[triangleIndex++] = a;
                        triangles[triangleIndex++] = b;
                        triangles[triangleIndex++] = c;

                        // remove test vertex from index list
                        indexList.RemoveAt(i);
                        break;
                    }
                }
            }

            // add remaining triangle indices to triangle list
            triangles[triangleIndex++] = indexList[0];
            triangles[triangleIndex++] = indexList[1];
            triangles[triangleIndex++] = indexList[2];

            return triangles;
        }

        private static T CyclicIndex<T>(List<T> list, int i)
        {
            i = (i % list.Count);
            if (i < 0) i += list.Count;
            return list[i];
        }
        private static T CyclicIndex<T>(T[] array, int i)
        {
            i = (i % array.Length);
            if (i < 0) i += array.Length;
            return array[i];
        }

        private static float CrossProduct2D(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        private static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            float crossA = CrossProduct2D(b - a, point - a);
            float crossB = CrossProduct2D(c - b, point - b);
            float crossC = CrossProduct2D(a - c, point - c);

            return !(crossA < 0f || crossB < 0f || crossC < 0f);
        }
        private static bool IsTriangleAnEar(Vector2[] vertices, int testIndex, Vector2 a, Vector2 b, Vector2 c)
        {
            // concave triangles can't be an ear
            if (CrossProduct2D(a - b, c - b) <= 0f)
            {
                return false;
            }

            // test if triangle contains any other points in vertex list
            for (int j = 0; j < vertices.Length - 3; j++)
            {
                Vector2 testPoint = CyclicIndex(vertices, j + testIndex + 2);
                if (IsPointInTriangle(testPoint, a, b, c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

