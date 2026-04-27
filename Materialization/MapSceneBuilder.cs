using ProcGen.Graph.Core;
using UnityEngine;

namespace ProcGen.Materialization
{
    public sealed class MapSceneBuilder
    {
        private readonly GridRepository _gridRepository;
        private readonly PropertyTracker _propertyTracker;

        public MapSceneBuilder(GridRepository gridRepository, PropertyTracker propertyTracker)
        {
            _gridRepository = gridRepository;
            _propertyTracker = propertyTracker;
        }

        public Transform Materialize(GridMaterializationSettings settings, Transform parent)
        {
            if (settings == null)
            {
                Debug.LogError("Cannot materialize grid because settings are null.");
                return null;
            }

            var root = new GameObject("Generated Hex Grid").transform;
            root.SetParent(parent, false);

            for (int y = 0; y < _gridRepository.Height; y++)
            {
                for (int x = 0; x < _gridRepository.Width; x++)
                {
                    Hex node = _gridRepository.GetNode(x, y);
                    if (node == null)
                    {
                        continue;
                    }

                    NodeProp property = _propertyTracker.GetProperty(x, y);
                    GameObject prefab = settings.GetPrefab(property);

                    if (prefab == null)
                    {
                        Debug.LogWarning($"No prefab configured for {property}. Skipping cell ({x}, {y}).");
                        continue;
                    }

                    GameObject instance = Object.Instantiate(prefab, root);
                    instance.name = $"{node.Name} [{x},{y}]";
                    instance.transform.localPosition = GetCellPosition(settings, x, y);
                }
            }

            return root;
        }

        private static Vector3 GetCellPosition(GridMaterializationSettings settings, int x, int y)
        {
            float rowOffset = (y & 1) == 1 ? settings.OddRowHorizontalOffset : 0f;
            float xPosition = settings.Origin.x + x * settings.HorizontalSpacing + rowOffset;
            float zPosition = settings.Origin.z + y * settings.VerticalSpacing;

            return new Vector3(xPosition, settings.Origin.y, zPosition);
        }
    }
}
