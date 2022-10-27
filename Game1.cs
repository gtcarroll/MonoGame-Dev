using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HexMap.Graphics;
using HexMap.Input;
using System.Reflection;
using System.Collections.Generic;

namespace HexMap;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Camera _camera;

    //private HexMap _hexMap;
    private LevelMap _levelMap;

    //private LevelGenerator _levelGen;

    private Model _prism;

    private float _angle;
    private Matrix _transform;
    private Random _random;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Viewport vp = GraphicsDevice.Viewport;

        _camera = new Camera(this);

        _random = new Random();

        _levelMap = new LevelMap(_random);

        //_levelGen = new LevelGenerator(13);
        //_levelGen.WriteLevel(_hexMap);

        _angle = -MathHelper.PiOver2 - 1;
        _transform = Matrix.Identity;

        //_camera.Pan(_hexGrid.GetTranslation(0, 0 - 10));
        //_camera.PanTo(_hexGrid.GetTranslation(0, 62 - 10), 3100);

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
            Util.ToggleFullScreen(_graphics);
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
            _levelMap = new LevelMap(_random);

            //_levelGen = new LevelGenerator(13);
            //_levelGen.WriteLevel(_hexMap);
        }
        if (keyboard.IsKeyClicked(Keys.Space) && !isShifted)
        {
            _camera.Reset();
        }

        if (keyboard.IsKeyDown(Keys.Up) && !isShifted)
        {
            _camera.PanY(.1f);
        }
        if (keyboard.IsKeyDown(Keys.Down) && !isShifted)
        {
            _camera.PanY(-.1f);
        }
        if (keyboard.IsKeyDown(Keys.Right) && !isShifted)
        {
            _camera.PanX(.1f);
        }
        if (keyboard.IsKeyDown(Keys.Left) && !isShifted)
        {
            _camera.PanX(-.1f);
        }

        //if (keyboard.IsKeyClicked(Keys.Up) && isShifted)
        //{
        //    _hexGrid.Stretch++;
        //}
        //if (keyboard.IsKeyClicked(Keys.Down) && isShifted)
        //{
        //    _hexGrid.Stretch--;
        //}
        //if (keyboard.IsKeyClicked(Keys.Right) && isShifted)
        //{
        //    _renderRGB = !_renderRGB;
        //}
        //if (keyboard.IsKeyClicked(Keys.Left) && isShifted)
        //{
        //    _hexGrid.Normalize = !_hexGrid.Normalize;
        //}

        _angle += MathHelper.PiOver2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        _transform = Matrix.CreateRotationY(_angle);

        //_camera.Update(gameTime);
        _camera.UpdateMatrices();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        Matrix rotate = Matrix.CreateRotationZ(MathHelper.Pi / 6f);

        //foreach (KeyValuePair<HexCoord, HexTile> entry in _hexMap.Tiles)
        foreach (KeyValuePair<HexCoord, LevelNode> entry in _levelMap.Nodes)
        {
            HexCoord coord = entry.Key;
            //HexTile tile = entry.Value;
            LevelNode node = entry.Value;

            //float height = (float)tile.Height;
            //Matrix translation = Matrix.CreateScale(8f) * Matrix.CreateTranslation(_levelMap.GetWorldPosition(coord));
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
                effect.EmissiveColor = Color.Black.ToVector3(); //tile.Color.ToVector3();

                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }
    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, HexTile tile)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.FogEnabled = true;
                effect.FogColor = Color.Black.ToVector3(); // For best results, make this color whatever your background is.
                effect.FogStart = 0f;
                effect.FogEnd = 200f;

                effect.EnableDefaultLighting();
                effect.EmissiveColor = tile.Color.ToVector3();

                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }
}
