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

        bool _debugCombatStart = true;

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
                                     new Point(1700, 932), 9, true, 0);
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

            if (_debugCombatStart)
            {
                _debugCombatStart = false;
                _handPlot.InitDeck(_drawPlot, 12);
            }

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                if (!_handPlot.DrawCard(_drawPlot))
                {
                    _handPlot.ShuffleInto(_discardPlot, _drawPlot);
                    _handPlot.DrawCard(_drawPlot);
                }
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.Z))
            {
                for (int i = 0; i < 10; i++) _handPlot.DrawCard(_drawPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.X))
            {
                for (int i = 0; i < 10; i++) _handPlot.RemoveCard(0, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.C))
            {
                _handPlot.ShuffleInto(_discardPlot, _drawPlot);
            }

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D0))
            {
                _handPlot.RemoveCard(0, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D1))
            {
                _handPlot.RemoveCard(1, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D2))
            {
                _handPlot.RemoveCard(2, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D3))
            {
                _handPlot.RemoveCard(3, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D4))
            {
                _handPlot.RemoveCard(4, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D5))
            {
                _handPlot.RemoveCard(5, _discardPlot);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.D6))
            {
                _handPlot.RemoveCard(0, _drawPlot);
            }
        }
    }
}

