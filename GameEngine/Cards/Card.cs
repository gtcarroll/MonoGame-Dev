using System;
using System.Collections.Generic;
using EverythingUnder.Characters;
using EverythingUnder.Combat;

namespace EverythingUnder.Cards
{
    public enum Color
    {
        Enemy,
        Tarot,
        Shrimp,
        Squid,
        Frog,
        Otter
    }

    public enum Rarity
    {
        Basic,
        Bronze,
        Silver,
        Golden
    }

    public enum Keyword
    {
        Ghost,
        Doomed,
        Heavy,
        Flying
    }

    public abstract class Card
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public Color Color { get; set; }
        public Rarity Rarity { get; set; }
        public int ManaCost { get; set; }
        public int TargetCount { get; set; }

        public List<Character> Targets { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<bool> Enchantments { get; set; }
        public List<bool> Curses { get; set; }

        public Card()
        {
            Targets = new List<Character>();
            Keywords = new List<Keyword>();
        }

        public virtual void Cast(CombatState combat, Character caster)
        {
            if (Targets.Count < TargetCount) AddRandomTargets(combat, caster);
        }

        public abstract int[] CalculateDamageRange(CombatState combat,
                                                   Character caster,
                                                   Character target);

        public bool AddRandomTarget(CombatState combat, Character caster) =>
            AddRandomTargets(combat, caster, 1);
        public bool AddRandomTargets(CombatState combat, Character caster) =>
            AddRandomTargets(combat, caster, TargetCount - Targets.Count);
        public bool AddRandomTargets(CombatState combat, Character caster,
                                                    int numTargets)
        {
            List<Character> team = combat.Teams[combat.GetOpposingTeam(caster)];
            List<Character> newTargets = new List<Character>();
            foreach (Character character in team)
            {
                if (!Targets.Contains(character)) newTargets.Add(character);
            }

            int added = 0;
            while (added < numTargets)
            {
                if (newTargets.Count <= 0) return false;

                int index = combat.Random.Next(0, newTargets.Count);
                Targets.Add(newTargets[index]);
                newTargets.RemoveAt(index);
                added++;
            }
            return true;
        }

        public override string ToString()
        {
            return "(" + ManaCost + ") " + Name;
        }

        public void ResetTargets()
        {
            Targets.Clear();
        }

        public Card Clone()
        {
            Card clone = (Card)this.MemberwiseClone();
            clone.Targets = new List<Character>();
            return clone;
        }
    }
}

