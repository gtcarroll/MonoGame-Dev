using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Graphics
{
    public static class ModelHelper
    {
        public static Ray CalculateRay(Vector2 mouseLocation, Viewport viewport,
            Matrix view, Matrix projection)
        {
            Vector3 nearPoint = viewport.Unproject(
                new Vector3(mouseLocation.X, mouseLocation.Y, 0.0f),
                projection,
                view,
                Matrix.Identity);

            Vector3 farPoint = viewport.Unproject(
                new Vector3(mouseLocation.X, mouseLocation.Y, 1.0f),
                projection,
                view,
                Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public static float? IntersectDistance(BoundingSphere sphere, Vector2 mouseLocation,
                    Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = CalculateRay(mouseLocation, viewport, view, projection);
            return mouseRay.Intersects(sphere);
        }

        public static bool IntersectsHex(Vector2 mouseLocation, Vector3 pos,
                    Matrix view, Matrix projection, Viewport viewport)
        {
            pos += new Vector3(0, 0, 1f);
            BoundingSphere sphere = new BoundingSphere(pos, 0.7f);
            float? distance = IntersectDistance(sphere, mouseLocation, view, projection, viewport);

            if (distance != null)
            {
                return true;
            }
            return false;
        }

        public static void DrawModel(Model model, Matrix world,
            Matrix view, Matrix projection, bool selected = false)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = Color.Black.ToVector3(); // For best results, make this color whatever your background is.
                    effect.FogStart = 0f;
                    effect.FogEnd = 15f;

                    effect.EnableDefaultLighting();
                    effect.EmissiveColor = selected ? Color.Yellow.ToVector3() : Color.Black.ToVector3();

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}

