using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class HandPlot : GUIPlot
    {
        private const int MaxHandSize = 10;

        public static Point Size = new Point(625, 175);

        private Point _downGap;
        private Point _upGap;

        public HandPlot(Point location) : base()
        {
            // initialize params
            // Direction
            // Alignment
            // Gap (change to array?)

            Direction = PlotDirection.Row;

            _downGap = new Point(55, 90);
            _upGap = new Point(55, -90);

            int width = 650;
            int height = 200;

            ScreenSpace = new Rectangle(location, new Point(width, height));

            // initialize graphics
            // ScreenSpace
            // Background


            // create and connect all nodes
            AddAllNodes();
            ConnectAllNodes();
        }

        private void AddAllNodes()
        {
            Point nodePosition = ScreenSpace.Location + _downGap;

            for (int i = 0; i < MaxHandSize; i++)
            {
                Nodes.Add(new CardNode(nodePosition));

                nodePosition += i % 2 == 0 ? _upGap : _downGap;
            }
        }

        private void ConnectAllNodes()
        {
            // add neighbors of leftmost node
            Nodes[0].SetNeighbor(InputDirection.Right, Nodes[1]);
            Nodes[0].SetNeighbor(InputDirection.Up, Nodes[1]);

            // add neighbors of middle nodes
            for (int i = 1; i < Nodes.Count - 1; i++)
            {
                Nodes[i].SetNeighbor(InputDirection.Left, Nodes[i - 1]);
                Nodes[i].SetNeighbor(InputDirection.Right, Nodes[i + 1]);

                InputDirection nextDir = i % 2 == 0 ? InputDirection.Up
                                                    : InputDirection.Down;

                Nodes[i].SetNeighbor(nextDir, Nodes[i + 1]);
            }

            // add neighbors of rightmost node
            Nodes[Nodes.Count - 1].SetNeighbor(InputDirection.Left,
                                               Nodes[Nodes.Count - 2]);
            //Nodes[Nodes.Count - 1].SetNeighbor(InputDirection.Down,
            //                                   Nodes[Nodes.Count - 2]);
        }

        // implement functions for adding/removing cards,
        // placing them relative to the plot,
        // and managing neighbors


    }
}

