namespace ProcGen.Seed
{
    public interface ISeedHasher
    {
        string NormalizeSeed(string seed);
        uint HashSeed(string normalizedSeed);
        uint DeriveSubseed(uint baseSeed, string salt);
    }
}