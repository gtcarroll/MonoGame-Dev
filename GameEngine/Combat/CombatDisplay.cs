using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Characters;
using EverythingUnder.ScreenManagement;
using EverythingUnder.Screens;
using EverythingUnder.GUI;

namespace EverythingUnder.Combat
{
    public class CombatDisplay
    {
        #region Properties

        private GameManager _game;

        private SpriteBatch _spriteBatch;

        private List<SelectableSprite> _selections;

        private List<SpriteGroup> _sprites;

        private SpriteGroup _prev;
        private SpriteGroup _hovered;


        private GUIGarden _guiGarden;

        private HighlightCursor _cursor;
        private int _cursorIndex;

        #endregion

        #region Constructors

        public CombatDisplay(GameManager game, List<Character> friends,
                                                List<Character> enemies)
        {
            _game = game;

            _guiGarden = new CombatGarden(game);

            //_selections = new List<SelectableSprite>();
            //_selections.Add(new SelectableSprite(_game, new Point(100, 300)));
            //_selections.Add(new SelectableSprite(_game, new Point(350, 50)));
            //_selections.Add(new SelectableSprite(_game, new Point(600, 300)));
            //_selections.Add(new SelectableSprite(_game, new Point(450, 200)));

            //_sprites = new List<SpriteGroup>();
            //_sprites.Add(new CardSprite(new Point(100, 300)));
            //_sprites.Add(new CardSprite(new Point(175, 200)));
            //_sprites.Add(new CardSprite(new Point(250, 300)));
            //_sprites.Add(new CardSprite(new Point(325, 200)));
            //_sprites.Add(new CardSprite(new Point(400, 300)));
            //_sprites.Add(new CardSprite(new Point(475, 200)));
            //_sprites.Add(new CardSprite(new Point(530, 290)));
            //_sprites.Add(new CardSprite(new Point(585, 200)));


            //_cursor = new HighlightCursor(game);
            //_cursorIndex = -1;
        }

        #endregion

        #region Loading Methods

        public void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            //Texture2D hex = _game.Content.Load<Texture2D>("Textures/simple-hex");
            //foreach (SelectableSprite sprite in _selections)
            //{
            //    sprite.LoadContent();

            //    // debug only
            //}
            //foreach (SpriteGroup spriteGroup in _sprites)
            //{
            //    spriteGroup.LoadContent(_game.Content);
            //}

            //SetHovered();
        }

        #endregion

        private void SetHovered()
        {
            if (_hovered != null)
            {
                _hovered.Style = SpriteStyle.Default;
                _prev = _hovered;
            }

            _cursorIndex = (_cursorIndex + 1) % _sprites.Count;

            _hovered = _sprites[_cursorIndex];
            _hovered.Style = SpriteStyle.Hover;
            _cursor.AnimateTo(_hovered);
        }

        #region Rendering Methods

        public void HandleInput(InputState input)
        {
            if (input.WasSelectPressed())
            {
                //SetHovered();
            }
        }

        public void Update(GameTime time)
        {
            //_cursor.Update(time);

            //foreach (SpriteGroup spriteGroup in _sprites)
            //{
            //    spriteGroup.Update(time);
            //}
            _guiGarden.Update(time);
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    _cursor.Draw(spriteBatch);
        //}

        public void DrawAllHighlighted()
        {
            _spriteBatch.Begin();

            _guiGarden.DrawBG(_spriteBatch);

            _guiGarden.DrawFG(_spriteBatch);

            _spriteBatch.End();

            //_spriteBatch.Begin();

            //foreach (SpriteGroup spriteGroup in _sprites)
            //{
            //    spriteGroup.Draw(_spriteBatch);
            //}

            //_cursor.Draw(_spriteBatch);

            //if (_prev != null && _cursor.IsAnimating)
            //{
            //    _prev.Draw(_spriteBatch);
            //}

            //_hovered.Draw(_spriteBatch);

            //_spriteBatch.End();
        }

        #endregion

        #region State Methods

        #endregion

        #region Helper Methods

        #endregion
    }
}

