//using System;
//using System.Xml.Linq;
//using EverythingUnder.Levels;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using MonoGame.Extended.Input;
//using MonoGame.Extended.Screens;

//namespace EverythingUnder.Screens
//{
//    public class EventScreen : GameScreen
//    {
//        private Game _game;

//        public EventScreen(Game game) : base(game)
//        {
//            _game = game;
//        }

//        public override void LoadContent()
//        {
//            base.LoadContent();
//        }

//        public override void Update(GameTime gameTime)
//        {
//            //throw new NotImplementedException();
//            //KeyboardStateExtended keyboard = KeyboardExtended.GetState();

//            // re-generate level
//            if (Keyboard.GetState().IsKeyDown(Keys.E))
//            {
//                UnloadContent();
//                Dispose();
//            }
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            //throw new NotImplementedException();
//            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

//        }
//    }
//}

