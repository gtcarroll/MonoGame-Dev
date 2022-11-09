using System;
using System.Collections.Generic;
using EverythingUnder.Cards;
using System.Text;
using EverythingUnder.Characters;

namespace EverythingUnder.Combat
{
    public enum Team
    {
        Enemies,
        Friends,
        Players,
        ALL
    }

    public enum Phase
    {
        CombatStart,
        CombatEnd,
        BeforeTurnStart,
        TurnStart,
        TurnEnd
    }

    public class CombatState
    {
        #region Properties

        public Dictionary<Team, List<Character>> Teams;
        public Random Random;

        private bool _isPlayerTurn;
        private bool _isCombatOver;

        #endregion

        #region Constructors

        public CombatState(List<Character> friends, List<Character> enemies)
        {
            _isPlayerTurn = true;
            _isCombatOver = false;
            Random = new Random();

            // Initialize team lists
            Teams = new Dictionary<Team, List<Character>>();
            foreach (Team team in Team.GetValues(typeof(Team)))
            {
                Teams[team] = new List<Character>();
            }

            // Summon all characters
            foreach (Character friend in friends)
            {
                Summon(friend, Team.Friends);
            }
            foreach (Character enemy in enemies)
            {
                Summon(enemy, Team.Enemies);
            }
        }

        #endregion

        #region State Methods

        public void Start()
        {
            // Trigger combat start effects
            Console.WriteLine("--- Combat Start ---");
            TriggerPhase(Phase.CombatStart);

            // Run combat until one team wins
            while (Teams[Team.Enemies].Count > 0
                   && Teams[Team.Friends].Count > 0)
            {
                RunTurn();
            }
        }

        public void End(bool isVictory)
        {
            // Trigger combat end effects
            Console.WriteLine("--- Combat End ---");
            TriggerPhase(Phase.CombatEnd);
            _isCombatOver = true;

            // Display combat results
            if (isVictory) Console.WriteLine("--- VICTORY! ---");
            else Console.WriteLine("--- DEFEAT! ---");
        }

        public bool Summon(Character character, Team team)
        {
            if (team == Team.ALL || team == Team.Players) return false;
            Console.WriteLine("- Summoning: " + character.Name + " -");

            if (character is Player)
            {
                Teams[Team.Players].Add(character);
                ((Player)character).InitializePlayDeck(this);
            }

            if (team == Team.Friends)
            {
                Teams[Team.Friends].Add(character);
            }
            else if (team == Team.Enemies)
            {
                Teams[Team.Enemies].Add(character);
            }

            Teams[Team.ALL].Add(character);

            character.PrepareAttack(this);
            return true;
        }

        public bool Unsummon(Character character)
        {
            Console.WriteLine("- Unsummoning: " + character.Name + " -");

            if (character is Player && Teams[Team.Players].Contains(character))
            {
                Teams[Team.Players].Remove(character);
            }

            if (Teams[Team.Enemies].Contains(character))
            {
                Teams[Team.Enemies].Remove(character);
            }
            else if (Teams[Team.Friends].Contains(character))
            {
                Teams[Team.Friends].Remove(character);
            }

            bool result = Teams[Team.ALL].Remove(character);

            CheckCombatEnd();
            return result;
        }

        public Team GetTeam(Character character)
        {
            if (Teams[Team.Friends].Contains(character))
            {
                return Team.Friends;
            }
            if (Teams[Team.Enemies].Contains(character))
            {
                return Team.Enemies;
            }
            return Team.ALL;
        }

        public Team GetOpposingTeam(Character character)
        {
            if (Teams[Team.Friends].Contains(character))
            {
                return Team.Enemies;
            }
            if (Teams[Team.Enemies].Contains(character))
            {
                return Team.Friends;
            }
            return Team.ALL;
        }

        #endregion

        #region Helper Methods

        private void RunTurn()
        {
            Team team = _isPlayerTurn ? Team.Friends : Team.Enemies;

            // Trigger turn start effects
            TriggerPhase(Phase.BeforeTurnStart);
            Console.WriteLine("-- Turn Start: " + team + " --");
            TriggerPhase(Phase.TurnStart);


            if (_isPlayerTurn)
            {
                // ALL characters prepare attacks
                PrepareTeamAttacks(Team.ALL);

                // Players take their turn
                foreach (Player player in Teams[Team.Players])
                {
                    player.Draw(this, player.DrawPower);
                    while (!_isCombatOver && player.GetAction(this)) { }
                }

                if (!_isCombatOver) Console.Clear();
            }

            // Cast prepared attacks
            CastTeamAttacks(team);

            // If combat isn't over...
            if (!_isCombatOver)
            {
                // Trigger turn end effects
                Console.WriteLine("-- Turn End: " + team + " --\n");
                TriggerPhase(Phase.TurnEnd);
            }

            // Toggle turn
            _isPlayerTurn = !_isPlayerTurn;
        }

        private void CheckCombatEnd()
        {
            if (Teams[Team.Enemies].Count <= 0) End(true);
            if (Teams[Team.Players].Count <= 0) End(false);
        }

        private void TriggerPhase(Phase phase)
        {
            // Get relevant team
            Team team = Team.ALL;
            if (phase != Phase.CombatStart && phase != Phase.CombatEnd)
            {
                team = _isPlayerTurn ? Team.Friends : Team.Enemies;
            }

            // Execute each character's effects
            foreach (Character character in Teams[team])
            {
                // Copy effects list as it may be modified during iteration
                Action[] effects = new Action[character.Effects[phase].Count];
                character.Effects[phase].CopyTo(effects);

                foreach (Action effect in effects)
                {
                    effect();
                }

                if (_isCombatOver) break;
            }
        }

        private void CastTeamAttacks(Team team)
        {
            // Copy character list as it may be modified during iteration
            Character[] startingTeam = new Character[Teams[team].Count];
            Teams[team].CopyTo(startingTeam);

            foreach (Character character in startingTeam)
            {
                character.CastAttack(this);
                if (_isCombatOver) break;
            }
        }
        private void PrepareTeamAttacks(Team team)
        {
            foreach (Character character in Teams[team])
            {
                character.PrepareAttack(this);
            }
        }

        #endregion

        #region ToString

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine(this.ToString());
        }

        public void PrintCardOptions(Player player)
        {
            Console.WriteLine("Pick a card\t MANA: " + player.Mana);
            Console.WriteLine("0: End Turn");
            int i = 1;
            foreach (Card card in player.Hand)
            {
                Console.WriteLine(i + ": " + card);
                i++;
            }
        }

        public void PrintTargetOptions(Team team)
        {
            Console.WriteLine("Pick a target");
            List<Character> targets = Teams[team];
            int i = 1;
            foreach (Character target in targets)
            {
                Console.WriteLine(i + ": " + target);
                i++;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (Character enemy in Teams[Team.Enemies])
            {
                result.Append(enemy).Append("\n");
            }
            result.Append("\t-VS-\n");
            foreach (Character friend in Teams[Team.Friends])
            {
                result.Append(friend).Append("\n");
            }
            return result.ToString();
        }

        #endregion
    }
}

