//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Shrimp
//{
//    public class Block : Card
//    {
//        public int BaseArmor { get; set; }

//        public Block() : base()
//        {
//            Name = "Block";
//            Color = Color.Shrimp;
//            Rarity = Rarity.Basic;
//            ManaCost = 1;
//            BaseArmor = 10;
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            Console.WriteLine(caster.Name + " gained " + BaseArmor + " armor");
//            caster.Armor += BaseArmor;
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            return new int[2] { 0, 0 };
//        }
//    }
//}

