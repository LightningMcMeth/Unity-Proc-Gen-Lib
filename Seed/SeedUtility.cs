using System;

namespace ProcGen.Seed
{
    public sealed class SeedUtility : ISeedHasher
    {
        public string NormalizeSeed(string seed)
        {
            if (seed == null)
            {
                return "";
            }

            return seed.Trim().ToLowerInvariant();
        }

        // FNV-1a 32bit
        public uint HashSeed(string normalizedSeed)
        {
            const uint fnvOffset = 2166136261u;
            const uint fnvPrime = 16777619u;

            uint hash = fnvOffset;
            for (int i = 0; i < normalizedSeed.Length; i++)
            {
                hash ^= normalizedSeed[i];
                hash *= fnvPrime;
            }
            return hash;
        }

        // Derive sub-seeds by hashing "baseSeed:salt"
        public uint DeriveSubseed(uint baseSeed, string salt)
        {
            string s = $"{baseSeed}:{(salt ?? "")}";
            return HashSeed(NormalizeSeed(s));
        }

        //build an RNG stream directly
        public IRngStream CreateStream(string userSeed, string salt)
        {
            string norm = NormalizeSeed(userSeed);
            uint baseSeed = HashSeed(norm);
            uint subSeed = DeriveSubseed(baseSeed, salt);
            
            return new UnityRngStream(subSeed);
        }
    }
}