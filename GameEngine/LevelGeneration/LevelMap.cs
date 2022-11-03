using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverythingUnder.Screens;

namespace EverythingUnder.Levels
{
    public class LevelMap
    {
        private static readonly HexCoord LeftDelta = new HexCoord(0, 1);
        private static readonly HexCoord RightDelta = new HexCoord(1, 1);
        private static readonly HexCoord SiblingDelta = new HexCoord(1, 0);

        private static readonly Vector3 CameraOffset = new Vector3(0, 0.3f, 1.8f);

        private static readonly Vector3 _basisQ = new Vector3(MathF.Sqrt(3), 0, 0);
        private static readonly Vector3 _basisR = new Vector3(-MathF.Sqrt(3) / 2f, 3f / 2f, 0);
        private static readonly Vector3 _basisZ = new Vector3(0, 0, -MathF.Sqrt(3) / 2f);

        private readonly GameManager _game;
        private readonly Random _random;

        private readonly int _levelLength;

        private HexCoord _playerPosition;
        private int _playerPositionIndex;
        public HexCoord PlayerPosition { get { return _playerPosition; } }


        public Dictionary<HexCoord, LevelNode> Nodes;
        public Vector3[] CameraPositions;

        public HexCoord?[] NextCoords;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="random"> Random object used to generate level </param>
        /// <param name="length"> Length of level (# of rows of nodes) </param>
        public LevelMap(GameManager game, int length = 15)
        {
            _game = game;
            _random = game.Random;

            _levelLength = length;

            Generate();

            _playerPosition = new HexCoord(0, 0);
            _playerPositionIndex = 0;
            NextCoords = GetNextCoords();
        }

        private void UpdatePlayerPosition(HexCoord target)
        {
            _playerPosition = target;
            _playerPositionIndex++;

            CameraPositions[_playerPositionIndex] = GetWorldPosition(target) + CameraOffset;

            NextCoords = GetNextCoords();
        }

        /// <summary>
        /// Gets the HexCoords of the next LevelNodes the player can travel to
        /// </summary>
        private HexCoord?[] GetNextCoords()
        {
            HexCoord?[] nextCoords = new HexCoord?[2];
            HexCoord left = PlayerPosition + LeftDelta;
            HexCoord right = PlayerPosition + RightDelta;


            nextCoords[0] = Nodes.ContainsKey(left) ? left : null;
            nextCoords[1] = Nodes.ContainsKey(right) ? right : null;

            return nextCoords;
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
                UpdatePlayerPosition(target);

                LevelNode node = Nodes[target];

                //_game.ScreenManager.LoadScreen(new EventScreen(_game), new FadeTransition(_game.GraphicsDevice, Color.CornflowerBlue));

                return node;
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
        /// Gets the position of the given HexCoord in the World matrix
        /// </summary>
        /// <param name="q">Q coordinate as a float value</param>
        /// <param name="r">R coordinate as a float value</param>
        /// <param name="r">Z coordinate as a float value</param>
        /// <returns>
        /// Vector3 position of the given HexCoord in the World matrix
        /// </returns>
        public Vector3 GetWorldPosition(float q, float r, float z)
        {
            return q * _basisQ + r * _basisR + z * _basisZ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        private Vector3 GetCameraPosition(HexCoord[] coords)
        {
            float z = Nodes.ContainsKey(coords[0]) ? Nodes[coords[0]].Z : 0f;
            float sumQ = 0;
            float sumR = 0;

            for (int i = 0; i < coords.Length; i++)
            {
                sumQ += coords[i].Q;
                sumR += coords[i].R;
            }

            float n = (float)coords.Length;
            float avgQ = sumQ / n;
            float avgR = sumR / n;

            return GetWorldPosition(avgQ, avgR, z) + CameraOffset;
        }
        public Vector3 GetCameraPosition(HexCoord coord)
        {
            return GetWorldPosition(coord) + CameraOffset;
        }

        /// <summary>
        /// Generates a new level map
        /// </summary>
        private void Generate()
        {
            Nodes = new Dictionary<HexCoord, LevelNode>();
            CameraPositions = new Vector3[_levelLength];

            // add starting node
            HexCoord start = new HexCoord(0, 0);
            Nodes.Add(start, new LevelNode(0));
            CameraPositions[0] = GetCameraPosition(new HexCoord[] { start });

            // add second row of nodes
            HexCoord[] secondRow = new HexCoord[] { LeftDelta, RightDelta };
            Nodes.Add(secondRow[0], new LevelNode(1));
            Nodes.Add(secondRow[1], new LevelNode(1));
            CameraPositions[1] = GetCameraPosition(secondRow);

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

                CameraPositions[z] = GetCameraPosition(rowCoords);

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

