using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace EverythingUnder.ScreenManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Properties

        private List<GameScreen> _screens = new List<GameScreen>();
        private Stack<GameScreen> _updateStack = new Stack<GameScreen>();

        public SpriteBatch SpriteBatch;
        public InputState InputState;

        #endregion

        #region Constructors

        public ScreenManager(Game game) : base(game)
        {
            TouchPanel.EnabledGestures = GestureType.None;
        }

        #endregion

        #region Loading Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = Game.Content;

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameScreen screen in _screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen screen in _screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Rendering Methods

        public override void Update(GameTime gameTime)
        {
            foreach (GameScreen screen in _screens)
            {
                _updateStack.Push(screen);
            }

            // is the game window itself active?
            bool isFocused = Game.IsActive;
            // are lower screens completely covered?
            bool isCovered = false;

            while (_updateStack.Count > 0)
            {
                GameScreen screen = _updateStack.Pop();

                screen.Update(gameTime, isFocused, isCovered);

                if (!screen.IsClosing)
                {
                    // the topmost non-closing screen handles input
                    if (isFocused)
                    {
                        screen.HandleInput(InputState);
                        isFocused = false;
                    }

                    // any non-popup screen will cover the screens below
                    if (!screen.IsPopUp)
                    {
                        isCovered = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in _screens)
            {
                if (!screen.IsCovered)
                {
                    screen.Draw(gameTime);
                }
            }
        }

        #endregion

        #region State Methods

        public void AddScreen(GameScreen screen, int player = -1)
        {
            screen.ControllingPlayer = player;
            screen.IsClosing = false;

            screen.LoadContent();

            _screens.Add(screen);

            // set the enabled gestures to that of the new topmost screen
            TouchPanel.EnabledGestures = screen.EnabledGestures;
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();

            _screens.Remove(screen);
            // should we remove from _updateStack as well?

            // set the enabled gestures to that of the new topmost screen
            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
            }
        }

        #endregion
    }
}

