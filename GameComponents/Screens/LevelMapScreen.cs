using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EverythingUnder.Graphics;
using EverythingUnder.Levels;
using EverythingUnder.ScreenManagement;

namespace EverythingUnder.Screens
{
    public class LevelMapScreen : GameScreen
    {
        private readonly GameManager _game;
        private readonly Random _random;
        private readonly SplineCamera _camera;

        private Model _prism;
        private Model _sphere;
        private LevelMap _level;

        private float _fogDepth = 12f;

        private List<HexCoord> _highlighted;

        public LevelMapScreen(GameManager game) : base()
        {
            _game = game;

            _random = game.Random;
            _level = new LevelMap(game);
            _camera = new SplineCamera(game, _level.CameraPositions);

            _highlighted = new List<HexCoord>();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _prism = _game.Content.Load<Model>("hexagonal-prism");
            _sphere = _game.Content.Load<Model>("sphere");
        }

        public override void Update(GameTime gameTime, bool isFocused,
                                                       bool isCovered)
        {
            _highlighted = new List<HexCoord>();
            Viewport viewport = _game.GraphicsDevice.Viewport;

            //InputState inputState = 

            //MouseState mouse = Mouse.GetState();
            //Vector2 mouseLocation = new Vector2(mouse.X, mouse.Y);

            //HexCoord? left = _level.NextCoords[0];
            //HexCoord? right = _level.NextCoords[1];

            //Vector3 topDelta = new Vector3(0, 0, 1);

            //if (_camera.IsReturned)
            //{
            //    if (left != null)
            //    {
            //        Vector3 pos = _level.GetWorldPosition((HexCoord)left);

            //        if (ModelHelper.IntersectsHex(mouseLocation, pos, _camera.View, _camera.Projection, viewport))
            //        {
            //            _highlighted.Add((HexCoord)left);
            //            //if (mouse.WasButtonJustUp(MouseButton.Left))
            //            //{
            //            //    MoveLeft();
            //            //}
            //        }
            //    }
            //    if (right != null)
            //    {
            //        Vector3 pos = _level.GetWorldPosition((HexCoord)right);

            //        if (ModelHelper.IntersectsHex(mouseLocation, pos, _camera.View, _camera.Projection, viewport))
            //        {
            //            _highlighted.Add((HexCoord)right);
            //            //if (mouse.WasButtonJustUp(MouseButton.Left))
            //            //{
            //            //    MoveRight();
            //            //}
            //        }
            //    }
            //}

            //KeyboardStateExtended keyboard = KeyboardExtended.GetState();

            //// re-generate level
            //if (keyboard.WasKeyJustUp(Keys.Space))
            //{
            //    _level = new LevelMap(_game);
            //    _camera.LoadPoints(_level.CameraPositions);

            //    _camera.T = -4f;
            //    _camera.Return(1f);
            //}

            //// progress left or right
            //HexCoord?[] nextCoords = _level.NextCoords;
            //if (keyboard.WasKeyJustUp(Keys.A))
            //{
            //    MoveLeft();
            //}
            //if (keyboard.WasKeyJustUp(Keys.D))
            //{
            //    MoveRight();
            //}

            //// move camera forward or backward
            //if (keyboard.IsKeyDown(Keys.W))
            //{
            //    _camera.MoveForward(gameTime);
            //}
            //if (keyboard.IsKeyDown(Keys.S))
            //{
            //    _camera.MoveBackward(gameTime);
            //}

            //// return camera to home position
            //if (keyboard.WasKeyJustUp(Keys.Q))
            //{
            //    _camera.Return();
            //}

            //// return camera to home position
            //if (keyboard.WasKeyJustUp(Keys.Up))
            //{
            //    _fogDepth++;
            //    Console.WriteLine(_fogDepth);
            //}
            //if (keyboard.WasKeyJustUp(Keys.Down))
            //{
            //    _fogDepth--;
            //    Console.WriteLine(_fogDepth);
            //}

            _camera.Update(gameTime);
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
                ModelHelper.DrawModel(_prism, translation, _camera.View, _camera.Projection, _fogDepth, _highlighted.Contains(coord));
            }
        }

        private void MoveLeft()
        {
            MoveToNextNode(0);
        }
        private void MoveRight()
        {
            MoveToNextNode(1);
        }
        private bool MoveToNextNode(int index)
        {
            if (_camera.IsReturned)
            {
                // get next HexCoord
                HexCoord? next = _level.NextCoords[index];

                // update camera positions
                if (next == null || _level.MoveToNode((HexCoord)next) == null)
                {
                    return false;
                }

                // load new spline points
                _camera.LoadPoints(_level.CameraPositions);

                // animate camera moving forward to next Y value
                _camera.AnimateTo(_level.GetCameraPosition((HexCoord)next).Y);

                return true;
            }
            else if (!_camera.IsAnimating)
            {
                _camera.Return();
            }

            return false;
        }
    }
}

