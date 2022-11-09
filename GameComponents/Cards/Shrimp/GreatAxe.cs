//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class GreatAxe : Card
//    {
//        public int BaseDamage { get; set; }
//        public int CritDamage { get; set; }

//        public GreatAxe() : base()
//        {
//            Name = "Great Axe";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Bronze;
//            ManaCost = 1;
//            TargetCount = 1;

//            BaseDamage = 1;
//            CritDamage = 20;
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
//            int lo = BaseDamage + caster.Strength;
//            int hi = CritDamage + caster.Strength;
//            return new int[2] { lo, hi };
//        }

//        private int GetDamage(Combat combat, Character caster, Character target)
//        {
//            return combat.Random.Next(BaseDamage, CritDamage + 1) + caster.Strength;
//        }
//    }
//}

