//using System;
//using EverythingUnder.Cards.Enemy;
//using EverythingUnder.Events;

//namespace EverythingUnder.Characters
//{
//    public class Slime : Character
//    {
//        public int Turn { get; set; }
//        private int _cloneTurn;

//        public Slime() : this(30) { }
//        public Slime(int HP) : base()
//        {
//            Name = "Slime";
//            MaxHealth = HP;
//            Health = MaxHealth;
//            Turn = 1;
//        }

//        public override bool PrepareAttack(Combat combat)
//        {
//            if (_cloneTurn == 0) _cloneTurn = combat.Random.Next(2, 4);
//            NextMove = Turn == _cloneTurn ? new SlimeClone() : new Strike(6);
//            NextMove.AddRandomTargets(combat, this);
//            return true;
//        }
//        public override bool CastAttack(Combat combat)
//        {
//            NextMove.Cast(combat, this);
//            Turn++;
//            return true;
//        }
//    }
//}

