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

        public static Point Size = new Point(770, 200);

        private Point _downGap;
        private Point _upGap;

        public List<CardSprite> Cards;

        public HandPlot(GameManager game, Point location) : base(game)
        {
            // initialize HandPlot params
            _downGap = new Point(55, 90);
            _upGap = new Point(55, -90);

            Cards = new List<CardSprite>();

            // initialize GUIPlot params
            Direction = PlotDirection.Row;
            ScreenSpace = new Rectangle(location, Size);

            // create and connect all nodes
            AddAllNodes();
            ConnectAllNodes();
        }

        private void AddAllNodes()
        {
            Point nodePosition = ScreenSpace.Location + _downGap;

            for (int i = 0; i < MaxHandSize + 3; i++)
            {
                Nodes.Add(new CardNode(nodePosition));

                if (i >= 2 && i < 12)
                {
                    Nodes[i].RemoveSprite();
                }

                nodePosition += i % 2 == 0 ? _upGap : _downGap;
            }
        }

        public override void Update(GameTime time)
        {


            base.Update(time);
        }

        public bool AddCard()
        {
            if (Cards.Count >= MaxHandSize) return false;

            int i = 2 + Cards.Count;

            CardSprite cardSprite = new CardSprite(Nodes[i].Center);
            Cards.Add(cardSprite);

            Nodes[i].AddSprite(cardSprite);
            Nodes[i].LoadContent(Game);

            return true;
        }

        public bool RemoveCard(int i)
        {
            if (Cards.Count <= i) return false;

            Cards.RemoveAt(i);
            Nodes[2 + i].RemoveSprite();

            UpdateCardPositions();

            return true;
        }

        private void UpdateCardPositions()
        {
            Point nodePosition = GetNodePosition(2);

            for (int i = 0; i < Cards.Count; i++)
            {
                CardSprite cardSprite = Cards[i];

                if (cardSprite.Center != nodePosition)
                {
                    cardSprite.Center = nodePosition;//CurrentState.GetCopyAt(nodePosition));
                    Nodes[3 + i].RemoveSprite();
                    Nodes[2 + i].AddSprite(cardSprite);
                }

                nodePosition += i % 2 == 0 ? _upGap : _downGap;
            }
        }

        private Point GetNodePosition(int index)
        {
            Point nodePosition = ScreenSpace.Location + _downGap;

            for (int i = 0; i < index; i++)
            {
                nodePosition += i % 2 == 0 ? _upGap : _downGap;
            }

            return nodePosition;
        }

        //private void UpdateCardSprites()
        //{
        //    Point nodePosition = ScreenSpace.Location + _downGap;

        //    for (int i = 0; i < MaxHandSize; i++)
        //    {
        //        Nodes.Add(new CardNode(nodePosition));

        //        nodePosition += i % 2 == 0 ? _upGap : _downGap;
        //    }
        //}

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

