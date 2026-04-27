using System.Collections.Generic;
using ProcGen.Graph.Core;
using UnityEngine;

namespace ProcGen.Materialization
{
    [CreateAssetMenu(menuName = "ProcGen/Grid Materialization Settings", fileName = "GridMaterializationSettings")]
    public sealed class GridMaterializationSettings : ScriptableObject
    {
        [SerializeField]
        private GameObject defaultHexPrefab;
        [SerializeField]
        private Vector3 origin = Vector3.zero;
        [Min(0.01f)]
        [SerializeField]
        private float horizontalSpacing = 1.75f;
        [Min(0.01f)]
        [SerializeField]
        private float verticalSpacing = 1.5f;
        [SerializeField]
        private float oddRowHorizontalOffset = 0.875f;
        [SerializeField]
        private List<NodePropPrefabBinding> prefabBindings = new();

        public Vector3 Origin => origin;
        public float HorizontalSpacing => horizontalSpacing;
        public float VerticalSpacing => verticalSpacing;
        public float OddRowHorizontalOffset => oddRowHorizontalOffset;

        public GameObject GetPrefab(NodeProp property)
        {
            foreach (NodePropPrefabBinding binding in prefabBindings)
            {
                if (binding.Property == property && binding.Prefab != null)
                {
                    return binding.Prefab;
                }
            }

            return defaultHexPrefab;
        }
    }

    [System.Serializable]
    public struct NodePropPrefabBinding
    {
        public NodeProp Property;
        public GameObject Prefab;
    }
}
