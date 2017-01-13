namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// The base class
    /// </summary>
    public abstract class VariableCall : ReturningCommand
    {
        protected VariableCall(Variable variable) : base(false, false, new []{1})
        {
            Variable = variable;
        }

        public Variable Variable { get; }
    }
}