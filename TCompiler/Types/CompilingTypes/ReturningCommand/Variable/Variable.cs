namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable : Command
    {
        public string Name { get; }
        public bool IsConstant { get; }

        public Variable(bool isConstant, string name = null)
        {
            Name = name;
            IsConstant = isConstant;
        }

        public override string ToString() => Name;
    }
}