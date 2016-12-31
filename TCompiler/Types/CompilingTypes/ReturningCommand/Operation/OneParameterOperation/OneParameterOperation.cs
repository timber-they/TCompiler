using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public abstract class OneParameterOperation : Operation
    {
        protected VariableCall ParamA { get; }

        protected OneParameterOperation(VariableCall paramA)
        {
            ParamA = paramA;
        }
    }
}