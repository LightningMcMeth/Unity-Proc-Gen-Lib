using System;

namespace ProcGen.Graph.Core
{
    [Serializable]
    public struct HexCellDefinition
    {
        public int X;
        public int Y;
        public string Name;
        public NodeProp Property;
    }
}
