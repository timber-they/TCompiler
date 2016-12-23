namespace TCompiler.Types.CompilingTypes
{
    public class Label : Command
    {
        public string Name { get; }

        public Label(string name)
        {
            Name = name;
        }
    }
}