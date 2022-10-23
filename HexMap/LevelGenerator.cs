using System;
namespace HexMap
{
    public class LevelGenerator
    {
        private class NodeRow
        {
            private readonly static HexCoord[] IndexCoords = new HexCoord[5]
            {
                new HexCoord(-12, 6), //(-10, 5),
                new HexCoord(-6, 2), //(-5, 2),
                new HexCoord(0, 0),
                new HexCoord(6, -4), //(5, -3),
                new HexCoord(12, -6), //(10, -5),
            };

            public int Width;
            public HexCoord Center;
            public int[] NodeIndices;

            public NodeRow(HexCoord center, int[] nodeIndices)
            {
                Center = center;
                NodeIndices = nodeIndices;
                Width = nodeIndices[nodeIndices.Length - 1] - nodeIndices[0] + 1;
            }

            public static HexCoord GetHexOffset(int index)
            {
                return IndexCoords[index + 2];
            }

            public HexCoord[] GetNodeCoords()
            {
                HexCoord[] result = new HexCoord[NodeIndices.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = Center + GetHexOffset(NodeIndices[i]);
                }
                return result;
            }

        }

        public readonly static int MinRowSize = 2;
        public readonly static int MaxRowSize = 3;

        private readonly static HexCoord _rowGap = new HexCoord(0, 12); //(0, 8);

        public int Length { get; }

        private Random _rand;

        private NodeRow[] _nodeRows;


        public LevelGenerator(int n)
        {
            Length = n;

            _rand = new Random();

            GenerateLevel(Length);
        }

        public void WriteLevel(HexMap hexMap)
        {
            for (int r = 0; r < _nodeRows.Length; r++)
            {
                NodeRow nodeRow = _nodeRows[r];
                HexCoord[] nodeCoords = nodeRow.GetNodeCoords();

                for (int n = 0; n < nodeCoords.Length; n++)
                {
                    HexCoord nodeCoord = nodeCoords[n];

                    hexMap.SetTiles(HexShapes.Translate(nodeCoord, HexShapes.EventNode(height: r * -9f)));
                }
            }
        }

        private void GenerateLevel(int length)
        {
            _nodeRows = new NodeRow[length];

            HexCoord rowCenter = new HexCoord(0, 0);
            NodeRow prevRow = new NodeRow(rowCenter, GetNodeIndices(1));
            _nodeRows[0] = prevRow;

            for (int i = 1; i < _nodeRows.Length; i++)
            {
                int rowSize = _rand.Next(MinRowSize, MaxRowSize + 1);//prevRow.NodeIndices.Length == 3 ? 2 : _rand.Next(MinRowSize, MaxRowSize + 1);
                int[] nodeIndices = GetNodeIndices(rowSize);
                int nextWidth = nodeIndices[nodeIndices.Length - 1] - nodeIndices[0] + 1;
                int rowOffset = GetRowOffset(prevRow.Width, nextWidth);

                rowCenter += NodeRow.GetHexOffset(rowOffset) + _rowGap;

                prevRow = new NodeRow(rowCenter, nodeIndices);
                _nodeRows[i] = prevRow;
            }
        }

        // generates offset of next row in the range [-2, 2]
        private int GetRowOffset(int prevWidth, int nextWidth)
        {
            int variability = 0;
            if (prevWidth == 3)
            {
                variability = 1;
            }
            else if (prevWidth == 5 && nextWidth == 5)
            {
                variability = 2;
            }

            int rowOffset = _rand.Next(-variability, variability + 1);
            return rowOffset;
        }

        // generates locations of nodes within a given row
        private int[] GetNodeIndices(int numNodes)
        {
            int[] nodeIndices = new int[numNodes];

            if (numNodes == 1)
            {
                nodeIndices[0] = 0;
            }
            else if (numNodes == 2)
            {
                int startIndex = _rand.Next(0, 4);
                if (startIndex >= 3)
                {
                    nodeIndices[0] = -2;
                    nodeIndices[1] = 2;
                }
                else
                {
                    nodeIndices[0] = -2 + startIndex;
                    nodeIndices[1] = startIndex;
                }
            }
            else if (numNodes == 3)
            {
                nodeIndices[0] = -2;
                nodeIndices[1] = 0;
                nodeIndices[2] = 2;
            }

            return nodeIndices;
        }
    }
}

