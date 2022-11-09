using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverythingUnder.Characters;
using EverythingUnder.ScreenManagement;
using EverythingUnder.Screens;

namespace EverythingUnder.Combat
{
    public class CombatDisplay
    {
        #region Properties

        private GameManager _game;

        private SpriteBatch _spriteBatch;

        private List<SelectableSprite> _selections;


        private HighlightCursor _cursor;
        private int _cursorIndex;

        #endregion

        #region Constructors

        public CombatDisplay(GameManager game, List<Character> friends,
                                                List<Character> enemies)
        {
            _game = game;

            _selections = new List<SelectableSprite>();
            _selections.Add(new SelectableSprite(_game, new Point(100, 300)));
            _selections.Add(new SelectableSprite(_game, new Point(350, 50)));
            _selections.Add(new SelectableSprite(_game, new Point(600, 300)));

            _cursor = new HighlightCursor(game);
            _cursorIndex = 0;
        }

        #endregion

        #region Loading Methods

        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            Texture2D hex = _game.Content.Load<Texture2D>("simple-hex");
            foreach (SelectableSprite sprite in _selections)
            {
                sprite.LoadContent();

                // debug only
            }
        }

        #endregion

        #region Rendering Methods

        public void HandleInput(InputState input)
        {
            if (input.WasSelectPressed())
            {
                _cursor.AnimateTo(_selections[_cursorIndex]);
                _cursorIndex = (_cursorIndex + 1) % _selections.Count;
            }
        }

        public void Update(GameTime time)
        {
            _cursor.Update(time);
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    _cursor.Draw(spriteBatch);
        //}

        public void DrawAllHighlighted()
        {
            _spriteBatch.Begin();


            _cursor.Draw(_spriteBatch);

            foreach (SelectableSprite sprite in _selections)
            {
                //sprite.DrawHighlight(_spriteBatch);
                sprite.DrawSprite(_spriteBatch);
            }

            _spriteBatch.End();
        }

        #endregion

        #region State Methods

        #endregion

        #region Helper Methods

        #endregion
    }
}

