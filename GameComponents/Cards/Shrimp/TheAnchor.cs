//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class TheAnchor : Card
//    {
//        public int BaseDamage { get; set; }

//        public TheAnchor() : base()
//        {
//            Name = "The Anchor";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Golden;
//            ManaCost = 2;
//            TargetCount = 1;

//            BaseDamage = 2;
//            Description = "Deal " + BaseDamage + " for each card in your starting deck.";
//            Keywords.Add(Keyword.Heavy);
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            base.Cast(combat, caster);
//            foreach (Character target in Targets)
//            {
//                int damage = GetDamage(combat, caster, target);
//                Console.WriteLine(caster.Name + " dealt " + damage + " to " + target.Name);
//                target.Damage(combat, damage);
//            }
//            ResetTargets();
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            int dmg = GetDamage(combat, caster, target);
//            return new int[2] { dmg, dmg };
//        }

//        private int GetDamage(Combat combat, Character caster, Character target)
//        {
//            int cardCount = 1;
//            if (caster is Player)
//            {
//                Player player = (Player)caster;
//                cardCount = player.Deck.Count;
//            }
//            return (BaseDamage * cardCount) + caster.Strength;
//        }
//    }
//}

