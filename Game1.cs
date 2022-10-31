using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using EverythingUnder.Screens;

namespace EverythingUnder;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly ScreenManager _screenManager;

    public Game1()
    {
        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

        _graphics = new GraphicsDeviceManager(this);

        // Set fullscreen
        //_graphics.IsFullScreen = true;
        //_graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Viewport vp = GraphicsDevice.Viewport;


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        LoadLevelScreen();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }

    private void LoadLevelScreen()
    {
        _screenManager.LoadScreen(new LevelMapScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
    }
}
