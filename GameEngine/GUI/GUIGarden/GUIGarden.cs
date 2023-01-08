using System;
using System.Collections.Generic;

using EverythingUnder.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public abstract class GUIGarden
    {
        #region Properties

        private Vector2 _prevMousePos;
        //private bool _mouseChangedLastFrame;

        // game reference
        public GameManager Game;
        public SpriteBatch SpriteBatch;

        // garden state
        public List<GUIPlot> Plots;
        public GUIPlot CurrPlot;
        public GUINode CurrNode;

        private GUINode PrevNode;

        // garden graphics
        public HighlightCursor Cursor;
        public List<SpriteGroup> ActiveSprites;
        public List<SpriteGroup> TempSprites;

        public AnimationQueue AnimationQueue;

        #endregion

        #region Constructors

        public GUIGarden(GameManager game)
        {
            Game = game;
            AnimationQueue = new AnimationQueue();

            Plots = new List<GUIPlot>();

            Cursor = new HighlightCursor(game);

            ActiveSprites = new List<SpriteGroup>();
            TempSprites = new List<SpriteGroup>();
        }

        #endregion

        #region Loading Methods

        public virtual void LoadContent()
        {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            foreach (GUIPlot plot in Plots)
            {
                plot.LoadContent(Game);
            }
        }

        public virtual void UnloadContent()
        {
            foreach (GUIPlot plot in Plots)
            {
                plot.UnloadContent(Game);
            }
        }

        //public virtual void AddSprite(SpriteGroup sprite, bool isTemp = false)
        //{
        //    if (isTemp) TempSprites.Add(sprite);
        //    else ActiveSprites.Add(sprite);
        //}

        //public virtual void RemoveSprite(SpriteGroup sprite)
        //{
        //    if (ActiveSprites.Contains(sprite))
        //    {
        //        if (AnimationQueue.Contains(sprite))
        //        {

        //        }
        //    }
        //}

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime time)
        {
            AnimationQueue.Update(time);

            foreach (GUIPlot plot in Plots)
            {
                plot.Update(time);
            }

            Cursor.Update(time);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //SpriteBatch.Begin();

            DrawBG(spriteBatch);

            DrawFG(spriteBatch);

            //SpriteBatch.End();
        }

        public virtual void DrawBG(SpriteBatch spriteBatch)
        {
            foreach (GUIPlot plot in Plots)
            {
                plot.DrawBG(spriteBatch);
            }
        }

        public virtual void DrawFG(SpriteBatch spriteBatch)
        {
            foreach (GUIPlot plot in Plots)
            {
                plot.DrawFG(spriteBatch);
            }

            Cursor.Draw(spriteBatch);

            if (PrevNode != null && Cursor.IsAnimating)
            {
                PrevNode.Draw(spriteBatch);
            }

            CurrNode.Draw(spriteBatch);
        }

        #endregion

        #region Update Methods

        public virtual void HandleInput(InputState input)
        {
            if (input.WasSelectPressed())
            {
                CurrNode.OnSelected?.Invoke();
            }
            else if (input.WasCancelPressed())
            {

            }
            else if (input.WasUpPressed())
            {
                TryToMoveTo(InputDirection.Up);
            }
            else if (input.WasRightPressed())
            {
                TryToMoveTo(InputDirection.Right);
            }
            else if (input.WasDownPressed())
            {
                TryToMoveTo(InputDirection.Down);
            }
            else if (input.WasLeftPressed())
            {
                TryToMoveTo(InputDirection.Left);
            }

            // handle mouse
            Vector2 mousePos = input.GetMousePosition();
            if (mousePos == _prevMousePos) { return; }

            _prevMousePos = mousePos;
            foreach (GUIPlot plot in Plots)
            {
                Point gamePos = Game.ScreenManager.GetGamePosition(mousePos);
                if (plot.Contains(gamePos))
                {
                    foreach (GUINode node in plot.Nodes)
                    {
                        if (node.Contains(gamePos))
                        {
                            //_mouseChangedLastFrame = true;
                            //MoveToPlot(plot);
                            MoveToNode(node);
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        private void TryToMoveTo(InputDirection direction)
        {
            if (CurrNode.Neighbors.ContainsKey(direction)
                && CurrNode.Neighbors[direction].IsActive)
            {
                MoveToNode(CurrNode.GetNeighbor(direction));
            }
            else
            {
                GUIPlot targetPlot = CurrPlot;
                GUINode targetNode;
                while (targetPlot.Neighbors.ContainsKey(direction))
                {
                    targetPlot = targetPlot.Neighbors[direction];
                    targetNode = targetPlot.GetNearestNode(CurrNode.Center);

                    if (targetNode != null)
                    {
                        MoveToPlot(targetPlot);
                        return;
                    }
                }
            }
        }

        private void MoveToPlot(GUIPlot plot)
        {
            if (plot == null) plot = CurrPlot;

            CurrPlot.IsActive = false;

            CurrPlot = plot;

            CurrPlot.IsActive = true;

            MoveToNode(CurrPlot.GetNearestNode(CurrNode.Center));
        }

        private void MoveToNode(GUINode node)
        {
            if (node != CurrNode)
            {
                if (node == null) node = CurrNode;

                CurrNode.IsHovered = false;

                PrevNode = CurrNode;
                CurrNode = node;

                CurrNode.IsHovered = true;

                Cursor.AnimateTo(CurrNode.Sprite);
            }
        }

        #endregion
    }
}

