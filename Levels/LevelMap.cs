using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Levels
{
    public class LevelMap
    {
        private static readonly HexCoord LeftDelta = new HexCoord(0, 1);
        private static readonly HexCoord RightDelta = new HexCoord(1, 1);
        private static readonly HexCoord SiblingDelta = new HexCoord(1, 0);

        private readonly Random _random;

        private readonly int _levelLength;

        private HexCoord _playerPosition;
        public HexCoord PlayerPosition { get { return _playerPosition; } }

        public Dictionary<HexCoord, LevelNode> Nodes;

        private Vector3 _basisQ;
        private Vector3 _basisR;
        private Vector3 _basisZ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="random"> Random object used to generate level </param>
        /// <param name="length"> Length of level (# of rows of nodes) </param>
        public LevelMap(Random random, int length = 15)
        {
            _random = random;

            _levelLength = length;

            _playerPosition = new HexCoord(0, 0);

            _basisQ = new Vector3(MathF.Sqrt(3), 0, 0);
            _basisR = new Vector3(-MathF.Sqrt(3) / 2f, 3f / 2f, 0);
            _basisZ = new Vector3(0, 0, -MathF.Sqrt(3) / 2f);

            Generate();
        }

        /// <summary>
        /// Gets the HexCoords of the next LevelNodes the player can travel to
        /// </summary>
        public HexCoord[] GetNextCoords()
        {
            List<HexCoord> nextCoords = new List<HexCoord>();
            HexCoord left = PlayerPosition + LeftDelta;
            HexCoord right = PlayerPosition + RightDelta;

            if (Nodes.ContainsKey(left))
            {
                nextCoords.Add(left);
            }
            if (Nodes.ContainsKey(right))
            {
                nextCoords.Add(right);
            }

            return nextCoords.ToArray();
        }

        /// <summary>
        /// Moves the player's position to the LevelNode at the given HexCoord.
        /// </summary>
        /// <returns>
        /// LevelNode at the given HexCoord (null if not found in level)
        /// </returns>
        public LevelNode MoveToNode(HexCoord target)
        {
            if (Nodes.ContainsKey(target))
            {
                _playerPosition = target;
                return Nodes[target];
            }

            return null;
        }

        /// <summary>
        /// Gets the position of the given HexCoord in the World matrix
        /// </summary>
        /// <returns>
        /// Vector3 position of the given HexCoord in the World matrix
        /// (Vector3.Z will be 0 if coord isn't found in level)
        /// </returns>
        public Vector3 GetWorldPosition(HexCoord coord)
        {
            int z = 0;
            if (Nodes.ContainsKey(coord))
            {
                z = Nodes[coord].Z;
            }

            return coord.Q * _basisQ + coord.R * _basisR + z * _basisZ;
        }

        /// <summary>
        /// Generates a new level map
        /// </summary>
        private void Generate()
        {
            Nodes = new Dictionary<HexCoord, LevelNode>();

            // add starting node
            Nodes.Add(new HexCoord(0, 0), new LevelNode(0));

            // add second row of nodes
            Nodes.Add(LeftDelta, new LevelNode(1));
            Nodes.Add(RightDelta, new LevelNode(1));

            // add remaining rows of nodes randomly
            int prevWidth = 2;
            HexCoord rowStart = LeftDelta + LeftDelta;
            for (int z = 2; z < _levelLength; z++)
            {
                int rowWidth = GetRowWidth();
                int rowOffset = GetRowOffset(prevWidth, rowWidth);
                HexCoord[] rowCoords = GetRowCoords(rowStart, rowWidth, rowOffset);

                for (int i = 0; i < rowCoords.Length; i++)
                {
                    Nodes.Add(rowCoords[i], new LevelNode(z));
                }

                rowStart = rowCoords[0] + LeftDelta;
                prevWidth = rowWidth;
            }
        }

        /// <summary>
        /// Generates a random width for a row of LevelNodes
        /// </summary>
        /// <returns>Either 2 or 3</returns>
        private int GetRowWidth()
        {
            return _random.Next(2, 4); // 2 or 3
        }

        /// <summary>
        /// Generates a random offset (in hexes) for a row of LevelNodes
        /// </summary>
        /// <param name="prevWidth">Width (in hexes) of previous row</param>
        /// <param name="nextWidth">Width (in hexes) of row to be generated</param>
        /// <returns>Either 0 or 1</returns>
        private int GetRowOffset(int prevWidth, int nextWidth)
        {
            if (prevWidth < nextWidth)
            {
                return 0;
            }
            else if (prevWidth > nextWidth)
            {
                return 1;
            }
            else // prevSize == nextSize
            {
                return _random.Next(0, 2); // 0 or 1
            }
        }

        /// <summary>
        /// Generates a random array of HexCoords for a row of LevelNodes
        /// </summary>
        /// <param name="start">Leftmost HexCoord of row to be generated</param>
        /// <param name="width">Width (in hexes) of row to be generated</param>
        /// <param name="offset">Offset (in hexes) of row to be generated</param>
        /// <returns>
        /// An array of either 2 or 3 HexCoords that span the given width
        /// </returns>
        private HexCoord[] GetRowCoords(HexCoord start, int width, int offset)
        {
            int nodesInRow = width == 2 ? 2 : _random.Next(2, 4); // 2 or 3

            HexCoord[] rowCoords = new HexCoord[nodesInRow];

            int missingIndex = width > nodesInRow ? 1 : -1;
            int rowIndex = 0;

            for (int i = 0; i < width; i++)
            {
                if (i != missingIndex)
                {
                    rowCoords[rowIndex++] = start + ((i + offset) * SiblingDelta);
                }
            }

            return rowCoords;
        }
    }
}

