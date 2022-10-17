using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HexMap.Graphics;
using HexMap.Input;
using HexMap.HexMap;
using System.Reflection;

namespace HexMap;

public class Game1 : Game
{
    private bool _renderRGB = false;
    private bool _isPaused = false;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private PolygonBatch _polygons;
    private Camera _camera;

    private HexGrid _hexGrid;

    private Model _prism;

    private Polygon2D _polygon;
    private int _sides;
    private bool _flipped;

    private Polygon2D _hexagon;
    private Polygon2D _irregular;

    private Stopwatch _stopwatch;

    private float _angle;
    private Matrix _transform;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Viewport vp = GraphicsDevice.Viewport;

        _polygons = new PolygonBatch(this);
        _camera = new Camera(this);

        _stopwatch = new Stopwatch();

        _hexGrid = new HexGrid(62, 62); //new HexGrid(31, 31);

        _hexagon = new Polygon2D(6);

        Vector2[] vertices = new Vector2[5];
        vertices[0] = new Vector2(-3,0);
        vertices[1] = new Vector2(0, 3);
        vertices[2] = new Vector2(3, 0);
        vertices[3] = new Vector2(0, -3);
        vertices[4] = new Vector2(-1, -1);

        _irregular = new Polygon2D(vertices);

        _angle = -MathHelper.PiOver2 - 1;
        _transform = Matrix.Identity;

        _sides = 3;
        _polygon = new Polygon2D(_sides, 10f);
        _flipped = false;

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

        if (keyboard.IsKeyClicked(Keys.OemTilde))
        {
            Console.WriteLine(_stopwatch.Elapsed.TotalMilliseconds);
        }

        if (keyboard.IsKeyClicked(Keys.F))
        {
            Util.ToggleFullScreen(_graphics);
        }

        if (keyboard.IsKeyDown(Keys.Q))
        {
            _camera.MoveZ(-6);
        }
        if (keyboard.IsKeyDown(Keys.E))
        {
            _camera.MoveZ(6);
        }
        if (keyboard.IsKeyDown(Keys.W))
        {
            _camera.TiltX(-MathHelper.PiOver4 / 60);
        }
        if (keyboard.IsKeyDown(Keys.S))
        {
            _camera.TiltX(MathHelper.PiOver4 / 60);
        }
        if (keyboard.IsKeyDown(Keys.A))
        {
            _camera.TiltY(-MathHelper.PiOver4 / 60);
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            _camera.TiltY(MathHelper.PiOver4 / 60);
        }

        bool isShifted = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);

        if (keyboard.IsKeyClicked(Keys.Space) && isShifted)
        {
            _camera.Reset();
        }

        if (keyboard.IsKeyDown(Keys.Up) && !isShifted)
        {
            _camera.PanY(-6);
        }
        if (keyboard.IsKeyDown(Keys.Down) && !isShifted)
        {
            _camera.PanY(6);
        }
        if (keyboard.IsKeyDown(Keys.Right) && !isShifted)
        {
            _camera.PanX(-6);
        }
        if (keyboard.IsKeyDown(Keys.Left) && !isShifted)
        {
            _camera.PanX(6);
        }

        if (keyboard.IsKeyClicked(Keys.Up) && isShifted)
        {
            _hexGrid.Stretch++;
        }
        if (keyboard.IsKeyClicked(Keys.Down) && isShifted)
        {
            _hexGrid.Stretch--;
        }
        if (keyboard.IsKeyClicked(Keys.Right) && isShifted)
        {
            _renderRGB = !_renderRGB;
        }
        if (keyboard.IsKeyClicked(Keys.Left) && isShifted)
        {
            _hexGrid.Normalize = !_hexGrid.Normalize;
        }
        if (keyboard.IsKeyClicked(Keys.Space) && !isShifted)
        {
            _isPaused = !_isPaused;
        }

        _angle += MathHelper.PiOver2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_angle > MathHelper.TwoPi)
        {
            _angle = _angle % MathHelper.TwoPi;
            _flipped = false;
        }
        if (!_flipped && _angle > 3 * MathHelper.PiOver2)
        {
            _sides++;
            _flipped = true;
            _polygon = new Polygon2D(_sides, 10f);
        }

        _transform = Matrix.CreateScale(10f)
            * Matrix.CreateRotationY(_angle);

        //_hexagon.Transform(_transform);

        if (!_isPaused)
        {
            _hexGrid.Update();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _polygons.Begin(_camera);

        //float angle = _angle % MathHelper.TwoPi;
        //if (angle < MathHelper.PiOver2 || angle > 3 * MathHelper.PiOver2)
        //{
        //    _polygons.DrawPolygonFill(_polygon, _transform, Color.Aquamarine);
        //}
        //else
        //{
        //    _polygons.DrawPolygonFill(_polygon, _transform, Color.Aquamarine);
        //    _polygons.DrawPolygonTriangles(_polygon, _transform);
        //}

        //DrawModel(_prism, Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Matrix.CreateScale(10f), _camera.View, _camera.Proj);

        Matrix scale = Matrix.CreateScale(10f) * Matrix.CreateRotationZ(MathHelper.Pi / 6f);
        for (int r = 0; r < _hexGrid.Rows; r++)
        {
            for (int c = 0; c < _hexGrid.Cols; c++)
            {
                float value = (float)_hexGrid.GetTile(r, c).Noise;
                Matrix translation = scale * Matrix.CreateTranslation(new Vector3(_hexGrid.GetTranslation(r, c), value * 30f));
                if (_renderRGB)
                {
                    //_polygons.DrawPolygonFill(_hexGrid.Hexagon, translation, _hexGrid.GetTile(r, c).Color);
                    DrawModelColor(_prism, translation, _camera.View, _camera.Proj, r, c);
                }
                else
                {
                    //float value = (float)_hexGrid.GetTile(r, c).Noise;
                    //Color color = new Color(value, value, value);
                    //_polygons.DrawPolygonFill(_hexGrid.Hexagon, translation, color);

                    DrawModel(_prism, translation, _camera.View, _camera.Proj);
                }
            }
        }

        _polygons.End();

        base.Draw(gameTime);
    }

    private void DrawModelColor(Model model, Matrix world, Matrix view, Matrix projection, int r, int c)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                Color color = _hexGrid.GetTile(r, c).Color;
                effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                effect.EmissiveColor = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);

                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }

    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();

                effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                effect.EmissiveColor = new Vector3(1f, 0f, 0f);

                effect.World = world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }
}
