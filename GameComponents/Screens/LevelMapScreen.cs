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
        #region Properties

        private readonly GameManager _game;
        private readonly Random _random;
        private readonly SplineCamera _camera;

        private Model _prism;
        private Model _sphere;
        private LevelMap _level;

        private float _fogDepth = 12f;

        private HexCoord? _highlighted;
        private HexCoord?[] _nextCoords;
        private bool _wasHoveredLastFrame;

        #endregion

        #region Constructors

        public LevelMapScreen(GameManager game) : base()
        {
            _game = game;

            _random = game.Random;
            _level = new LevelMap(game);
            _camera = new SplineCamera(game, _level.CameraPositions);

            _wasHoveredLastFrame = false;
        }

        #endregion

        #region Loading Methods

        public override void LoadContent()
        {
            base.LoadContent();

            _prism = _game.Content.Load<Model>("Models/hexagonal-prism");
            _sphere = _game.Content.Load<Model>("Models/sphere");
        }

        #endregion

        #region Rendering Methods

        public override void HandleInput(GameTime time, InputState input)
        {
            // clear _highlighted if using mouse input
            if (_wasHoveredLastFrame) _highlighted = null;

            // highlight selected hex if it exists
            _nextCoords = _level.NextCoords;
            if (_camera.IsReturned && !_camera.IsAnimating)
            {
                Viewport viewport = _game.GraphicsDevice.Viewport;
                Vector2 mousePos = input.GetMousePosition();

                HexCoord? left = _nextCoords[0];
                HexCoord? right = _nextCoords[1];

                bool isLeftHovered = IsHexHovered(left, mousePos, viewport);
                bool isRightHovered = IsHexHovered(right, mousePos, viewport);

                if (input.WasLeftPressed() || isLeftHovered)
                {
                    _highlighted = left;
                }
                else if (input.WasRightPressed() || isRightHovered)
                {
                    _highlighted = right;
                }

                if ((isLeftHovered || isRightHovered)
                    && input.WasPressed(MouseButtons.Left))
                {
                    MoveToHighlighted();
                }

                _wasHoveredLastFrame = (isLeftHovered || isRightHovered);
            }

            // progress to highlighted hex
            if (input.WasSelectPressed())
            {
                MoveToHighlighted();
            }

            // deselect highlighted hex
            if (input.WasCancelPressed())
            {
                _highlighted = null;
            }

            // move camera forward or backward
            if (input.IsUpPressed())
            {
                _camera.MoveForward(time);
            }
            if (input.IsDownPressed())
            {
                _camera.MoveBackward(time);
            }

            // re-generate level
            if (input.WasPausePressed())
            {
                _level = new LevelMap(_game);
                _camera.LoadPoints(_level.CameraPositions);

                _camera.T = -4f;
                _camera.Return(1f);
            }
        }

        public override void Update(GameTime gameTime, bool isFocused,
                                                       bool isCovered)
        {
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
                ModelHelper.DrawModel(_prism, translation, _camera.View, _camera.Projection, _fogDepth, IsHexHighlighted(coord));
            }
        }

        #endregion

        #region Helper Methods

        private bool IsHexHovered(HexCoord? target, Vector2 mousePos, Viewport viewport)
        {
            if (target == null) return false;

            Vector3 hexPos = _level.GetWorldPosition((HexCoord)target);

            return ModelHelper.IntersectsHex(mousePos, hexPos, _camera.View,
                                            _camera.Projection, viewport);
        }

        private bool IsHexHighlighted(HexCoord? coord)
        {
            if (coord == null) return false;
            return IsHexHighlighted((HexCoord)coord);
        }
        private bool IsHexHighlighted(HexCoord coord)
        {
            if (_highlighted == null) return false;
            else return ((HexCoord)_highlighted).Equals(coord);
        }

        private void MoveToHighlighted()
        {
            if (IsHexHighlighted(_nextCoords[0]))
            {
                MoveToNextNode(0);
            }
            else if (IsHexHighlighted(_nextCoords[1]))
            {
                MoveToNextNode(1);
            } else
            {
                _camera.Return();
            }
        }

        private bool MoveToNextNode(int index)
        {
            if (_camera.IsReturned)
            {
                // get next HexCoord
                HexCoord? next = _nextCoords[index];

                // update camera positions
                if (next == null || _level.MoveToNode((HexCoord)next) == null)
                {
                    return false;
                }

                // clear _highlighted hex because it has been selected
                _highlighted = null;

                // load new spline points
                _camera.LoadPoints(_level.CameraPositions);

                // animate camera moving forward to next Y value
                _camera.AnimateTo(_level.GetCameraPosition((HexCoord)next).Y);

                OpenNodeScreen(_level.Nodes[(HexCoord)next]);

                return true;
            }
            else if (!_camera.IsAnimating)
            {
                _camera.Return();
            }

            return false;
        }

        private void OpenNodeScreen(LevelNode node)
        {
            _game.ScreenManager.AddScreen(new CombatScreen(_game));
        }

        #endregion
    }
}

