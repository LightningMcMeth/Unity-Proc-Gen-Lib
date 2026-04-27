using ProcGen.Graph.Core;
using ProcGen.Seed;

namespace ProcGen.Graph.Run
{
    public interface IInitialGridBuilder
    {
        void Build(IRngStream rng, GraphTopology topology);
    }
}
