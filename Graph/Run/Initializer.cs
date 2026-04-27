using ProcGen.Graph.Core;
using ProcGen.Materialization;
using UnityEngine;

namespace ProcGen.Graph.Run
{
    public sealed class Initializer
    {
        //they shouldn't be stored here
        public GridRepository GridRepository { get; private set; }
        public PropertyTracker PropertyTracker { get; private set; }
        public MapSceneBuilder Materializer { get; private set; }

        public void Initialize(GraphTopology topology)
        {
            if (topology == null)
            {
                Debug.LogError("GraphTopology is not assigned.");
                return;
            }

            bool needsRebuild =
                GridRepository == null ||
                GridRepository.Width != topology.Width ||
                GridRepository.Height != topology.Height;

            if (!needsRebuild)
            {
                return;
            }

            GridRepository = new GridRepository(topology.Width, topology.Height);
            PropertyTracker = new PropertyTracker(topology.Width);
            Materializer = new MapSceneBuilder(GridRepository, PropertyTracker);
        }
    }
}
