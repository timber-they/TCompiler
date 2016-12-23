namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : Command
    {
        public string Name { get; }

        public Method(string name)
        {
            Name = name;
        }
    }
}