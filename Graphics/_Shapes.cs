using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public sealed class Shapes : IDisposable
    {
        private bool _isDisposed;
        private Game _game;
        private BasicEffect _effect;

        private VertexPositionColor[] _vertices;
        private int[] _indices;

        private int shapeCount;
        private int vertexCount;
        private int indexCount;

        private bool isStarted;

        private Camera _camera;

        public Shapes(Game game)
        {
            this._isDisposed = false;
            this._game = game ?? throw new ArgumentNullException("game");

            this._effect = new BasicEffect(game.GraphicsDevice);
            this._effect.TextureEnabled = false;
            this._effect.FogEnabled = false;
            this._effect.LightingEnabled = false;
            this._effect.VertexColorEnabled = true;
            this._effect.World = Matrix.Identity;
            this._effect.View = Matrix.Identity;
            this._effect.Projection = Matrix.Identity;

            const int MaxVertexCount = 1024;
            const int MaxIndexCount = MaxVertexCount * 3;

            this._vertices = new VertexPositionColor[MaxVertexCount];
            this._indices = new int[MaxIndexCount];

            this.shapeCount = 0;
            this.vertexCount = 0;
            this.indexCount = 0;

            this.isStarted = false;

            _camera = null;
        }

        public void Dispose()
        {
            if (this._isDisposed)
            {
                return;
            }

            _effect?.Dispose();
            _isDisposed = true;
        }

        public void Begin(Camera camera)
        {
            if (isStarted)
            {
                throw new Exception("batching is already started");
            }

            Viewport vp = _game.GraphicsDevice.Viewport;

            if (camera is null)
            {
                _effect.View = Matrix.Identity;
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, 0, vp.Height, 0f, 1f);
            }
            else
            {
                camera.UpdateMatrices();
                _effect.View = camera.View;
                _effect.Projection = camera.Proj;
            }
            _camera = camera;

            isStarted = true;
        }

        public void End()
        {
            Flush();
            isStarted = false;
        }

        public void Flush()
        {
            if (shapeCount == 0)
            {
                return;
            }

            EnsureStarted();

            foreach(EffectPass pass in this._effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length,
                    _indices, 0, indexCount / 3);
            }

            shapeCount = 0;
            vertexCount = 0;
            indexCount = 0;
        }

        private void EnsureStarted()
        {
            if (!isStarted)
            {
                throw new Exception("batching was never started");
            }
        }

        private void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > _vertices.Length)
            {
                throw new Exception("maximum shape vertex count is " + _vertices.Length);
            }

            if (shapeIndexCount > _indices.Length)
            {
                throw new Exception("maximum shape index count is " + _indices.Length);
            }

            if (vertexCount + shapeVertexCount > _vertices.Length ||
                indexCount + shapeIndexCount > this._indices.Length)
            {
                Flush();
            }
        }

        public void DrawRectangleFill(float x, float y, float width, float height, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float left = x;
            float right = x + width;
            float bottom = y;
            float top = y + height;

            Vector2 a = new Vector2(left, top);
            Vector2 b = new Vector2(right, top);
            Vector2 c = new Vector2(right, bottom);
            Vector2 d = new Vector2(left, bottom);

            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 1;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 3;

            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), color);

            shapeCount++;
        }
        public void DrawRectangle(float x, float y, float width, float height, float thickness, Color color)
        {
            float left = x;
            float right = x + width;
            float top = y;
            float bottom = y + height;

            DrawLine(left, top, right, top, thickness, color);
            DrawLine(right, top, right, bottom, thickness, color);
            DrawLine(right, bottom, left, bottom, thickness, color);
            DrawLine(left, bottom, left, top, thickness, color);
        }

        public void DrawLineSlow(float ax, float ay, float bx, float by, float thickness, Color color)
        {
            DrawLineSlow(new Vector2(ax, ay), new Vector2(bx, by), thickness, color);
        }
        public void DrawLineSlow(Vector2 a, Vector2 b, float thickness, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            if (_camera != null)
            {
                thickness /= _camera.Zoom;
            }
            float halfThickness = thickness / 2f;

            Vector2 e1 = b - a;
            e1.Normalize();
            e1 *= halfThickness;
            Vector2 e2 = -e1;

            Vector2 n1 = new Vector2(-e1.Y, e1.X);
            Vector2 n2 = -n1;

            Vector2 q1 = a + n1 + e2;
            Vector2 q2 = b + n1 + e1;
            Vector2 q3 = b + n2 + e1;
            Vector2 q4 = a + n2 + e2;

            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 1;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 3;

            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q1, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q2, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q3, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q4, 0f), color);

            shapeCount++;
        }

        public void DrawLine(Vector2 a, Vector2 b, float thickness, Color color)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, thickness, color);
        }

        public void DrawLine(float ax, float ay, float bx, float by, float thickness, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            if (_camera != null)
            {
                thickness /= _camera.Zoom;
            }
            float halfThickness = thickness / 2f;

            float e1x = bx - ax;
            float e1y = by - ay;
            Normalize(ref e1x, ref e1y);
            e1x *= halfThickness;
            e1y *= halfThickness;

            float e2x = -e1x;
            float e2y = -e1y;

            float n1x = -e1y;
            float n1y = e1x;

            float n2x = -n1x;
            float n2y = -n1y;

            float q1x = ax + n1x + e2x;
            float q1y = ay + n1y + e2y;

            float q2x = bx + n1x + e1x;
            float q2y = by + n1y + e1y;

            float q3x = bx + n2x + e1x;
            float q3y = by + n2y + e1y;

            float q4x = ax + n2x + e2x;
            float q4y = ay + n2y + e2y;

            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 1;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 0;
            _indices[indexCount++] = vertexCount + 2;
            _indices[indexCount++] = vertexCount + 3;

            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q1x, q1y, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q2x, q2y, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q3x, q3y, 0f), color);
            _vertices[vertexCount++] = new VertexPositionColor(new Vector3(q4x, q4y, 0f), color);

            shapeCount++;
        }

        public void DrawCircleSlow(float x, float y, float radius, int points, float thickness, Color color)
        {
            const int minPoints = 3;
            const int maxPoints = 256;
            points = MathHelper.Clamp(points, minPoints, maxPoints);

            float deltaAngle = MathHelper.TwoPi / (float)points;
            float angle = 0f;

            for (int i = 0; i < points; i++)
            {
                float ax = MathF.Sin(angle) * radius + x;
                float ay = MathF.Cos(angle) * radius + y;

                angle += deltaAngle;

                float bx = MathF.Sin(angle) * radius + x;
                float by = MathF.Cos(angle) * radius + y;

                DrawLine(ax, ay, bx, by, thickness, color);
            }
        }
        public void DrawCircle(float x, float y, float radius, int points, float thickness, Color color)
        {
            const int minPoints = 3;
            const int maxPoints = 256;
            points = MathHelper.Clamp(points, minPoints, maxPoints);

            float rotation = MathHelper.TwoPi / (float)points;
            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);

            float ax = radius;
            float ay = 0f;

            for (int i = 0; i < points; i++)
            {
                float bx = cos * ax - sin * ay;
                float by = sin * ax + cos * ay;

                DrawLine(ax + x, ay + y, bx + x, by + y, thickness, color);

                ax = bx;
                ay = by;
            }
        }
        public void DrawCircleFill(float x, float y, float radius, int points, Color color)
        {
            EnsureStarted();

            const int minPoints = 3;
            const int maxPoints = 256;

            int shapeVertexCount = MathHelper.Clamp(points, minPoints, maxPoints); ;
            int shapeTriangleCount = shapeVertexCount - 2;
            int shapeIndexCount = shapeTriangleCount * 3;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            for (int i = 0; i < shapeTriangleCount; i++)
            {
                _indices[indexCount++] = 0 + vertexCount;
                _indices[indexCount++] = i + 1 + vertexCount;
                _indices[indexCount++] = i + 2 + vertexCount;
            }

            float rotation = MathHelper.TwoPi / (float)points;
            float sin = MathF.Sin(rotation);
            float cos = MathF.Cos(rotation);

            float ax = radius;
            float ay = 0f;

            for (int i = 0; i < shapeVertexCount; i++)
            {
                float bx = ax;
                float by = ay;

                _vertices[vertexCount++] = new VertexPositionColor(new Vector3(bx + x, by + y, 0f), color);

                ax = cos * bx - sin * by;
                ay = sin * bx + cos * by;
            }

            shapeCount++;
        }

        public void DrawPolygon(Vector2[] vertices, float thickness, Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[(i + 1) % vertices.Length];

                DrawLine(a, b, thickness, color);
            }
        }

        //public void DrawPolygon(Vector2[] vertices, Matrix transform, float thickness, Color color)
        //{
        //    for (int i = 0; i < vertices.Length; i++)
        //    {
        //        Vector2 a = vertices[i];
        //        Vector2 b = vertices[(i + 1) % vertices.Length];

        //        DrawLine(a, b, thickness, color);
        //    }
        //}

        public void DrawPolygon(Vector2[] vertices, Matrix transform, float thickness, Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[(i + 1) % vertices.Length];

                a = Vector2.Transform(a, transform);
                b = Vector2.Transform(b, transform);

                DrawLine(a, b, thickness, color);
            }
        }

        public void DrawPolygonFlat(Vector2[] vertices, FlatTransform transform, float thickness, Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[(i + 1) % vertices.Length];

                a = Util.Transform2D(a, transform);
                b = Util.Transform2D(b, transform);

                DrawLine(a, b, thickness, color);
            }
        }

        public void DrawPolygonFill(Vector2[] vertices, int[] triangleIndices, Matrix transform, Color color)
        {
#if DEBUG
            if (vertices is null)
            {
                throw new ArgumentNullException("vertices");
            }

            if (triangleIndices is null)
            {
                throw new ArgumentNullException("indices");
            }

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException("verticess");
            }

            if (triangleIndices.Length < 3)
            {
                throw new ArgumentOutOfRangeException("indices");
            }
#endif

            this.EnsureStarted();
            this.EnsureSpace(vertices.Length, triangleIndices.Length);

            for (int i = 0; i < triangleIndices.Length; i++)
            {
                _indices[indexCount++] = triangleIndices[i] + this.vertexCount;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertex = vertices[i];
                vertex = Vector2.Transform(vertex, transform);
                _vertices[vertexCount++] = new VertexPositionColor(new Vector3(vertex.X, vertex.Y, 0f), color);
            }

            this.shapeCount++;
        }

        public void DrawPolygonFill2(Vector2[] vertices, int[] indices, Matrix transform, Color color)
        {
#if DEBUG
            if (vertices is null)
            {
                throw new ArgumentNullException("vertices");
            }
            if (indices is null)
            {
                throw new ArgumentNullException("indices");
            }

            if (vertices.Length < 3)
            {
                throw new ArgumentOutOfRangeException("vertices");
            }
            if (indices.Length < 3)
            {
                throw new ArgumentOutOfRangeException("indices");
            }
#endif
            EnsureStarted();
            EnsureSpace(vertices.Length, indices.Length);

            for (int i = 0; i < indices.Length; i++)
            {
                _indices[indexCount++] = indices[i] + vertexCount;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertex = Vector2.Transform(vertices[i], transform);

                _vertices[vertexCount++] = new VertexPositionColor(new Vector3(vertex, 0f), color);
            }

            shapeCount++;
        }

        public void DrawPolygonTriangles(Vector2[] vertices, int[] triangles, Matrix transform, Color color)
        {
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int a = triangles[i];
                int b = triangles[i + 1];
                int c = triangles[i + 2];

                Vector2 va = vertices[a];
                Vector2 vb = vertices[b];
                Vector2 vc = vertices[c];

                va = Vector2.Transform(va, transform);
                vb = Vector2.Transform(vb, transform);
                vc = Vector2.Transform(vc, transform);

                DrawLine(va, vb, 1f, color);
                DrawLine(vb, vc, 1f, color);
                DrawLine(vc, va, 1f, color);
            }
        }

        public static void Normalize(ref float x, ref float y)
        {
            float invLen = 1f / MathF.Sqrt(x * x + y * y);
            x *= invLen;
            y *= invLen;
        }
    }
}

