#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Increment : OneParameterOperation
    {
        public Increment(ByteVariableCall paramA) : base(paramA)
        {
        }

        public override string ToString() => $"inc {((ByteVariableCall) ParamA).ByteVariable}";
    }
}