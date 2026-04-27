using System.Collections.Generic;

namespace ProcGen.Graph.Core
{
    public sealed class PropertyTracker
    {
        private readonly Dictionary<int, NodeProp> _properties = new();

        public PropertyTracker(int gridWidth)
        {
            if (gridWidth <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(gridWidth));
            }

            GridWidth = gridWidth;
        }

        public int GridWidth { get; }

        public void SetProperty(int nodeId, NodeProp property)
        {
            _properties[nodeId] = property;
        }

        public void SetProperty(int x, int y, NodeProp property)
        {
            SetProperty(GetNodeId(x, y), property);
        }

        public NodeProp GetProperty(int nodeId)
        {
            return _properties[nodeId];
        }

        public NodeProp GetProperty(int x, int y)
        {
            return GetProperty(GetNodeId(x, y));
        }

        public bool TryGetProperty(int nodeId, out NodeProp property)
        {
            return _properties.TryGetValue(nodeId, out property);
        }

        public bool TryGetProperty(int x, int y, out NodeProp property)
        {
            return TryGetProperty(GetNodeId(x, y), out property);
        }

        public int GetNodeId(int x, int y)
        {
            return x + y * GridWidth;
        }

        public void Clear()
        {
            _properties.Clear();
        }
    }
}
