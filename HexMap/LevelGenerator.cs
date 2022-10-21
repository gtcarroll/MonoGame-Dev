using System;
namespace HexMap
{
    public class LevelGenerator
    {
        public readonly static int MinRowSize = 2;
        public readonly static int MaxRowSize = 4;

        public int Length { get; }

        private Random _rand;

        private int[] _nodeRows;

        public LevelGenerator(int n)
        {
            Length = n;

            _rand = new Random();
            GenerateLevel(Length);
        }

        private void GenerateLevel(int length)
        {
            _nodeRows = new int[length];

            _nodeRows[0] = 1;

            for (int i = 1; i < _nodeRows.Length; i++)
            {
                int prevSize = _nodeRows[i - 1];
                int minSize = Math.Max(MinRowSize, prevSize - 2);
                int maxSize = Math.Min(MaxRowSize, prevSize + 2);
                int nextSize = _rand.Next(minSize, maxSize);

                _nodeRows[i] = nextSize;
            }
        }

        //public void DrawLevel(OldHexGrid hexGrid)
        //{
        //    OldHexCoord horizontalOffset = new OldHexCoord(5, 0);
        //    OldHexCoord verticalOffsetSame = new OldHexCoord(0, 8);
        //    OldHexCoord verticalOffsetDiff = new OldHexCoord(0, 7);
        //    OldHexCoord colGap = new OldHexCoord(10, 0);

        //    OldHexCoord prevLeftmostNode = new OldHexCoord(0, 0);
        //    hexGrid.Stamp(prevLeftmostNode, 0);

        //    for (int i = 1; i < _nodeRows.Length; i++)
        //    {
        //        int prevRowSize = _nodeRows[i - 1];
        //        int nextRowSize = _nodeRows[i];
        //        int deltaSize = prevRowSize - nextRowSize;
        //        bool isSameSign = prevRowSize % 2 == nextRowSize % 2;

        //        OldHexCoord nextLeftmostNode = prevLeftmostNode
        //            + (deltaSize * horizontalOffset)
        //            + (isSameSign ? verticalOffsetSame : verticalOffsetDiff);

        //        OldHexCoord nodeCenter = nextLeftmostNode;
        //        for (int col = 0; col < nextRowSize; col++)
        //        {
        //            hexGrid.StampCircle(nodeCenter, 2, 0f);
        //            nodeCenter += colGap;
        //        }

        //        prevLeftmostNode = nextLeftmostNode;
        //    }
        //}
    }
}

