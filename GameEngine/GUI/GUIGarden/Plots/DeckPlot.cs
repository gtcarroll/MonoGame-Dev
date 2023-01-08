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

        public int DeckSize
        {
            get { return _deckSize; }
        }
        private int _deckSize;

        public static Point Size = new Point(128, 256);

        public CardSprite TopCard;
        public List<DeckSprite> OtherCards;

        public DeckPlot(GameManager game, Point location, int maxNodes = 9, bool isBottom = false, int deckSize = 0) : base(game)
        {
            // initialize DeckPlot fields
            _maxDeckNodes = maxNodes + 1;
            _isBottomRow = isBottom;
            _initialPosition = new Point(64, 64);
            _cardGap = new Point(0, 6);
            _deckSize = deckSize;

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
            if (_deckSize < 1) Nodes[0].RemoveSprite();
            //Nodes[0].RemoveSprite();

            for (int i = 1; i < _maxDeckNodes; i++)
            {
                nodePosition += _cardGap;

                Nodes.Add(new DeckNode(nodePosition));
                if (_deckSize < i + 1) Nodes[i].RemoveSprite();
                Nodes[i].LoadContent(Game);
            }
        }

        public SpriteGroup Pop()
        {
            if (_deckSize <= 0) { return null; }

            _deckSize--;

            SpriteGroup topCard = Nodes[0].Sprite;
            Nodes[0].RemoveSprite();
            UpdateCardPositions();
            return topCard;
        }

        public void Push(GUINode node)
        {
            _deckSize++;

            for (int i = Nodes.Count - 1; i > 0; i--)
            {
                Nodes[i].AddSprite(Nodes[i - 1].Sprite);
                Nodes[i - 1].RemoveSprite();
            }

            SpriteGroup sprite = node.Sprite;

            // calc midpt between node and this deck
            Point midPt = new Point(
                (Nodes[0].Center.X + sprite.CurrentState.Center.X) / 2,
                (Nodes[0].Center.Y + sprite.CurrentState.Center.Y) / 2);

            // animate node's sprite flipping down
            Nodes[0].BackSprites.Add(sprite);
            sprite.Animation =
                new SpriteGroupAnimation(
                    sprite,
                    sprite.GetVerticallyFlattenedState().GetCopyAt(midPt),
                    new FlipFunction(256, false));

            // instantiate card back sprite
            CardBackSprite next = new CardBackSprite(midPt);
            next.LoadContent(Game.Content);
            Nodes[0].AddSprite(next);

            // animate card back flipping up
            next.CurrentState = next.GetVerticallyFlattenedState().GetCopyAt(midPt);
            next.Animation =
                new SpriteGroupAnimation(
                    next,
                    next.DefaultState.GetCopyAt(Nodes[0].Center),
                    new FlipFunction(256, true));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        private void UpdateCardPositions()
        {
            // shuffle all sprites up one node
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Nodes[i].Sprite == null)
                {
                    Nodes[i].AddSprite(Nodes[i + 1].Sprite);
                    Nodes[i + 1].RemoveSprite();
                }
            }

            // if deck has more cards, add another sprite on bottom
            if (_deckSize >= _maxDeckNodes)
            {
                Nodes[Nodes.Count - 1].AddSprite(new CardBackSprite(Point.Zero));
                Nodes[Nodes.Count - 1].LoadContent(Game);

            }
        }
    }
}

