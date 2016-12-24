namespace TCompiler.Types.CompilingTypes
{
    public class Label : Command
    {
        public string Name { get; }

        public Label(string name)
        {
            Name = new string(name.ToCharArray());
        }

        public override string ToString() => Name;
    }
}