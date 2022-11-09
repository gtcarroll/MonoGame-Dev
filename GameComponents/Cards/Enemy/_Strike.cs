//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Enemy
//{
//    public class Strike : Card
//    {
//        public int BaseDamage { get; set; }

//        public Strike() : this("Strike", 10) { }
//        public Strike(String name) : this(name, 10) { }
//        public Strike(int damage) : this("Strike", damage) { }
//        public Strike(String name, int damage) : base()
//        {
//            Name = name;
//            Color = Color.Enemy;
//            Rarity = Rarity.Bronze;
//            ManaCost = 1;
//            TargetCount = 1;

//            BaseDamage = damage;
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

