namespace ProcGen.Seed
{
    public interface IRngStream
    {
        int NextInt(int minInclusive, int maxExclusive);
        float NextFloat01();
        uint NextUInt();
    }
}