using Unity.Mathematics;

namespace ProcGen.Seed
{
    public sealed class UnityRngStream : IRngStream
    {
        private Random _rng;

        public UnityRngStream(uint seed)
        {
            // !!!!REMINDER!!!!! unity.mathematics.random cannot be seeded with 0
            _rng = new Random(seed == 0u ? 1u : seed);
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            return _rng.NextInt(minInclusive, maxExclusive);
        }

        public float NextFloat01()
        {
            return _rng.NextFloat(0f, 1f);
        }

        public uint NextUInt()
        {
            return _rng.NextUInt();
        }
    }
}