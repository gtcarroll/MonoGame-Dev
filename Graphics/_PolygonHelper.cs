using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public enum WindingOrder
    {
        Invalid,
        Clockwise,
        CounterClockwise
    }

    public static class PolygonHelper
    {
        public static bool Triangulate(Vector2[] vertices, out int[] triangles, out string errorMessage)
        {
            triangles = null;
            errorMessage = string.Empty;

            // validate vertex list
            if (vertices is null)
            {
                errorMessage = "The vertex list is null";
                return false;
            }
            if (vertices.Length < 3)
            {
                errorMessage = "The vertex list must have at least 3 vertices";
                return false;
            }

            // validate polygon
            //if (!IsSimplePolygon(vertices))
            //{
            //    errorMessage = "The vertex list does not define a simle polygon";
            //    return false;
            //}
            //if (ContainsColinearEdges(vertices))
            //{

            //    errorMessage = "The vertex list contains colinear edges";
            //    return false;
            //}

            //// validate winding order
            //ComputePolygonArea(vertices, out float area, out WindingOrder windingOrder);
            //if (windingOrder is WindingOrder.Invalid)
            //{
            //    errorMessage = "The vertex list does not contain a valid polygon";
            //    return false;
            //}
            //else if (windingOrder is WindingOrder.CounterClockwise)
            //{
            //    Array.Reverse(vertices);
            //}

            // triangulate polygon
            List<int> indexList = new List<int>();
            for (int i = 0; i < vertices.Length; i++)
            {
                indexList.Add(i);
            }

            int totalTriangleCount = vertices.Length - 2;

            triangles = new int[totalTriangleCount * 3];
            int triangleIndex = 0;

            while (indexList.Count > 3)
            {
                for (int i = 0; i < indexList.Count; i++)
                {
                    int a = indexList[i];
                    int b = Util.GetItem(indexList, i - 1);
                    int c = Util.GetItem(indexList, i + 1);

                    Vector2 va = vertices[a];
                    Vector2 vb = vertices[b];
                    Vector2 vc = vertices[c];

                    Vector2 va2vb = vb - va;
                    Vector2 va2vc = vc - va;

                    // is test triangle convex?
                    if (Util.CrossProduct2D(va2vb, va2vc) < 0f)
                    {
                        continue;
                    }

                    // is test triangle an ear (no contained vertices)
                    bool isEar = true;
                    for (int j = 0; j < vertices.Length; j++)
                    {
                        if (j == a || j == b || j == c)
                        {
                            continue;
                        }
                        Vector2 nextVertex = vertices[j];
                        //Vector2 nextVertex = Util.GetItem(vertices, j + i + 2);

                        if (IsPointInTriangle(nextVertex, vb, va, vc))
                        {
                            isEar = false;
                            break;
                        }
                    }

                    if (isEar)
                    {
                        triangles[triangleIndex++] = b;
                        triangles[triangleIndex++] = a;
                        triangles[triangleIndex++] = c;

                        indexList.RemoveAt(i);
                        break;
                    }
                }
            }

            triangles[triangleIndex++] = indexList[0];
            triangles[triangleIndex++] = indexList[1];
            triangles[triangleIndex++] = indexList[2];

            return true;
        }

        public static bool IsSimplePolygon(Vector2[] vertices)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsColinearEdges(Vector2[] vertices)
        {
            throw new NotImplementedException();
        }

        public static void ComputePolygonArea(Vector2[] vertices, out float area, out WindingOrder windingOrder)
        {
            throw new NotImplementedException();
        }

        public static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;
            Vector2 ca = a - c;

            Vector2 ap = point - a;
            Vector2 bp = point - b;
            Vector2 cp = point - c;

            float crossA = Util.CrossProduct2D(ab, ap);
            float crossB = Util.CrossProduct2D(bc, bp);
            float crossC = Util.CrossProduct2D(ca, cp);

            return !(crossA > 0f || crossB > 0f || crossC > 0f);
        }
    }
}

