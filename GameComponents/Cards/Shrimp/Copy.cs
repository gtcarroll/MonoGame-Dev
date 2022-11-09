//using System;
//using EverythingUnder.Cards;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class Copy : Card
//    {
//        public Copy() : base()
//        {
//            Name = "Copy";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Bronze;
//            ManaCost = 0;
//            TargetCount = 1;
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            base.Cast(combat, caster);
//            foreach (Character target in Targets)
//            {
//                if (caster is Player)
//                {
//                    Console.WriteLine(caster.Name + " copied " + target.NextMove + " from " + target.Name);
//                    Card copy = target.NextMove.Clone();
//                    copy.Name += " (copy)";
//                    ((Player)caster).Hand.Add(copy);
//                }
//            }
//            ResetTargets();
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            return new int[2] { 0, 0 };
//        }
//    }
//}

