namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class VariableCall : ReturningCommand
    {
        private Variable _variable;

        public VariableCall(Variable variable)
        {
            _variable = variable;
        }
    }
}