using ProcGen.Graph.Core;
using ProcGen.Seed;
using UnityEngine;

namespace ProcGen.Graph.Run
{
    public sealed class ManualGridBuilder : IInitialGridBuilder
    {
        private readonly GridRepository _gridRepository;
        private readonly PropertyTracker _propertyTracker;

        public ManualGridBuilder(GridRepository gridRepository, PropertyTracker propertyTracker)
        {
            _gridRepository = gridRepository;
            _propertyTracker = propertyTracker;
        }

        public void Build(IRngStream rng, GraphTopology topology)
        {
            _ = rng;

            if (topology == null)
            {
                Debug.LogError("Cannot build grid because GraphTopology is null.");
                return;
            }

            if (_gridRepository.Width != topology.Width || _gridRepository.Height != topology.Height)
            {
                Debug.LogError("GridRepository dimensions do not match GraphTopology.");
                return;
            }

            _gridRepository.Clear();
            _propertyTracker.Clear();

            for (int y = 0; y < topology.Height; y++)
            {
                for (int x = 0; x < topology.Width; x++)
                {
                    string defaultName = $"{topology.DefaultHexName} ({x},{y})";
                    _gridRepository.SetNode(x, y, new Hex(defaultName));
                    _propertyTracker.SetProperty(x, y, topology.DefaultProperty);
                }
            }

            foreach (HexCellDefinition cell in topology.Cells)
            {
                if (!_gridRepository.IsInBounds(cell.X, cell.Y))
                {
                    Debug.LogWarning($"Skipping out-of-bounds topology cell at ({cell.X}, {cell.Y}).");
                    continue;
                }

                Hex currentNode = _gridRepository.GetNode(cell.X, cell.Y);
                string resolvedName = string.IsNullOrWhiteSpace(cell.Name) ? currentNode.Name : cell.Name;

                _gridRepository.SetNode(cell.X, cell.Y, new Hex(resolvedName));
                _propertyTracker.SetProperty(cell.X, cell.Y, cell.Property);
            }
        }
    }
}
