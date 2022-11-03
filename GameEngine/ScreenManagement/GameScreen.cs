using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace EverythingUnder.ScreenManagement
{
    public abstract class GameScreen
    {
        #region Properties

        public bool IsPopUp { get; set; }
        public bool IsClosing { get; set; }
        public bool IsCovered { get; set; }
        public bool IsFocused { get; set; }

        public TimeSpan TransitionTime { get; set; }
        public float TransitionProgress { get; set; }

        public int ControllingPlayer { get; set; }

        public GestureType EnabledGestures { get; set; }

        #endregion

        #region Constructors

        public GameScreen(bool isPopUp = false, int controllingPlayer = -1)
        {
            IsPopUp = isPopUp;
            IsClosing = false;
            IsCovered = false;
            IsFocused = true;

            TransitionTime = TimeSpan.Zero;
            TransitionProgress = 0f;

            ControllingPlayer = controllingPlayer;

            EnabledGestures = GestureType.None;
        }

        #endregion

        #region Loading Methods

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime gameTime, bool isFocused,
                                                      bool isCovered)
        {
            IsFocused = isFocused;
            IsCovered = isCovered;

            UpdateTransition(gameTime);
        }

        private bool UpdateTransition(GameTime gameTime)
        {
            float transitionDelta = 1f;

            if (TransitionTime != TimeSpan.Zero)
            {
                transitionDelta = (float)
                    (gameTime.ElapsedGameTime.TotalMilliseconds
                     / TransitionTime.TotalMilliseconds);
            }

            int multiple = IsClosing ? -1 : 1;
            TransitionProgress += transitionDelta * multiple;

            bool isTransitionComplete =
                (IsClosing && TransitionProgress <= 0f)
                || (!IsClosing && TransitionProgress >= 1f);

            MathHelper.Clamp(TransitionProgress, 0f, 1f);

            return isTransitionComplete;
        }

        public virtual void HandleInput(InputState input) { }

        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region State Methods

        public void CloseScreen()
        {
            IsClosing = true;
        }

        #endregion
    }
}

