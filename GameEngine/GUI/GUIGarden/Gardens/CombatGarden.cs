using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class CombatGarden : GUIGarden
    {
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
            Plots.Add(new HandPlot(Game.GraphicsDevice.Viewport.Bounds.Size
                                   - HandPlot.Size));
        }
    }
}

