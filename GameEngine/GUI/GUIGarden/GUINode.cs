using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EverythingUnder.Combat;

namespace EverythingUnder.GUI
{
    public enum InputDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public abstract class GUINode
    {
        #region Properties

        // parent objects
        //public GUIGarden Garden;
        //public GUIPlot Plot;

        // connected nodes
        public Dictionary<InputDirection, GUINode> Neighbors;

        // node parameters
        public Action OnSelected;

        // node state
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        //private bool _isHovered;
        public bool IsHovered
        {
            get
            {
                if (Sprite != null) return Sprite.IsHovered;
                else return false;
            }
            set { if (Sprite != null) Sprite.IsHovered = value; }
        }

        // node graphics
        public SpriteGroup Sprite;
        public Point Center;
        public Rectangle ScreenSpace;

        // other sprites
        public List<SpriteGroup> FrontSprites;
        public List<SpriteGroup> BackSprites;

        #endregion

        #region Constructors

        public GUINode(Point center)
        {
            //Garden = plot.Garden;
            //Plot = plot;

            Neighbors = new Dictionary<InputDirection, GUINode>();
            //_isHovered = false;
            _isActive = false;

            FrontSprites = new List<SpriteGroup>();
            BackSprites = new List<SpriteGroup>();

            Sprite = null;
            Center = center;
        }

        #endregion

        #region Loading Methods

        public virtual void LoadContent(GameManager game)
        {
            if (Sprite != null)
            {
                Sprite.LoadContent(game.Content);
            }
        }

        public virtual void UnloadContent(GameManager game) { }

        public void AddSprite(SpriteGroup sprite)
        {
            Sprite = sprite;
            _isActive = true;
        }
        public void RemoveSprite()
        {
            // TODO: check for null when updating hover state etc.
            //Garden.RemoveSprite(Sprite);

            Sprite = null;
            _isActive = false;
        }

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime time)
        {
            RemoveNonAnimatingSprites(BackSprites);
            RemoveNonAnimatingSprites(FrontSprites);

            foreach (SpriteGroup backSprite in BackSprites)
            {
                backSprite.Update(time);
            }

            if (Sprite != null)
            {
                Sprite.Update(time);
            }

            foreach (SpriteGroup fronSprite in FrontSprites)
            {
                fronSprite.Update(time);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (SpriteGroup backSprite in BackSprites)
            {
                backSprite.Draw(spriteBatch);
            }

            if (Sprite != null)
            {
                Sprite.Draw(spriteBatch);
            }

            foreach (SpriteGroup fronSprite in FrontSprites)
            {
                fronSprite.Draw(spriteBatch);
            }
        }

        #endregion

        #region State Methods

        private void RemoveNonAnimatingSprites(List<SpriteGroup> sprites)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                SpriteGroupAnimation anim = sprites[i].Animation;

                if (anim != null && anim.IsStarted && anim.IsCompleted)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool Contains(Point mousePos)
        {
            return ScreenSpace.Contains(mousePos);
        }

        public void SetNeighbor(InputDirection direction, GUINode node)
        {
            if (Neighbors.ContainsKey(direction))
            {
                Neighbors[direction] = node;
            }
            else
            {
                Neighbors.Add(direction, node);
            }
        }

        public void RemoveNeighbor(InputDirection direction)
        {
            Neighbors.Remove(direction);
        }

        public GUINode GetNeighbor(InputDirection direction)
        {
            if (Neighbors.ContainsKey(direction))
            {
                GUINode neighbor = Neighbors[direction];
                if (neighbor.IsActive)
                {
                    return neighbor;
                } else
                {
                    return neighbor.GetNeighbor(direction);
                }
            }

            return null;
        }

        #endregion
    }
}

