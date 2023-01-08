using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;
using EverythingUnder.ScreenManagement;

namespace EverythingUnder.GUI
{
    public class CombatGarden : GUIGarden
    {
        HandPlot _handPlot;

        DeckPlot _drawPlot;

        DeckPlot _discardPlot;

        public CombatGarden(GameManager game) : base(game)
        {
            // initialize state
            AddAllPlots();
            ConnectAllPlots();
            // CurrPlot
            CurrPlot = Plots[0];
            // CurrNode
            CurrNode = CurrPlot.Nodes[0];

            // initialize cursor
            // HighlightCursor

            // initialize data for card frames?

            // create and connect all plots
            //LoadContent();
        }

        private void AddAllPlots()
        {
            _discardPlot = new DeckPlot(Game, AnimationQueue,
                                        new Point(963, 824), 27, false, 0);
            Plots.Add(_discardPlot);

            _drawPlot = new DeckPlot(Game, AnimationQueue,
                                     new Point(1700, 932), 9, true, 12);
            Plots.Add(_drawPlot);

            _handPlot = new HandPlot(Game, AnimationQueue,
                                     new Point(1920, 1080) - HandPlot.Size);
            Plots.Add(_handPlot);

            Console.WriteLine(Plots);
        }

        private void ConnectAllPlots()
        {
            // connect discard with hand
            _discardPlot.Neighbors.Add(InputDirection.Right, _handPlot);
            _discardPlot.Neighbors.Add(InputDirection.Down, _handPlot);
            _handPlot.Neighbors.Add(InputDirection.Left, _discardPlot);

            // connect hand with draw
            _handPlot.Neighbors.Add(InputDirection.Right, _drawPlot);
            _drawPlot.Neighbors.Add(InputDirection.Left, _handPlot);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                _handPlot.DrawCard(_drawPlot);
            }

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad0))
            {
                _handPlot.RemoveCard(0, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                _handPlot.RemoveCard(1, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                _handPlot.RemoveCard(2, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                _handPlot.RemoveCard(3, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
            {
                _handPlot.RemoveCard(4, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad5))
            {
                _handPlot.RemoveCard(5, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6))
            {
                _handPlot.RemoveCard(0, _drawPlot);
            }
        }
    }
}

