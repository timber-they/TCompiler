namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class VariableCall : ReturningCommand
    {
        protected VariableCall(Variable variable) : base(false, false)
        {
            Variable = variable;
        }

        public Variable Variable { get; }
    }
}