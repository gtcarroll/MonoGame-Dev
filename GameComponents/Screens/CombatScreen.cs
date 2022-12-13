using System.Collections.Generic;
using Microsoft.Xna.Framework;

using EverythingUnder.Characters;
using EverythingUnder.Combat;
using EverythingUnder.GUI;
using EverythingUnder.ScreenManagement;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.Screens
{
    public class CombatScreen : GameScreen
    {
        #region Properties

        private GameManager _game;

        private CombatState _state;
        //private CombatDisplay _display;
        private CombatController _controller;

        private GUIGarden _garden;

        #endregion

        #region Constructors

        public CombatScreen(GameManager game) : base()
        {
            _game = game;

            List<Character> Friendlies = new List<Character>
            {
                //new Shrimp()
            };
            List<Character> Enemies = new List<Character>
            {
                //new Slime(30), new Slime(30)
            };
            _state = new CombatState(Friendlies, Enemies);

            //_display = new CombatDisplay(game, Friendlies, Enemies);

            _garden = new CombatGarden(game);

            _controller = new CombatController();

            //_display.LoadContent();
        }

        #endregion

        #region Loading Methods

        public override void LoadContent()
        {
            base.LoadContent();

            _garden.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            //_display.UnloadContent();
        }

        #endregion

        #region Game Cycle Methods

        public override void HandleInput(GameTime time, InputState input)
        {
            if (input.IsCancelPressed())
            {
                IsClosing = true;
            }

            _garden.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool isFocused,
                                                       bool isCovered)
        {
            if (IsClosing && UpdateTransition(gameTime))
            {
                _game.ScreenManager.RemoveScreen(this);
            }

            _garden.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.Clear(Color.Black);

            _garden.Draw(spriteBatch);
        }

        #endregion
    }
}

