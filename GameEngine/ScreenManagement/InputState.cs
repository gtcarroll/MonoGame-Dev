using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace EverythingUnder.ScreenManagement
{
    public enum MouseButtons
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
            CurrGamePadStates = new GamePadState[MaxPlayerCount];
            PrevGamePadStates = new GamePadState[MaxPlayerCount];
            IsGamePadConnected = new bool[MaxPlayerCount];

            Gestures = new List<GestureSample>();
        }

        #endregion

        #region Game Cycle Methods

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

        public bool IsUpPressed(int player = -1)
        {
            return IsPressed(Keys.W)
                || IsPressed(Keys.Up)
                || IsPressed(Buttons.DPadUp, player)
                || IsPressed(Buttons.LeftThumbstickUp, player)
                || IsPressed(Buttons.RightThumbstickUp, player);
        }
        public bool WasUpPressed(int player = -1)
        {
            return WasPressed(Keys.W)
                || WasPressed(Keys.Up)
                || WasPressed(Buttons.DPadUp, player)
                || WasPressed(Buttons.LeftThumbstickUp, player)
                || WasPressed(Buttons.RightThumbstickUp, player);
        }
        public bool WasUpReleased(int player = -1)
        {
            return WasReleased(Keys.W)
                || WasReleased(Keys.Up)
                || WasReleased(Buttons.DPadUp, player)
                || WasReleased(Buttons.LeftThumbstickUp, player)
                || WasReleased(Buttons.RightThumbstickUp, player);
        }

        public bool IsDownPressed(int player = -1)
        {
            return IsPressed(Keys.S)
                || IsPressed(Keys.Down)
                || IsPressed(Buttons.DPadDown, player)
                || IsPressed(Buttons.LeftThumbstickDown, player)
                || IsPressed(Buttons.RightThumbstickDown, player);
        }
        public bool WasDownPressed(int player = -1)
        {
            return WasPressed(Keys.S)
                || WasPressed(Keys.Down)
                || WasPressed(Buttons.DPadDown, player)
                || WasPressed(Buttons.LeftThumbstickDown, player)
                || WasPressed(Buttons.RightThumbstickDown, player);
        }
        public bool WasDownReleased(int player = -1)
        {
            return WasReleased(Keys.S)
                || WasReleased(Keys.Down)
                || WasReleased(Buttons.DPadDown, player)
                || WasReleased(Buttons.LeftThumbstickDown, player)
                || WasReleased(Buttons.RightThumbstickDown, player);
        }

        public bool IsLeftPressed(int player = -1)
        {
            return IsPressed(Keys.A)
                || IsPressed(Keys.Left)
                || IsPressed(Buttons.DPadLeft, player)
                || IsPressed(Buttons.LeftThumbstickLeft, player)
                || IsPressed(Buttons.RightThumbstickLeft, player);
        }
        public bool WasLeftPressed(int player = -1)
        {
            return WasPressed(Keys.A)
                || WasPressed(Keys.Left)
                || WasPressed(Buttons.DPadLeft, player)
                || WasPressed(Buttons.LeftThumbstickLeft, player)
                || WasPressed(Buttons.RightThumbstickLeft, player);
        }
        public bool WasLeftReleased(int player = -1)
        {
            return WasReleased(Keys.A)
                || WasReleased(Keys.Left)
                || WasReleased(Buttons.DPadLeft, player)
                || WasReleased(Buttons.LeftThumbstickLeft, player)
                || WasReleased(Buttons.RightThumbstickLeft, player);
        }

        public bool IsRightPressed(int player = -1)
        {
            return IsPressed(Keys.D)
                || IsPressed(Keys.Right)
                || IsPressed(Buttons.DPadRight, player)
                || IsPressed(Buttons.LeftThumbstickRight, player)
                || IsPressed(Buttons.RightThumbstickRight, player);
        }
        public bool WasRightPressed(int player = -1)
        {
            return WasPressed(Keys.D)
                || WasPressed(Keys.Right)
                || WasPressed(Buttons.DPadRight, player)
                || WasPressed(Buttons.LeftThumbstickRight, player)
                || WasPressed(Buttons.RightThumbstickRight, player);
        }
        public bool WasRightReleased(int player = -1)
        {
            return WasReleased(Keys.D)
                || WasReleased(Keys.Right)
                || WasReleased(Buttons.DPadRight, player)
                || WasReleased(Buttons.LeftThumbstickRight, player)
                || WasReleased(Buttons.RightThumbstickRight, player);
        }

        public bool IsSelectPressed(int player = -1)
        {
            return IsPressed(Keys.Space)
                || IsPressed(Keys.Enter)
                || IsPressed(Buttons.A, player)
                || IsPressed(Buttons.RightTrigger, player);
        }
        public bool WasSelectPressed(int player = -1)
        {
            return WasPressed(Keys.Space)
                || WasPressed(Keys.Enter)
                || WasPressed(Buttons.A, player)
                || WasPressed(Buttons.RightTrigger, player);
        }
        public bool WasSelectReleased(int player = -1)
        {
            return WasReleased(Keys.Space)
                || WasReleased(Keys.Enter)
                || WasReleased(Buttons.A, player)
                || WasReleased(Buttons.RightTrigger, player);
        }

        public bool IsCancelPressed(int player = -1)
        {
            return IsPressed(Keys.Back)
                || IsPressed(Keys.Escape)
                || IsPressed(Buttons.B, player)
                || IsPressed(Buttons.LeftTrigger, player);
        }
        public bool WasCancelPressed(int player = -1)
        {
            return WasPressed(Keys.Back)
                || WasPressed(Keys.Escape)
                || WasPressed(Buttons.B, player)
                || WasPressed(Buttons.LeftTrigger, player);
        }
        public bool WasCancelReleased(int player = -1)
        {
            return WasReleased(Keys.Back)
                || WasReleased(Keys.Escape)
                || WasReleased(Buttons.B, player)
                || WasReleased(Buttons.LeftTrigger, player);
        }

        public bool IsPausePressed(int player = -1)
        {
            return IsPressed(Keys.P)
                || IsPressed(Buttons.Start, player);
        }
        public bool WasPausePressed(int player = -1)
        {
            return WasPressed(Keys.P)
                || WasPressed(Buttons.Start, player);
        }
        public bool WasPauseReleased(int player = -1)
        {
            return WasReleased(Keys.P)
                || WasReleased(Buttons.Start, player);
        }

        #endregion

        #region Mouse Input Methods

        public Vector2 GetMousePosition()
        {
            return new Vector2(CurrMouseState.X, CurrMouseState.Y);
        }

        public bool IsPressed(MouseButtons button)
        {
            return IsPressed(CurrMouseState, button);
        }
        public bool WasPressed(MouseButtons button)
        {
            return IsPressed(CurrMouseState, button)
                && !IsPressed(PrevMouseState, button);
        }
        public bool WasReleased(MouseButtons button)
        {
            return !IsPressed(CurrMouseState, button)
                && IsPressed(PrevMouseState, button);
        }

        private bool IsPressed(MouseState state, MouseButtons button)
        {
            return button switch
            {
                MouseButtons.Left =>
                    state.LeftButton == ButtonState.Pressed,
                MouseButtons.Middle =>
                    state.MiddleButton == ButtonState.Pressed,
                MouseButtons.Right =>
                    state.RightButton == ButtonState.Pressed,
                MouseButtons.X1 =>
                    state.XButton1 == ButtonState.Pressed,
                MouseButtons.X2 =>
                    state.XButton2 == ButtonState.Pressed,
                _ => false,
            };
        }

        #endregion

        #region Keyboard Input Methods

        public bool IsPressed(Keys key)
        {
            return CurrKeyboardState.IsKeyDown(key);
        }
        public bool WasPressed(Keys key)
        {
            return CurrKeyboardState.IsKeyDown(key)
                && !PrevKeyboardState.IsKeyDown(key);
        }
        public bool WasReleased(Keys key)
        {
            return !CurrKeyboardState.IsKeyDown(key)
                && PrevKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region GamePad Input Methods

        public bool IsPressed(Buttons button, int player = -1)
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
        public bool WasPressed(Buttons button, int player = -1)
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
        public bool WasReleased(Buttons button, int player = -1)
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

        #endregion
    }
}

