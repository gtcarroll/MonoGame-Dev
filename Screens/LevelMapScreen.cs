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
        private Game _game;
        private Model _prism;
        private Random _random;
        private SplineCamera _camera;
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

            if (keyboard.WasKeyJustUp(Keys.Space))
            {
                _level = new LevelMap(_random, 100);
                _camera.LoadPoints(_level.CameraPositions);
            }

            // pan camera w UP/DOWN/LEFT/RIGHT
            if (keyboard.IsKeyDown(Keys.Up))
            {
                _camera.MoveForward(gameTime);
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                _camera.MoveBackward(gameTime);
            }

            _camera.Update();
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

