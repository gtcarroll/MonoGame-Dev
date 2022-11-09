//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class Strike : Card
//    {
//        public int BaseDamage { get; set; }

//        public Strike() : base()
//        {
//            Name = "Strike";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Basic;
//            ManaCost = 1;
//            TargetCount = 1;

//            BaseDamage = 10;
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
//            return BaseDamage + caster.Strength;
//        }
//    }
//}

