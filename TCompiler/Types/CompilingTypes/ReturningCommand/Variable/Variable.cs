namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable : Command
    {
        public string Name { get; }
        private readonly bool _isConstant;

        public Variable(bool isConstant, string name = null)
        {
            Name = name;
            _isConstant = isConstant;
        }
    }
}