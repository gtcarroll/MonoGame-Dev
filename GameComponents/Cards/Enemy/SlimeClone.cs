//using System;
//using EverythingUnder.Characters;
//using EverythingUnder.Events;

//namespace EverythingUnder.Cards.Enemy
//{
//    public class SlimeClone : Card
//    {
//        public SlimeClone() : base()
//        {
//            Name = "Slime Clone";
//            Color = Color.Enemy;
//            Rarity = Rarity.Silver;
//            ManaCost = 1;
//        }

//        public override void Cast(Combat combat, Character caster)
//        {
//            Console.WriteLine(caster.Name + " summoned a slime clone");
//            combat.Summon(new Slime(caster.Health), combat.GetTeam(caster));
//        }
//        public override int[] CalculateDamageRange(Combat combat, Character caster, Character target)
//        {
//            return new int[2] { 0, 0 };
//        }
//    }
//}

