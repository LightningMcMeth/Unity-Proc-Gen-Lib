using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ProcGen.Graph.Core
{
    public sealed class GridRepository
    {
        private static readonly Vector2Int[] EvenRowNeighborOffsets =
        {
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1)
        };

        private static readonly Vector2Int[] OddRowNeighborOffsets =
        {
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        };

        private readonly Hex[,] _grid;

        public GridRepository(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
            _grid = new Hex[width, height];
        }

        public int Width { get; }
        public int Height { get; }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public Hex GetNode(int x, int y)
        {
            EnsureInBounds(x, y);
            return _grid[x, y];
        }

        public void SetNode(int x, int y, Hex node)
        {
            EnsureInBounds(x, y);
            _grid[x, y] = node;
        }

        public Hex[] GetRow(int y)
        {
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            var row = new Hex[Width];
            for (int x = 0; x < Width; x++)
            {
                row[x] = _grid[x, y];
            }

            return row;
        }

        public void SetRow(int y, Hex[] row)
        {
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            if (row.Length != Width)
            {
                throw new ArgumentException("Row length must match grid width.", nameof(row));
            }

            for (int x = 0; x < Width; x++)
            {
                _grid[x, y] = row[x];
            }
        }

        public IReadOnlyList<HexCellRef> GetNeighbors(int x, int y)
        {
            EnsureInBounds(x, y);

            var neighbors = new List<HexCellRef>(6);
            Vector2Int[] offsets = (y & 1) == 0 ? EvenRowNeighborOffsets : OddRowNeighborOffsets;

            foreach (Vector2Int offset in offsets)
            {
                int neighborX = x + offset.x;
                int neighborY = y + offset.y;

                if (!IsInBounds(neighborX, neighborY))
                {
                    continue;
                }

                Hex neighborNode = _grid[neighborX, neighborY];
                if (neighborNode == null)
                {
                    continue;
                }

                neighbors.Add(new HexCellRef(neighborX, neighborY, GetNodeId(neighborX, neighborY), neighborNode));
            }

            return neighbors;
        }

        public int GetNodeId(int x, int y)
        {
            EnsureInBounds(x, y);
            return x + y * Width;
        }

        public void Clear()
        {
            Array.Clear(_grid, 0, _grid.Length);
        }

        public string ToDebugString(PropertyTracker propertyTracker)
        {
            var builder = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                if ((y & 1) == 1)
                {
                    builder.Append("  ");
                }

                for (int x = 0; x < Width; x++)
                {
                    Hex node = _grid[x, y];
                    string nodeName = node?.Name ?? "null";
                    NodeProp property = propertyTracker != null && propertyTracker.TryGetProperty(x, y, out NodeProp resolvedProperty)
                        ? resolvedProperty
                        : default;

                    builder.Append($"[{GetNodeId(x, y)}:{nodeName}:{property}]");
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private void EnsureInBounds(int x, int y)
        {
            if (!IsInBounds(x, y))
            {
                throw new ArgumentOutOfRangeException(nameof(x), $"Cell ({x}, {y}) is outside the grid bounds.");
            }
        }
    }
}
