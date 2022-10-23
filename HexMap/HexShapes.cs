using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace HexMap
{
    public enum HexDirection
    {
        Down,
        DownLeft,
        UpLeft,
        Up,
        UpRight,
        DownRight,
    }

    public enum HexPath
    {
        Up,
        LeftX,
        LeftY,
        RightX,
        RightY
    }

    public static class HexShapes
    {
        private static readonly HexCoord Origin = new HexCoord(0, 0);

        private static readonly Dictionary<int, HexCoord[]> _circles;
        private static readonly Dictionary<int, ReadOnlyDictionary<HexCoord, HexTile>> _eventNodes;
        private static readonly Dictionary<HexPath, ReadOnlyDictionary<HexCoord, HexTile>>_paths;

        private static readonly Dictionary<TileType, HexTile> _tileTypes;
        private static readonly HexCoord[] _directions;

        static HexShapes()
        {
            _circles = new Dictionary<int, HexCoord[]>();
            _eventNodes = new Dictionary<int, ReadOnlyDictionary<HexCoord, HexTile>>();
            _paths = new Dictionary<HexPath, ReadOnlyDictionary<HexCoord, HexTile>>();

            _tileTypes = new Dictionary<TileType, HexTile>()
            {
                { TileType.Default, new HexTile(TileType.Default) },
                { TileType.Wall, new HexTile(TileType.Wall) },
                { TileType.Path, new HexTile(TileType.Path) },
            };

            _directions = new HexCoord[]
            {
                new HexCoord(0, -1),
                new HexCoord(-1, 0),
                new HexCoord(-1, 1),
                new HexCoord(0, 1),
                new HexCoord(1, 0),
                new HexCoord(1, -1)
            };
        }

        public static ReadOnlyDictionary<HexCoord, HexTile> Translate(HexCoord origin, ReadOnlyDictionary<HexCoord, HexTile> shape)
        {
            Dictionary<HexCoord,HexTile> translated = new Dictionary<HexCoord, HexTile>();

            foreach (KeyValuePair<HexCoord, HexTile> entry in shape)
            {
                HexCoord coord = entry.Key;
                HexTile tile = entry.Value;

                translated.Add(origin + coord, tile);
            }

            return new ReadOnlyDictionary<HexCoord, HexTile>(translated);
        }

        public static HexCoord[] Circle(int radius = 3)
        {
            // return cached result if it exists
            if (_circles.ContainsKey(radius))
            {
                return _circles[radius];
            }

            HexCoord[] circle = new HexCoord[GetCircleSize(radius)];

            // walk {radius} steps in each HexDirection
            HexCoord hex = new HexCoord(radius, 0);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    circle[i * radius + j] = hex;
                    hex = GetNeighbor(hex, i);
                }
            }

            // cache result and return
            _circles.Add(radius, circle);
            return circle;
        }

        public static ReadOnlyDictionary<HexCoord, HexTile> EventNode(float height = 0f, int radius = 3)
        {
            // return cached result if it exists
            if (_eventNodes.ContainsKey(radius))
            {
                return _eventNodes[radius];
            }

            Dictionary<HexCoord, HexTile> eventNode = new Dictionary<HexCoord, HexTile>();

            // set perimeter to Wall
            foreach (HexCoord coord in Circle(radius))
            {
                eventNode.Add(coord, new HexTile(TileType.Wall, height));//_tileTypes[TileType.Wall]);
            }

            // set inner rings to Path
            for (int r = radius - 1; r >= 0; r--)
            {
                foreach (HexCoord coord in Circle(r))
                {
                    eventNode.Add(coord, new HexTile(TileType.Path, height));//_tileTypes[TileType.Path]);
                }
            }

            // cache result and return
            ReadOnlyDictionary<HexCoord,HexTile> result = new ReadOnlyDictionary<HexCoord, HexTile>(eventNode);
            //_eventNodes.Add(radius, result);
            return result;
        }

        public static ReadOnlyDictionary<HexCoord, HexTile> Path(HexPath path)
        {
            // return cached result if it exists
            if (_paths.ContainsKey(path))
            {
                return _paths[path];
            }

            switch(path)
            {
                case HexPath.LeftX:
                    return PathXLeft();

                case HexPath.LeftY:
                    return PathYLeft();

                case HexPath.RightX:
                    return PathXRight();

                case HexPath.RightY:
                    return PathYRight();

                default:
                    return PathUp();
            }
        }

        public static ReadOnlyDictionary<HexCoord, HexTile> PathUp(int length = 5)
        {
            // return cached result if it exists
            if (length == 5 && _paths.ContainsKey(HexPath.Up))
            {
                return _paths[HexPath.Up];
            }

            Dictionary<HexCoord, HexTile> pathUp = new Dictionary<HexCoord, HexTile>();

            // set left and right side to Wall
            HexCoord leftHex = GetNeighbor((int)HexDirection.UpLeft);
            HexCoord rightHex = GetNeighbor((int)HexDirection.UpRight);
            for (int i = 0; i < length - 1; i++)
            {
                pathUp.Add(leftHex, _tileTypes[TileType.Wall]);
                pathUp.Add(rightHex, _tileTypes[TileType.Wall]);

                leftHex = GetNeighbor(leftHex, (int)HexDirection.Up);
                rightHex = GetNeighbor(rightHex, (int)HexDirection.Up);
            }

            // set center to Path
            HexCoord centerHex = Origin;
            for (int i = 0; i < length; i++)
            {
                pathUp.Add(centerHex, _tileTypes[TileType.Path]);

                centerHex = GetNeighbor(centerHex, (int)HexDirection.Up);
            }

            // cache result and return
            ReadOnlyDictionary<HexCoord, HexTile> result = new ReadOnlyDictionary<HexCoord, HexTile>(pathUp);
            if (length == 5) _paths.Add(HexPath.Up, result);
            return result;
        }

        public static HexCoord GetNeighbor(HexCoord coord, int side)
        {
            return coord + _directions[side % 6];
        }
        public static HexCoord GetNeighbor(int side)
        {
            return _directions[side % 6];
        }

        private static ReadOnlyDictionary<HexCoord, HexTile> PathXLeft()
        {
            return PathX(true);
        }
        private static ReadOnlyDictionary<HexCoord, HexTile> PathXRight()
        {
            return PathX(false);
        }
        private static ReadOnlyDictionary<HexCoord, HexTile> PathX(bool isLeft)
        {
            Dictionary<HexCoord, HexTile> pathX = new Dictionary<HexCoord, HexTile>();

            int dirPath = (int)(isLeft ? HexDirection.UpLeft : HexDirection.UpRight);
            int dirOffset = (int)(isLeft ? HexDirection.UpRight : HexDirection.UpLeft);
            int dirBottom = (int)(isLeft ? HexDirection.DownLeft : HexDirection.DownRight);

            HexCoord topHex = GetNeighbor((int)HexDirection.Up);
            HexCoord centerHex = Origin;
            HexCoord bottomHex = GetNeighbor(dirBottom);

            // add bottom 2 HexTiles
            pathX.Add(centerHex, _tileTypes[TileType.Path]);
            pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

            centerHex = GetNeighbor(centerHex, dirPath);
            bottomHex = GetNeighbor(bottomHex, dirPath);

            // add staggered rows of 3 HexTiles
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    pathX.Add(topHex, _tileTypes[TileType.Wall]);
                    pathX.Add(centerHex, _tileTypes[TileType.Path]);
                    pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

                    topHex = GetNeighbor(topHex, dirPath);
                    centerHex = GetNeighbor(centerHex, dirPath);
                    bottomHex = GetNeighbor(bottomHex, dirPath);
                }

                topHex = GetNeighbor(topHex, dirOffset);
                centerHex = GetNeighbor(centerHex, dirOffset);
                bottomHex = GetNeighbor(bottomHex, dirOffset);
            }

            // add top 2 HexTiles
            topHex = GetNeighbor(topHex, dirBottom);
            centerHex = GetNeighbor(centerHex, dirBottom);

            pathX.Add(topHex, _tileTypes[TileType.Wall]);
            pathX.Add(centerHex, _tileTypes[TileType.Path]);

            // cache result and return
            ReadOnlyDictionary<HexCoord, HexTile> result = new ReadOnlyDictionary<HexCoord, HexTile>(pathX);
            if (isLeft) _paths.Add(HexPath.LeftX, result);
            else _paths.Add(HexPath.RightX, result);
            return result;
        }

        private static ReadOnlyDictionary<HexCoord, HexTile> PathYLeft()
        {
            return PathY(true);
        }
        private static ReadOnlyDictionary<HexCoord, HexTile> PathYRight()
        {
            return PathY(false);
        }
        private static ReadOnlyDictionary<HexCoord, HexTile> PathY(bool isLeft)
        {
            Dictionary<HexCoord, HexTile> pathX = new Dictionary<HexCoord, HexTile>();

            int dirPath = (int)(isLeft ? HexDirection.UpLeft : HexDirection.UpRight);
            int dirOffset = (int)(isLeft ? HexDirection.UpRight : HexDirection.UpLeft);
            int dirBottom = (int)(isLeft ? HexDirection.DownLeft : HexDirection.DownRight);

            HexCoord topHex = GetNeighbor((int)HexDirection.Up);
            HexCoord centerHex = Origin;
            HexCoord bottomHex = GetNeighbor(dirBottom);

            // add bottom 2 HexTiles
            pathX.Add(centerHex, _tileTypes[TileType.Path]);
            pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

            centerHex = GetNeighbor(centerHex, dirPath);
            bottomHex = GetNeighbor(bottomHex, dirPath);


            // add rows of HexTiles moving along dirPath
            for (int i = 0; i < 3; i++)
            {
                pathX.Add(topHex, _tileTypes[TileType.Wall]);
                pathX.Add(centerHex, _tileTypes[TileType.Path]);
                pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

                topHex = GetNeighbor(topHex, dirPath);
                centerHex = GetNeighbor(centerHex, dirPath);
                bottomHex = GetNeighbor(bottomHex, dirPath);
            }

            topHex = GetNeighbor(topHex, dirOffset);
            centerHex = GetNeighbor(centerHex, dirOffset);
            bottomHex = GetNeighbor(bottomHex, dirOffset);


            // add rows of Hextiles moving up
            for (int i = 0; i < 2; i++)
            {
                pathX.Add(topHex, _tileTypes[TileType.Wall]);
                pathX.Add(centerHex, _tileTypes[TileType.Path]);
                pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

                topHex = GetNeighbor(topHex, (int)HexDirection.Up);
                centerHex = GetNeighbor(centerHex, (int)HexDirection.Up);
                bottomHex = GetNeighbor(bottomHex, (int)HexDirection.Up);
            }


            // add top 2 HexTiles
            pathX.Add(centerHex, _tileTypes[TileType.Path]);
            pathX.Add(bottomHex, _tileTypes[TileType.Wall]);

            // cache result and return
            ReadOnlyDictionary<HexCoord, HexTile> result = new ReadOnlyDictionary<HexCoord, HexTile>(pathX);
            if (isLeft) _paths.Add(HexPath.LeftX, result);
            else _paths.Add(HexPath.RightX, result);
            return result;
        }

        private static int GetCircleSize(int radius)
        {
            if (radius == 0) return 1;
            return radius * 6;
        }
    }
}

