using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Cards;

namespace EverythingUnder.GUI
{
    public class DeckPlot : GUIPlot
    {
        private int _maxDeckNodes;
        private bool _isBottomRow;

        private Point _initialPosition;
        private Point _cardGap;

        public static Point Size = new Point(128, 256);

        public CardSprite TopCard;
        public List<DeckSprite> OtherCards;

        public DeckPlot(GameManager game, Point location, int maxNodes = 9, bool isBottom = false) : base(game)
        {
            // initialize DeckPlot fields
            _maxDeckNodes = maxNodes + 1;
            _isBottomRow = isBottom;
            _initialPosition = new Point(64, 64);
            _cardGap = new Point(0, 6);

            OtherCards = new List<DeckSprite>();

            // initialize GUIPlot params
            Direction = PlotDirection.Column;
            ScreenSpace = new Rectangle(location, Size);

            // create all nodes
            AddAllNodes();
        }

        public override void DrawFG(SpriteBatch spriteBatch)
        {
            // draw cards
            for (int i = Nodes.Count - 1; i >= 0; i--)
            {
                Nodes[i].Draw(spriteBatch);
            }
        }

        public override GUINode GetNearestNode(Point point)
        {
            return Nodes[0];
        }

        private void AddAllNodes()
        {
            Point nodePosition = ScreenSpace.Location + _initialPosition;

            Nodes.Add(new CardNode(nodePosition, _isBottomRow, false));
            Nodes[0].LoadContent(Game);
            //Nodes[0].RemoveSprite();

            for (int i = 1; i < _maxDeckNodes; i++)
            {
                nodePosition += _cardGap;

                Nodes.Add(new DeckNode(nodePosition));
                Nodes[i].LoadContent(Game);
            }
        }

        public SpriteGroup Pop()
        {
            SpriteGroup topCard = Nodes[0].Sprite;
            Nodes[0].RemoveSprite();
            UpdateCardPositions();
            return topCard;
        }

        public void Push(GUINode node)
        {
            for (int i = Nodes.Count - 1; i > 0; i--)
            {
                Nodes[i].AddSprite(Nodes[i - 1].Sprite);
                Nodes[i - 1].RemoveSprite();
            }

            CardBackSprite next = new CardBackSprite(node.Sprite.CurrentState.Center);
            next.LoadContent(Game.Content);
            Nodes[0].AddSprite(next);
                
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        private void UpdateCardPositions()
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Nodes[i].Sprite == null)
                {
                    Nodes[i].AddSprite(Nodes[i + 1].Sprite);
                    Nodes[i + 1].RemoveSprite();
                }
            }
        }
    }
}

