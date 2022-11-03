using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace EverythingUnder.ScreenManagement
{
    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        X1,
        X2
    }

    public class InputState
    {
        #region Properties

        public const int MaxPlayerCount = 2;

        public MouseState CurrMouseState;
        public MouseState PrevMouseState;

        public KeyboardState CurrKeyboardState;
        public KeyboardState PrevKeyboardState;

        public readonly GamePadState[] CurrGamePadStates;
        public readonly GamePadState[] PrevGamePadStates;
        public readonly bool[] IsGamePadConnected;

        public TouchCollection TouchState;
        public readonly List<GestureSample> Gestures;

        #endregion

        #region Constructors

        public InputState()
        {
            //CurrMouseState = Mouse.GetState();
            //PrevMouseState = CurrMouseState;

            //CurrKeyboardState = Keyboard.GetState();
            //PrevKeyboardState = CurrKeyboardState;

            //TouchState = TouchPanel.GetState();

            CurrGamePadStates = new GamePadState[MaxPlayerCount];
            PrevGamePadStates = new GamePadState[MaxPlayerCount];
            IsGamePadConnected = new bool[MaxPlayerCount];

            Gestures = new List<GestureSample>();
        }

        #endregion

        #region Rendering Methods

        public void Update()
        {
            // update keyboard and mouse states
            PrevMouseState = CurrMouseState;
            CurrMouseState = Mouse.GetState();

            PrevKeyboardState = CurrKeyboardState;
            CurrKeyboardState = Keyboard.GetState();

            // update all gamepad states
            for (int i = 0; i < MaxPlayerCount; i++)
            {
                PrevGamePadStates[i] = CurrGamePadStates[i];
                CurrGamePadStates[i] = GamePad.GetState(i);
                IsGamePadConnected[i] = CurrGamePadStates[i].IsConnected;
            }

            // update touch state
            TouchState = TouchPanel.GetState();

            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }
        }

        #endregion

        #region General Input Methods

        public bool IsUp(int player = -1)
        {
            return WasKeyJustDown(Keys.W)
                || WasKeyJustDown(Keys.Up)
                || WasButtonJustDown(Buttons.DPadUp, player)
                || WasButtonJustDown(Buttons.LeftThumbstickUp, player)
                || WasButtonJustDown(Buttons.RightThumbstickUp, player);
        }
        public bool IsDown(int player = -1)
        {
            return WasKeyJustDown(Keys.S)
                || WasKeyJustDown(Keys.Down)
                || WasButtonJustDown(Buttons.DPadDown, player)
                || WasButtonJustDown(Buttons.LeftThumbstickDown, player)
                || WasButtonJustDown(Buttons.RightThumbstickDown, player);
        }
        public bool IsLeft(int player = -1)
        {
            return WasKeyJustDown(Keys.A)
                || WasKeyJustDown(Keys.Left)
                || WasButtonJustDown(Buttons.DPadLeft, player)
                || WasButtonJustDown(Buttons.LeftThumbstickLeft, player)
                || WasButtonJustDown(Buttons.RightThumbstickLeft, player);
        }
        public bool IsRight(int player = -1)
        {
            return WasKeyJustDown(Keys.D)
                || WasKeyJustDown(Keys.Right)
                || WasButtonJustDown(Buttons.DPadRight, player)
                || WasButtonJustDown(Buttons.LeftThumbstickRight, player)
                || WasButtonJustDown(Buttons.RightThumbstickRight, player);
        }
        public bool IsSelect(int player = -1)
        {
            return WasKeyJustDown(Keys.Space)
                || WasKeyJustDown(Keys.Enter)
                || WasButtonJustDown(Buttons.A, player)
                || WasButtonJustDown(Buttons.RightTrigger, player);
        }
        public bool IsCancel(int player = -1)
        {
            return WasKeyJustDown(Keys.Back)
                || WasKeyJustDown(Keys.Escape)
                || WasButtonJustDown(Buttons.B, player)
                || WasButtonJustDown(Buttons.LeftTrigger, player);
        }
        public bool IsPause(int player = -1)
        {
            return WasKeyJustDown(Keys.P)
                || WasButtonJustDown(Buttons.Start, player);
        }

        #endregion

        #region Mouse Input Methods

        public bool WasMouseButtonJustUp(MouseButton button)
        {
            return IsMouseButtonDown(CurrMouseState, button)
                && !IsMouseButtonDown(PrevMouseState, button);
        }
        public bool WasMouseButtonJustDown(MouseButton button)
        {
            return !IsMouseButtonDown(CurrMouseState, button)
                && IsMouseButtonDown(PrevMouseState, button);
        }
        public bool IsMouseButtonDown(MouseButton button)
        {
            return IsMouseButtonDown(CurrMouseState, button);
        }

        private bool IsMouseButtonDown(MouseState state, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left =>
                    state.LeftButton == ButtonState.Pressed,
                MouseButton.Middle =>
                    state.MiddleButton == ButtonState.Pressed,
                MouseButton.Right =>
                    state.RightButton == ButtonState.Pressed,
                MouseButton.X1 =>
                    state.XButton1 == ButtonState.Pressed,
                MouseButton.X2 =>
                    state.XButton2 == ButtonState.Pressed,
                _ => false,
            };
        }

        #endregion

        #region Keyboard Input Methods

        public bool WasKeyJustUp(Keys key)
        {
            return CurrKeyboardState.IsKeyDown(key)
                && !PrevKeyboardState.IsKeyDown(key);
        }
        public bool WasKeyJustDown(Keys key)
        {
            return !CurrKeyboardState.IsKeyDown(key)
                && PrevKeyboardState.IsKeyDown(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return CurrKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region GamePad Input Methods

        public bool WasButtonJustUp(Buttons button, int player = -1)
        {
            bool wasJustUp = false;

            for (int i = 0; i < MaxPlayerCount; i++)
            {
                if (player == i || player < 0)
                {
                    wasJustUp |= CurrGamePadStates[i].IsButtonDown(button)
                        && !PrevGamePadStates[i].IsButtonDown(button);
                }
            }

            return wasJustUp;
        }
        public bool WasButtonJustDown(Buttons button, int player = -1)
        {
            bool wasJustDown = false;

            for (int i = 0; i < MaxPlayerCount; i++)
            {
                if (player == i || player < 0)
                {
                    wasJustDown |= !CurrGamePadStates[i].IsButtonDown(button)
                        && PrevGamePadStates[i].IsButtonDown(button);
                }
            }

            return wasJustDown;
        }
        public bool IsButtonDown(Buttons button, int player = -1)
        {
            bool isDown = false;

            for (int i = 0; i < MaxPlayerCount; i++)
            {
                if (player == i || player < 0)
                {
                    isDown |= CurrGamePadStates[i].IsButtonDown(button);
                }
            }

            return isDown;
        }

        #endregion
    }
}

