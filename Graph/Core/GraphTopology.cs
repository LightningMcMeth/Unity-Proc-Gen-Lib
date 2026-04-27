using System.Collections.Generic;
using UnityEngine;

namespace ProcGen.Graph.Core
{
    [CreateAssetMenu(menuName = "ProcGen/Graph Topology", fileName = "GraphTopology")]
    public sealed class GraphTopology : ScriptableObject
    {
        [Min(1)]
        [SerializeField]
        private int width = 6;
        [Min(1)]
        [SerializeField]
        private int height = 6;
        [SerializeField]
        private string defaultHexName = "Hex";
        [SerializeField]
        private NodeProp defaultProperty = NodeProp.Type1;
        [SerializeField]
        private List<HexCellDefinition> cells = new();

        public int Width => Mathf.Max(1, width);
        public int Height => Mathf.Max(1, height);
        public string DefaultHexName => string.IsNullOrWhiteSpace(defaultHexName) ? "Hex" : defaultHexName;
        public NodeProp DefaultProperty => defaultProperty;
        public IReadOnlyList<HexCellDefinition> Cells => cells;
    }
}
