using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexMap.Graphics
{
    public class TimingFunction
    {
        // sin
        //   ease-in
        //   ease-out
        //   ease-in-out

        // spring
        //   ease-in
        //   ease-out
        //   ease-in-out
    }

    // add properties, each w different timing functions
    // start animation w given duration
    // update to retrieve property values
    public class Animation
    {
        private bool _isAnimating;

        private float _property;

        public Animation()
        {

        }

        public void AddProperty<T>(T property, TimingFunction timingFunction)
        {
           
        }

        public void Start(float msDuration)
        {

        }
        public void End()
        {

        }

        public void Update(GameTime gameTime)
        {

        }
    }
}

