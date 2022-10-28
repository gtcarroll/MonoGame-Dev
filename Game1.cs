using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HexMap.Graphics;
using HexMap.Input;
using Levels;
using System.Reflection;
using System.Collections.Generic;

namespace HexMap;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Random _random;

    private Camera _camera;

    private CubicSpline3D _spline3D;

    private LevelMap _levelMap;

    private Model _prism; 

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Viewport vp = GraphicsDevice.Viewport;

        _random = new Random();

        _camera = new Camera(this);

        _levelMap = new LevelMap(_random, 100);

        _spline3D = new CubicSpline3D(_levelMap.CameraPositions);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _prism = Content.Load<Model>("hexagonal-prism");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        FlatKeyboard keyboard = FlatKeyboard.Instance;
        keyboard.Update();

        FlatMouse mouse = FlatMouse.Instance;
        mouse.Update();

        if (keyboard.IsKeyClicked(Keys.F))
        {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        if (keyboard.IsKeyDown(Keys.Q))
        {
            _camera.MoveZ(-.1f);
        }
        if (keyboard.IsKeyDown(Keys.E))
        {
            _camera.MoveZ(.1f);
        }
        if (keyboard.IsKeyDown(Keys.W))
        {
            _camera.TiltX(-MathHelper.PiOver4 / 120);
        }
        if (keyboard.IsKeyDown(Keys.S))
        {
            _camera.TiltX(MathHelper.PiOver4 / 120);
        }
        if (keyboard.IsKeyDown(Keys.A))
        {
            _camera.TiltY(-MathHelper.PiOver4 / 120);
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            _camera.TiltY(MathHelper.PiOver4 / 120);
        }

        bool isShifted = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

        if (keyboard.IsKeyClicked(Keys.Space) && isShifted)
        {
            _levelMap = new LevelMap(_random, 100);
            _spline3D = new CubicSpline3D(_levelMap.CameraPositions);
        }
        if (keyboard.IsKeyClicked(Keys.Space) && !isShifted)
        {
            _camera.Reset();
        }

        if (keyboard.IsKeyDown(Keys.Up) && !isShifted)
        {
            _camera.PanY(.07f);
        }
        if (keyboard.IsKeyDown(Keys.Down) && !isShifted)
        {
            _camera.PanY(-.07f);
        }
        if (keyboard.IsKeyDown(Keys.Right) && !isShifted)
        {
            _camera.PanX(.1f);
        }
        if (keyboard.IsKeyDown(Keys.Left) && !isShifted)
        {
            _camera.PanX(-.1f);
        }

        _camera.UpdateMatrices();

        //float t = (float)gameTime.TotalGameTime.TotalSeconds;// % _levelMap.CameraPositions.Length;
        Vector3 camPos = _spline3D.Eval3D(_camera.Y);//_levelMap.CameraPositions[t];
        _camera.PanTo(camPos);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        foreach (KeyValuePair<HexCoord, LevelNode> entry in _levelMap.Nodes)
        {
            HexCoord coord = entry.Key;
            LevelNode node = entry.Value;

            Matrix translation = Matrix.CreateScale(0.7f) * Matrix.CreateTranslation(_levelMap.GetWorldPosition(coord));
            DrawModel(_prism, translation, _camera.View, _camera.Proj);
        }

        base.Draw(gameTime);
    }

    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
    {
        foreach(ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.FogEnabled = true;
                effect.FogColor = Color.Black.ToVector3(); // For best results, make this color whatever your background is.
                effect.FogStart = 0f;
                effect.FogEnd = 20f;

                effect.EnableDefaultLighting();
                effect.EmissiveColor = Color.Black.ToVector3();

                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }
}
