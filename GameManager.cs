using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.ScreenManagement;
using EverythingUnder.Screens;

namespace EverythingUnder;

public class GameManager : Game
{
    #region Properties

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Random Random;

    public readonly ScreenManager ScreenManager;

    #endregion

    #region Constructors

    public GameManager()
    {
        Content.RootDirectory = "Content";

        Random = new Random();

        IsMouseVisible = true;

        // set up graphics device
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1440;
        _graphics.PreferredBackBufferHeight = 810;
        _graphics.ApplyChanges();

        // set up window properties
        Window.Title = "Everything Under";
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;

        // add screen manager component
        ScreenManager = new ScreenManager(this);
        Components.Add(ScreenManager);

    }

    public void OnResize(Object sender, EventArgs e)
    {
        ScreenManager.WasResized = true;
    }

    #endregion

    #region Loading Methods

    protected override void LoadContent()
    {
        base.LoadContent();

        ScreenManager.AddScreen(new LevelMapScreen(this));
    }

    #endregion

    #region Rendering Methods

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        base.Draw(gameTime);
    }

    #endregion
}
