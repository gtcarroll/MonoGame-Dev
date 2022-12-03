using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EverythingUnder.GUI
{
    public interface ITimingFunction
    {
        float Duration { get; }
        float AnimationPosition { get; }

        bool IsAnimating { get; }

        void Update(GameTime time);
    }
}

