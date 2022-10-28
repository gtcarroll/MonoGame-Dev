using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EverythingUnder.Input
{
    public sealed class FlatKeyboard
    {
        private static readonly Lazy<FlatKeyboard> Lazy = new Lazy<FlatKeyboard>(() => new FlatKeyboard());

        public static FlatKeyboard Instance
        {
            get { return Lazy.Value; }
        }

        private KeyboardState _prevState;
        private KeyboardState _currState;

        public FlatKeyboard()
        {
            _prevState = Keyboard.GetState();
            _currState = _prevState;
        }

        public void Update()
        {
            _prevState = _currState;
            _currState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys keys)
        {
            return _currState.IsKeyDown(keys);
        }

        public bool IsKeyClicked(Keys keys)
        {
            return _currState.IsKeyDown(keys) && !_prevState.IsKeyDown(keys);
        }
    }
}

