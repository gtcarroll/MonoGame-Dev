using System;
using System.Collections.Generic;
using System.Xml.Linq;
using EverythingUnder.Combat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Reflection.Metadata.BlobBuilder;

namespace EverythingUnder.GUI
{
    public enum PlotDirection
    {
        Row,
        Column
    }

    public enum PlotAlignment
    {
        Start,
        End,
        Center
    }

    public abstract class GUIPlot
    {
        #region Properties

        public GameManager Game;
        public AnimationQueue AnimationQueue;

        // connected plots
        public Dictionary<InputDirection, GUIPlot> Neighbors;

        // plot parameters
        //public PlotAlignment Alignment;
        public PlotDirection Direction;
        //public int Gap;

        // plot state
        public List<GUINode> Nodes;
        public bool IsActive;

        // plot graphics
        public Rectangle ScreenSpace;
        public Texture2D Background;

        #endregion

        #region Constructors

        public GUIPlot(GameManager game, AnimationQueue animationQueue)
        {
            Game = game;
            AnimationQueue = animationQueue;

            Neighbors = new Dictionary<InputDirection, GUIPlot>();

            Nodes = new List<GUINode>();
            IsActive = false;
        }

        #endregion

        #region Loading Methods

        public virtual void LoadContent(GameManager game)
        {
            foreach (GUINode node in Nodes)
            {
                node.LoadContent(game);
            }
        }

        public virtual void UnloadContent(GameManager game)
        {
            foreach (GUINode node in Nodes)
            {
                node.UnloadContent(game);
            }
        }

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime time)
        {
            foreach (GUINode node in Nodes)
            {
                node.Update(time);
            }
        }

        public virtual void DrawBG(SpriteBatch spriteBatch)
        {
            if (Background != null)
            {
                spriteBatch.Draw(Background, ScreenSpace, Color.White);
            }
        }

        public virtual void DrawFG(SpriteBatch spriteBatch)
        {
            foreach (GUINode node in Nodes)
            {
                node.Draw(spriteBatch);
            }
        }

        #endregion

        #region State Methods

        public bool Contains(Point mousePos)
        {
            return ScreenSpace.Contains(mousePos);
        }

        public void SetNeighbor(InputDirection direction, GUIPlot plot)
        {
            if (Neighbors.ContainsKey(direction))
            {
                Neighbors[direction] = plot;
            }
            else
            {
                Neighbors.Add(direction, plot);
            }
        }

        public void RemoveNeighbor(InputDirection direction)
        {
            Neighbors.Remove(direction);
        }

        public virtual GUINode GetNearestNode(Point point)
        {
            if (Nodes.Count == 0) return null;

            GUINode nearestNode = Nodes[0];
            int minDistance = GetDistanceTo(Nodes[0], point);

            for (int i = 1; i < Nodes.Count; i++)
            {
                if (Nodes[i].IsActive)
                {
                    int distance = GetDistanceTo(Nodes[i], point);

                    if (distance >= minDistance)
                    {
                        // we are moving away from the nearest node and can return
                        return nearestNode;
                    }

                    // we are moving toward the nearest node and can continue
                    nearestNode = Nodes[i];
                    minDistance = distance;
                }
            }

            return nearestNode.IsActive ? nearestNode : null;
        }

        #endregion

        #region Helper Methods

        private int GetDistanceTo(GUINode node, Point point)
        {
            if (Direction == PlotDirection.Row)
            {
                return Math.Abs(point.X - node.Center.X);
            }
            else // Direction == PlotDirection.Column
            {
                return Math.Abs(point.Y - node.Center.Y);
            }
        }

        #endregion
    }
}

