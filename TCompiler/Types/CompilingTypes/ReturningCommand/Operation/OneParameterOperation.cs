using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class OneParameterOperation : Operation
    {
        protected VariableCall _paramA;

        protected OneParameterOperation(VariableCall paramA)
        {
            _paramA = paramA;
        }
    }
}