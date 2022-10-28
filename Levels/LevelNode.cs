using System;

namespace Levels
{
    public enum NodeType
    {
        Empty,
        Shop,
        Event,
        Combat,
        Bonfire,
    }

    public class LevelNode
    {
        public int Z;

        NodeType Type;

        public LevelNode(int z) : this(z, NodeType.Empty) { }
        public LevelNode(int z, NodeType type)
        {
            Z = z;
            Type = type;
        }
    }
}

