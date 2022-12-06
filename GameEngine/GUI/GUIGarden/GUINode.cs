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

        private bool _isHovered;
        public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                _isHovered = value;
                if (_isActive && Sprite != null)
                {
                    //Sprite.IsHovered = value;
                    Sprite.Style = _isHovered ? SpriteStyle.Hover
                                              : SpriteStyle.Default;
                }
            }
        }

        // node graphics
        public SpriteGroup Sprite;
        public Point Center;
        public Rectangle ScreenSpace;

        #endregion

        #region Constructors

        public GUINode(Point center)
        {
            Neighbors = new Dictionary<InputDirection, GUINode>();
            _isHovered = false;
            _isActive = false;

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
            Sprite = null;
            _isActive = false;
        }

        #endregion

        #region Rendering Methods

        public virtual void Update(GameTime time)
        {
            if (Sprite != null)
            {
                Sprite.Update(time);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                Sprite.Draw(spriteBatch);
            }
        }

        #endregion

        #region State Methods

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

