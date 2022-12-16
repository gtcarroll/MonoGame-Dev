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

        #endregion

        #region Constructors

        public GUIGarden(GameManager game)
        {
            Game = game;
            Plots = new List<GUIPlot>();

            Cursor = new HighlightCursor(game);
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

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime time)
        {
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
            foreach (GUIPlot plot in Plots)
            {
                Point gamePos = Game.ScreenManager.GetGamePosition(mousePos);
                if (plot.Contains(gamePos))
                {
                    //Console.WriteLine(mousePos);
                    //Console.WriteLine(Game.ScreenManager.GetGamePosition(mousePos));
                    foreach (GUINode node in plot.Nodes)
                    {
                        if (node.Contains(gamePos))
                        {
                            //MoveToPlot(plot);
                            MoveToNode(node);
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper Methods

        private void TryToMoveTo(InputDirection direction)
        {
            if (CurrNode.Neighbors.ContainsKey(direction))
            {
                MoveToNode(CurrNode.GetNeighbor(direction));
            }
            else if (CurrPlot.Neighbors.ContainsKey(direction))
            {
                MoveToPlot(CurrPlot.Neighbors[direction]);
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

