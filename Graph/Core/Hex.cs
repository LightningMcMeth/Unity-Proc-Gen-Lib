namespace ProcGen.Graph.Core
{
    public sealed class Hex
    {
        public Hex(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Hex" : name;
        }

        public string Name { get; }
    }
}
