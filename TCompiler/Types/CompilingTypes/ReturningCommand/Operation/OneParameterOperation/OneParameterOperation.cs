#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public abstract class OneParameterOperation : Operation
    {
        protected OneParameterOperation(VariableCall paramA)
        {
            ParamA = paramA;
        }

        protected VariableCall ParamA { get; }
    }
}