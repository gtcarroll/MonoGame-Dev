//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class GreatShield : Card
//    {
//        public int BaseArmor { get; set; }
//        public int CritArmor { get; set; }

//        public GreatShield() : base()
//        {
//            Name = "Great Shield";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Bronze;
//            ManaCost = 1;

//            BaseArmor = 1;
//            CritArmor = 20;
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            int armorGained = combat.Random.Next(BaseArmor, CritArmor + 1);
//            Console.WriteLine(caster.Name + " gained " + armorGained + " armor");
//            caster.Armor += armorGained;
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            return new int[2] { 0, 0 };
//        }
//    }
//}

