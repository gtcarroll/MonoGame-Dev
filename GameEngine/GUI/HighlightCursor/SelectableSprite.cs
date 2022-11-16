using System;
using System.Threading.Channels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Combat;

namespace EverythingUnder.GUI
{
    public class SelectableSprite
    {
        #region Properties

        private GameManager _game;

        private Texture2D _sprite;

        //private Vector2 _position;
        //private Vector2 _dimensions;

        private Rectangle _destination;
        public Point Position;
        public Point Dimension;

        private bool _isHighlighted;

        public event Action Selected;

        #endregion

        #region Constructor

        public SelectableSprite(GameManager game, Point position)
        {
            _game = game;

            Position = position;
            Dimension = new Point(126, 126);
        }
        public SelectableSprite(Texture2D sprite, Point position, Point dimensions)
        {
            _sprite = sprite;
            Position = position;
            Dimension = dimensions;

            _isHighlighted = false;
        }

        #endregion

        #region Loading Methods

        //Todo: remove these

        public void LoadContent()
        {
            _sprite = _game.Content.Load<Texture2D>("Textures/simple-squashed-hex");
        }

        public void UnloadContent()
        {
            _game.Content.UnloadAsset("Textures/simple-hex");
        }

        #endregion

        #region State Methods

        public void Select()
        {
            Selected?.Invoke();
        }

         public void DrawSprite(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_sprite, _position, Color.White);

            Rectangle dest = new Rectangle(Position, Dimension);
            Console.WriteLine(Color.White + " + " + _sprite + ": " + dest);
            spriteBatch.Draw(_sprite, dest, Color.White);
        }

        public void DrawHighlight(SpriteBatch spriteBatch, int thickness = 10)
        {
            //spriteBatch.Draw(_sprite, _position, null, Color.CornflowerBlue, 0f,
            //    Vector2.Zero, 1.1f, SpriteEffects.None, 0f);

            Point highlightPos = Position - new Point(thickness);
            Point highlightDim = Dimension + new Point(thickness * 2);
            Rectangle dest = new Rectangle(highlightPos, highlightDim);

            spriteBatch.Draw(_sprite, dest, Color.CornflowerBlue);
        }

        public HighlightSprite GetHighlight(int thickness = 10)
        {
            return new HighlightSprite(
                _sprite,
                Dimension + new Point(thickness * 2));
        }

        #endregion
    }
}

