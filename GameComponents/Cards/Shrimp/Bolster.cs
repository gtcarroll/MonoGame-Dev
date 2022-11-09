//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class Bolster : Card
//    {
//        public int BaseArmor { get; set; }
//        public int StrengthBonus { get; set; }

//        public Bolster() : base()
//        {
//            Name = "Bolster";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Bronze;
//            ManaCost = 2;

//            BaseArmor = 20;
//            StrengthBonus = 3;
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            Console.WriteLine(caster.Name + " gained " + BaseArmor + " armor");
//            caster.Armor += BaseArmor;

//            void addStrengthIfArmored()
//            {
//                Console.WriteLine(caster.Name + " has " + caster.Armor + " armor... ");
//                if (caster.Armor > 0)
//                {
//                    caster.Strength += StrengthBonus;
//                    Console.WriteLine("\t" + caster.Name + " gained " + StrengthBonus + " strength!");
//                }
//                caster.Effects[Phase.BeforeTurnStart].Remove(addStrengthIfArmored);
//            }

//            caster.Effects[Phase.BeforeTurnStart].Add(addStrengthIfArmored);
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            return new int[2] { 0, 0 };
//        }
//    }
//}

