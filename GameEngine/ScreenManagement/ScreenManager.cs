using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace EverythingUnder.ScreenManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private List<GameScreen> _screens = new List<GameScreen>();
        private Stack<GameScreen> _updateStack = new Stack<GameScreen>();

        private SpriteBatch _spriteBatch;

        #endregion

        #region Properties


        public RenderTarget2D SafeArea;
        public Rectangle SafeRectangle;
        public bool WasResized;

        public InputState InputState;


        #endregion

        #region Constructors

        public ScreenManager(Game game) : base(game)
        {
            InputState = new InputState();
            TouchPanel.EnabledGestures = GestureType.None;

            WasResized = true;
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SafeArea = new RenderTarget2D(GraphicsDevice, 1920, 1080, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

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

        #region Game Cycle Methods

        public override void Update(GameTime time)
        {
            InputState.Update();

            // close the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

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

                // the topmost non-closing screen handles input
                if (!screen.IsClosing && isFocused)
                {
                    screen.HandleInput(time, InputState);
                    isFocused = false;
                }

                screen.Update(time, isFocused, isCovered);

                // any non-popup screen will cover the screens below
                if (!screen.IsClosing && !screen.IsPopUp)
                {
                    isCovered = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw foreground to resolution-locked render target
            Game.GraphicsDevice.SetRenderTarget(SafeArea);
            Game.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            foreach (GameScreen screen in _screens)
            {
                if (!screen.IsCovered)
                {
                    screen.Draw(gameTime, _spriteBatch);
                }
            }
            _spriteBatch.End();

            // Draw background and render target to screen
            Game.GraphicsDevice.SetRenderTarget(null);
            Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            if (WasResized)
            {
                SafeRectangle = GetSafeAreaDestinationRectangle();
                WasResized = false;
            }

            _spriteBatch.Begin();
            _spriteBatch.Draw(SafeArea, SafeRectangle, Color.White);
            _spriteBatch.End();
        }

        private Rectangle GetSafeAreaDestinationRectangle()
        {
            float outputAspectRatio = Game.Window.ClientBounds.Width
                                    / (float)Game.Window.ClientBounds.Height;
            float preferredAspectRatio = SafeArea.Width / (float)SafeArea.Height;//SafeArea.Height / SafeArea.Width;

            Console.WriteLine(Game.Window.ClientBounds);

            if (outputAspectRatio <= preferredAspectRatio)
            {
                // output is taller
                int presentHeight = (int)((Game.Window.ClientBounds.Width / preferredAspectRatio));
                int barHeight = (Game.Window.ClientBounds.Height - presentHeight) / 2;

                return new Rectangle(0, barHeight, Game.Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // output is wider
                int presentWidth = (int)((Game.Window.ClientBounds.Height * preferredAspectRatio));
                int barWidth = (Game.Window.ClientBounds.Width - presentWidth) / 2;

                return new Rectangle(barWidth, 0, presentWidth, Game.Window.ClientBounds.Height);
            }
        }

        #endregion

        #region State Methods

        public Point GetGamePosition(Vector2 mousePos)
        {
            float outputAspectRatio = Game.Window.ClientBounds.Width
                                    / (float)Game.Window.ClientBounds.Height;
            float preferredAspectRatio = SafeArea.Width / (float)SafeArea.Height;//SafeArea.Height / SafeArea.Width;

            //Console.WriteLine(Game.Window.ClientBounds);

            if (outputAspectRatio <= preferredAspectRatio)
            {
                // output is taller
                int presentHeight = (int)((Game.Window.ClientBounds.Width / preferredAspectRatio));
                int barHeight = (Game.Window.ClientBounds.Height - presentHeight) / 2;
                int barWidth = 0;

                //Console.WriteLine(mousePos.Y - barHeight);
                //Console.WriteLine(Game.Window.ClientBounds.Height - 2 * barHeight);
                //Console.WriteLine(SafeArea.Height);
                int gameX = (int)((mousePos.X - barWidth) / ((Game.Window.ClientBounds.Width - 2 * barWidth) / (float)SafeArea.Width));
                int gameY = (int)((mousePos.Y - barHeight) / ((Game.Window.ClientBounds.Height - 2 * barHeight) / (float)SafeArea.Height));

                return new Point(gameX, gameY);
            }
            else
            {
                // output is wider
                int presentWidth = (int)((Game.Window.ClientBounds.Height * preferredAspectRatio));
                int barWidth = (Game.Window.ClientBounds.Width - presentWidth) / 2;
                int barHeight = 0;

                int gameX = (int)((mousePos.X - barWidth) / ((Game.Window.ClientBounds.Width - 2 * barWidth) / (float)SafeArea.Width));
                int gameY = (int)((mousePos.Y - barHeight) / ((Game.Window.ClientBounds.Height - 2 * barHeight) / (float)SafeArea.Height));

                return new Point(gameX, gameY);
            }
        }

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

            // set the enabled gestures to that of the new topmost screen
            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures =
                    _screens[_screens.Count - 1].EnabledGestures;
            }
        }

        #endregion
    }
}

