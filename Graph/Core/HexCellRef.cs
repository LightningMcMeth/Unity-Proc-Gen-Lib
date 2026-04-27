namespace ProcGen.Graph.Core
{
    public readonly struct HexCellRef
    {
        public HexCellRef(int x, int y, int nodeId, Hex node)
        {
            X = x;
            Y = y;
            NodeId = nodeId;
            Node = node;
        }

        public int X { get; }
        public int Y { get; }
        public int NodeId { get; }
        public Hex Node { get; }
    }
}
