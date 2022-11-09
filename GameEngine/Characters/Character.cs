using System;
using System.Collections.Generic;
using EverythingUnder.Cards;
using EverythingUnder.Combat;

namespace EverythingUnder.Characters
{
    public class Character
    {
        public String Name { get; set; }

        public Card NextMove { get; set; }

        public int Strength { get; set; }
        public int SpellPower { get; set; }

        public int Armor { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public Dictionary<Phase, List<Action>> Effects;

        public List<Action> CombatStartEffects;
        public List<Action> CombatEndEffects;
        public List<Action> BeforeTurnStartEffects;
        public List<Action> TurnStartEffects;
        public List<Action> TurnEndEffects;

        public Character()
        {
            Effects = new Dictionary<Phase, List<Action>>();
            foreach (Phase phase in Phase.GetValues(typeof(Phase)))
            {
                Effects[phase] = new List<Action>();
            }
        }

        public void Damage(CombatState combat, int damage)
        {
            Armor -= damage;
            if (Armor < 0)
            {
                Health += Armor;
                Armor = 0;
            }
            if (Health <= 0)
            {
                Health = 0;
                combat.Unsummon(this);
            }
        }

        public virtual bool PrepareAttack(CombatState combat)
        {
            return false;
        }
        public virtual bool CastAttack(CombatState combat)
        {
            return false;
        }

        public void RemoveArmor()
        {
            this.Armor = 0;
        }

        public override string ToString()
        {
            return Name + ": "
                 + Health + "/" + MaxHealth
                 + (Armor > 0 ? " +" + Armor : "")
                 + (NextMove != null ? " [" + NextMove + "]" : "");
        }
    }
}

