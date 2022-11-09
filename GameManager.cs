﻿using System;
using EverythingUnder.ScreenManagement;
using EverythingUnder.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        _graphics = new GraphicsDeviceManager(this);
        //_graphics.GraphicsDevice.Viewport.AspectRatio 

        // Set fullscreen
        //_graphics.IsFullScreen = true;
        //_graphics.ApplyChanges();

        Random = new Random();

        IsMouseVisible = true;

        ScreenManager = new ScreenManager(this);
        Components.Add(ScreenManager);

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