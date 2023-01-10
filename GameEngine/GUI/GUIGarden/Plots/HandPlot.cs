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

        public static Point Size = new Point(896, 256);

        private Point _initialDelta;
        private Point _downGap;
        private Point _upGap;

        private Point _deckCenter;

        private int _numCards;

        //public List<CardSprite> Cards;

        //public DeckNode DrawPile;
        //public DeckNode DiscardPile;

        public HandPlot(GameManager game, AnimationQueue animationQueue,
                        Point location)
            : base(game, animationQueue)
        {
            // initialize HandPlot params
            _initialDelta = new Point(3, 64);
            _downGap = new Point(67, 108);
            _upGap = new Point(67, -108);

            //Cards = new List<CardSprite>();
            _numCards = 0;

            // initialize GUIPlot params
            Direction = PlotDirection.Row;
            ScreenSpace = new Rectangle(location, Size);

            // create and connect all nodes
            AddAllNodes();
            ConnectAllNodes();
        }

        public override void DrawFG(SpriteBatch spriteBatch)
        {
            // draw mana
            //Nodes[11].Draw(spriteBatch);

            // draw discard
            //Nodes[1].Draw(spriteBatch);

            // draw graveyard
            //Nodes[0].Draw(spriteBatch);

            // draw deck
            //Nodes[12].Draw(spriteBatch);

            // draw cards
            for (int i = MaxHandSize - 1; i >= 0; i--)
            {
                Nodes[i].Draw(spriteBatch);
            }
        }

        private void AddAllNodes()
        {
            Point nodePosition = GetNodePosition(0);

            // add graveyard
            //Nodes.Add(new GraveNode(nodePosition));
            //nodePosition += _upGap;

            // add discard pile
            //DiscardPile = new DeckNode(nodePosition, false);
            //Nodes.Add(DiscardPile);
            //nodePosition += _downGap;

            // add card nodes
            for (int i = 0; i < MaxHandSize; i++)
            {
                bool isBottomRow = i % 2 == 0;
                Nodes.Add(new CardNode(nodePosition, isBottomRow));
                Nodes[i].RemoveSprite();

                nodePosition += isBottomRow ? _upGap : _downGap;
            }

            // add draw pile
            //_deckCenter = nodePosition;
            //DrawPile = new DeckNode(_deckCenter, true);
            //Nodes.Add(DrawPile);
            //nodePosition += _upGap;

            // add mana pile
            //Nodes.Add(new ManaNode(nodePosition));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public void ShuffleInto(DeckPlot discard, DeckPlot draw)
        {

            AnimationQueue.BeginFrame();

            List<SpriteGroup> sprites = discard.Flush();

            AnimationQueue.BeginFrame();

            draw.Fill(sprites.Count);

            AnimationQueue.EndFrame();
        }

        public void InitDeck(DeckPlot deck, int size)
        {
            AnimationQueue.BeginFrame();

            deck.Fill(size);

            AnimationQueue.EndFrame();
        }

        public bool DrawCard(DeckPlot deck)
        {
            if (_numCards >= MaxHandSize) return false;

            int i = _numCards;

            AnimationQueue.BeginFrame();

            // retrieve card back sprite from deck
            SpriteGroup backSprite = deck.Pop();

            if (backSprite == null)
            {
                AnimationQueue.EndFrame();
                return false;
            }

            // instantiate and add card sprite
            CardSprite cardSprite = new CardSprite(Nodes[i].Center, true);
            cardSprite.LoadContent(Game.Content);

            Nodes[i].AddSprite(cardSprite);

            // calc midpt between Node and _deckCenter
            //Point midPt = new Point(backSprite.CurrentState.Center.X,
            //    (Nodes[i].Center.Y + backSprite.CurrentState.Center.Y) / 2);


            // queue draw animation for card
            AnimationQueue.Add(
                cardSprite,
                new CardDrawAnimation(
                    cardSprite,
                    cardSprite.DefaultState.GetCopyAt(Nodes[i].Center),
                    backSprite));

            // queue draw animation for card back
            deck.Nodes[0].FrontSprites.Add(backSprite);
            Point verticalPoint = new Point(backSprite.CurrentState.Center.X,
                                            Nodes[i].Center.Y);
            AnimationQueue.Add(
                backSprite,
                new FlipDownAnimation(backSprite, verticalPoint));

            _numCards++;
            AnimationQueue.EndFrame();
            return true;
        }

        public bool RemoveCard(int i, DeckPlot discardPile)
        {
            if (_numCards <= i) return false;

            AnimationQueue.BeginFrame();

            discardPile.Push(Nodes[i]);

            //Cards.RemoveAt(i);
            Nodes[i].RemoveSprite();
            _numCards--;

            UpdateCardPositions();

            AnimationQueue.EndFrame();

            return true;
        }

        private void UpdateCardPositions()
        {
            // shuffle all sprites up one node
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Nodes[i].Sprite == null
                    && Nodes[i + 1].Sprite != null)
                {
                    Nodes[i].AddSprite(Nodes[i + 1].Sprite);
                    Nodes[i + 1].RemoveSprite();

                    AnimationQueue.Add(
                        Nodes[i].Sprite,
                        Nodes[i].Sprite.RepositionAnimation(Nodes[i].Center));
                }
            }

            //Point nodePosition = GetNodePosition(2);

            //for (int i = 0; i < _numCards; i++)
            //{
            //    SpriteGroup cardSprite = Nodes[i].Sprite;

            //    if (cardSprite.Anchor != nodePosition)
            //    {
            //        //cardSprite.Anchor = nodePosition;
            //        Nodes[i + 1].RemoveSprite();
            //        Nodes[i].AddSprite(cardSprite);
            //    }

            //    nodePosition += i % 2 == 0 ? _upGap : _downGap;
            //}
        }

        private Point GetNodePosition(int index)
        {
            Point nodePosition = ScreenSpace.Location + _initialDelta + _downGap;

            for (int i = 0; i < index; i++)
            {
                nodePosition += i % 2 == 0 ? _upGap : _downGap;
            }

            return nodePosition;
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
        }

        // implement functions for adding/removing cards,
        // placing them relative to the plot,
        // and managing neighbors
    }
}

