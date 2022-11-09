using System;
using System.Linq;
using System.Collections.Generic;
using EverythingUnder.Cards;
using EverythingUnder.Combat;

namespace EverythingUnder.Characters
{
    public abstract class Player : Character
    {
        private const int HAND_MAX = 10;

        public List<Card> Hand;
        public List<Card> Deck;
        public List<Card> PlayDeck;
        public List<Card> Discard;
        public List<Card> Graveyard;

        public int Mana { get; set; }
        public int ManaRegen { get; set; }
        public int DrawPower { get; set; }

        public Player() : base()
        {
            Hand = new List<Card>();
            Discard = new List<Card>();
            Graveyard = new List<Card>();

            ManaRegen = 2;
            DrawPower = 4;

            Effects[Phase.TurnStart].Add(RemoveArmor);
            Effects[Phase.TurnStart].Add(RefreshMana);

            Effects[Phase.TurnEnd].Add(DiscardHand);
        }

        public bool GetAction(CombatState combat)
        {
            // prompt for card
            combat.Print();
            combat.PrintCardOptions(this);
            int i = int.Parse(Console.ReadKey(true).Key.ToString().Substring(1));
            if (i > 0 && i <= Hand.Count)
            {
                Card card = Hand[i - 1];
                while (CanPlay(combat, card) && card.Targets.Count < card.TargetCount)
                {
                    // prompt for target
                    combat.PrintTargetOptions(Team.Enemies);
                    i = int.Parse(Console.ReadKey(true).Key.ToString().Substring(1));
                    if (i > 0 && i <= combat.Teams[Team.Enemies].Count)
                    {
                        card.Targets.Add(combat.Teams[Team.Enemies][i - 1]);
                    }
                    else
                    {
                        card.AddRandomTarget(combat, this);
                    }
                }

                // play card
                Console.Clear();
                Play(combat, card);
                return true;
            }
            return false;
        }
        public bool CanPlay(CombatState combat, Card card)
        {
            return card.ManaCost <= Mana;
        }
        public void Play(CombatState combat, Card card)
        {
            if (CanPlay(combat, card))
            {
                Mana -= card.ManaCost;
                card.Cast(combat, this);
                Hand.Remove(card);
                Discard.Add(card);
            }
        }

        public bool Draw(CombatState combat)
        {
            if (PlayDeck.Count <= 0)
            {
                PlayDeck = Discard;
                Discard = new List<Card>();
                Shuffle(combat);
            }

            if (PlayDeck.Count > 0)
            {
                Card card = PlayDeck[PlayDeck.Count - 1];
                PlayDeck.RemoveAt(PlayDeck.Count - 1);

                if (Hand.Count < HAND_MAX)
                {
                    Hand.Add(card);
                    return true;
                }
                else
                {
                    Discard.Add(card);
                }
            }

            return false;
        }
        public bool Draw(CombatState combat, int amount)
        {
            while (amount > 0 && Draw(combat)) amount--;
            return amount == 0;
        }
        public void DiscardHand()
        {
            while (Hand.Count > 0)
            {
                Card card = Hand[Hand.Count - 1];
                Hand.Remove(card);
                Discard.Add(card);
            }
        }

        public void InitializePlayDeck(CombatState combat)
        {
            PlayDeck = new List<Card>(Deck.Count);
            foreach (Card card in Deck)
            {
                PlayDeck.Add(card.Clone());
            }
            Shuffle(combat);
        }
        public void Shuffle(CombatState combat)
        {
            PlayDeck = PlayDeck.OrderBy(x => combat.Random.Next()).ToList();
        }

        public void RefreshMana()
        {
            Mana = ManaRegen;
        }
    }
}

