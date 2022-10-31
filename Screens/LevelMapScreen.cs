using System;
using System.Collections.Generic;
using EverythingUnder.Graphics;
using EverythingUnder.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace EverythingUnder.Screens
{
    public class LevelMapScreen : GameScreen
    {
        private readonly Game _game;
        private readonly Random _random;
        private readonly SplineCamera _camera;

        private Model _prism;
        private LevelMap _level;

        public LevelMapScreen(Game game) : base(game)
        {
            _game = game;

            _random = new Random();
            _level = new LevelMap(_random);
            _camera = new SplineCamera(game, _level.CameraPositions);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _prism = _game.Content.Load<Model>("hexagonal-prism");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardStateExtended keyboard = KeyboardExtended.GetState();

            // re-generate level
            if (keyboard.WasKeyJustUp(Keys.Space))
            {
                _level = new LevelMap(_random, 100);
                _camera.LoadPoints(_level.CameraPositions);
            }

            // progress left or right
            HexCoord?[] nextCoords = _level.GetNextCoords();
            if (keyboard.WasKeyJustUp(Keys.A))
            {
                MoveLeft();
            }
            if (keyboard.WasKeyJustUp(Keys.D))
            {
                MoveRight();
            }

            // move camera forward or backward
            if (keyboard.IsKeyDown(Keys.W))
            {
                _camera.MoveForward(gameTime);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                _camera.MoveBackward(gameTime);
            }

            // return camera to home position
            if (keyboard.WasKeyJustUp(Keys.Q))
            {
                _camera.Return();
            }

            _camera.Update(gameTime);
        }

        private void MoveLeft()
        {
            if (_camera.IsReturned)
            {
                MoveToNextNode(true);
            }
            else
            {
                _camera.Return();
            }
        }
        private void MoveRight()
        {
            if (_camera.IsReturned)
            {
                MoveToNextNode(false);
            }
            else
            {
                _camera.Return();
            }
        }

        private bool MoveToNextNode(bool isLeft)
        {
            // get next HexCoord
            int nextIndex = isLeft ? 0 : 1;
            HexCoord? next = _level.GetNextCoords()[nextIndex];

            // update camera positions
            if (next == null
                || _camera.IsAnimating
                || _level.MoveToNode((HexCoord)next) == null)
            {
                return false;
            }

            // load new spline points
            _camera.LoadPoints(_level.CameraPositions);

            // animate camera moving forward to next Y value
            _camera.AnimateTo(_level.GetCameraPosition((HexCoord)next).Y);

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.Black);
            _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (KeyValuePair<HexCoord, LevelNode> entry in _level.Nodes)
            {
                HexCoord coord = entry.Key;
                LevelNode node = entry.Value;

                Matrix translation = Matrix.CreateScale(0.7f) * Matrix.CreateTranslation(_level.GetWorldPosition(coord));
                DrawModel(_prism, translation, _camera.View, _camera.Projection);
            }
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = Color.Black.ToVector3(); // For best results, make this color whatever your background is.
                    effect.FogStart = 0f;
                    effect.FogEnd = 20f;

                    effect.EnableDefaultLighting();
                    effect.EmissiveColor = Color.Black.ToVector3();

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}

