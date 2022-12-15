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

        public CombatGarden(GameManager game) : base(game)
        {
            // initialize state
            AddAllPlots();
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
            _handPlot = new HandPlot(Game, new Point(1920, 1080) - HandPlot.Size);
            Plots.Add(_handPlot);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                _handPlot.AddCard();
            }

            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad0))
            {
                _handPlot.RemoveCard(0);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad1))
            {
                _handPlot.RemoveCard(1);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad2))
            {
                _handPlot.RemoveCard(2);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad3))
            {
                _handPlot.RemoveCard(3);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad4))
            {
                _handPlot.RemoveCard(4);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad5))
            {
                _handPlot.RemoveCard(5);
            }
            if (input.WasPressed(Microsoft.Xna.Framework.Input.Keys.NumPad6))
            {
                _handPlot.RemoveCard(0);
                _handPlot.RemoveCard(0);
            }
        }
    }
}

