namespace TCompiler.Types.CompilingTypes
{
    public class Label : Command
    {
        public Label(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString() => Name;
    }
}