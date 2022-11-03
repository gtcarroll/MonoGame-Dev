using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public class PolygonBatch
    {
        private readonly int _MaxVertexCount = 1024;
        private readonly float _DefaultLineWidth = 1f;
        private readonly Color _DefaultColor = Color.White;

        private readonly Game _game;
        private Camera _camera;
        private BasicEffect _effect;

        private VertexPositionColor[] _vertices;
        private int[] _indices;

        private int _shapeCount;
        private int _vertexCount;
        private int _indexCount;

        private bool _isDisposed;
        private bool _isStarted;

        public PolygonBatch(Game game)
        {
            _game = game ?? throw new ArgumentNullException("game");
            BuildEffect(game);

            _vertices = new VertexPositionColor[_MaxVertexCount];
            _indices = new int[_MaxVertexCount * 3];

            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;

            _isStarted = false;
            _isDisposed = false;

            _camera = null;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _effect?.Dispose();
            _isDisposed = true;
        }

        public void Begin(Camera camera)
        {
            if (_isStarted)
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
                _effect.Projection = camera.Projection;
            }
            _camera = camera;

            _isStarted = true;
        }
        public void End()
        {
            Flush();
            _isStarted = false;
        }
        public void Flush()
        {
            if (_shapeCount == 0)
            {
                return;
            }

            EnsureStarted();

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length,
                    _indices, 0, _indexCount / 3);
            }

            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;
        }

        public void DrawLine(Vector2 a, Vector2 b, float lineWidth, Color color)
        {
            DrawLine(a.X, a.Y, b.X, b.Y, lineWidth, color);
        }
        public void DrawLine(float ax, float ay, float bx, float by, float lineWidth, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;

            EnsureSpace(shapeVertexCount, shapeIndexCount);

            //if (_camera != null)
            //{
            //    lineWidth /= _camera.Zoom;
            //}
            float halfThickness = lineWidth / 2f;

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

            _indices[_indexCount++] = _vertexCount + 0;
            _indices[_indexCount++] = _vertexCount + 1;
            _indices[_indexCount++] = _vertexCount + 2;
            _indices[_indexCount++] = _vertexCount + 0;
            _indices[_indexCount++] = _vertexCount + 2;
            _indices[_indexCount++] = _vertexCount + 3;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1x, q1y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2x, q2y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3x, q3y, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4x, q4y, 0f), color);

            _shapeCount++;
        }

        public void DrawPolygon(Polygon2D polygon)
        {
            DrawPolygon(polygon.Vertices, Matrix.Identity, _DefaultColor, _DefaultLineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, Matrix transform)
        {
            DrawPolygon(polygon.Vertices, transform, _DefaultColor, _DefaultLineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, Color color)
        {
            DrawPolygon(polygon.Vertices, Matrix.Identity, color, _DefaultLineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, float lineWidth)
        {
            DrawPolygon(polygon.Vertices, Matrix.Identity, _DefaultColor, lineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, Matrix transform, Color color)
        {
            DrawPolygon(polygon.Vertices, transform, color, _DefaultLineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, Matrix transform, float lineWidth)
        {
            DrawPolygon(polygon.Vertices, transform, _DefaultColor, lineWidth);
        }
        public void DrawPolygon(Polygon2D polygon, Color color, float lineWidth)
        {
            DrawPolygon(polygon.Vertices, Matrix.Identity, color, lineWidth);
        }
        public void DrawPolygon(Vector2[] vertices, Matrix transform, Color color, float lineWidth)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 a = vertices[i];
                Vector2 b = vertices[(i + 1) % vertices.Length];

                a = Vector2.Transform(a, (Matrix)transform);
                b = Vector2.Transform(b, (Matrix)transform);

                DrawLine(a, b, lineWidth, color);
            }
        }

        public void DrawPolygonFill(Polygon2D polygon)
        {
            DrawPolygonFill(polygon.Vertices, polygon.Triangles, Matrix.Identity, _DefaultColor);
        }
        public void DrawPolygonFill(Polygon2D polygon, Matrix transform)
        {
            DrawPolygonFill(polygon.Vertices, polygon.Triangles, transform, _DefaultColor);
        }
        public void DrawPolygonFill(Polygon2D polygon, Color color)
        {
            DrawPolygonFill(polygon.Vertices, polygon.Triangles, Matrix.Identity, color);
        }
        public void DrawPolygonFill(Polygon2D polygon, Matrix transform, Color color)
        {
            DrawPolygonFill(polygon.Vertices, polygon.Triangles, transform, color);
        }
        public void DrawPolygonFill(Vector2[] vertices, int[] indices, Matrix transform, Color color)
        {
            EnsureStarted();
            EnsureSpace(vertices.Length, indices.Length);

            for (int i = 0; i < indices.Length; i++)
            {
                _indices[_indexCount++] = indices[i] + _vertexCount;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertex = Vector2.Transform(vertices[i], transform);

                _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(vertex, 0f), color);
            }

            _shapeCount++;
        }

        public void DrawPolygonTriangles(Polygon2D polygon)
        {
            DrawPolygonTriangles(polygon.Vertices, polygon.Triangles, Matrix.Identity, _DefaultColor);
        }
        public void DrawPolygonTriangles(Polygon2D polygon, Matrix transform)
        {
            DrawPolygonTriangles(polygon.Vertices, polygon.Triangles, transform, _DefaultColor);
        }
        public void DrawPolygonTriangles(Polygon2D polygon, Color color)
        {
            DrawPolygonTriangles(polygon.Vertices, polygon.Triangles, Matrix.Identity, color);
        }
        public void DrawPolygonTriangles(Polygon2D polygon, Matrix transform, Color color)
        {
            DrawPolygonTriangles(polygon.Vertices, polygon.Triangles, transform, color);
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

                DrawLine(va, vb, 2f, color);
                DrawLine(vb, vc, 2f, color);
                DrawLine(vc, va, 2f, color);
            }
        }

        private void BuildEffect(Game game)
        {
            _effect = new BasicEffect(game.GraphicsDevice);
            _effect.TextureEnabled = false;
            _effect.FogEnabled = false;
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.Identity;
            _effect.Projection = Matrix.Identity;
        }

        private void EnsureStarted()
        {
            if (!_isStarted)
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

            if (_vertexCount + shapeVertexCount > _vertices.Length ||
                _indexCount + shapeIndexCount > this._indices.Length)
            {
                Flush();
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

