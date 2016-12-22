namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable : ReturningCommand
    {
        public string Name { get; }
        private readonly bool _isConstant;

        protected Variable(bool isConstant, string name = null)
        {
            Name = name;
            _isConstant = isConstant;
        }
    }
}