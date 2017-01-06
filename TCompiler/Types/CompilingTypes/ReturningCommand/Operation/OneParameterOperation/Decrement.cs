#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Decrement : OneParameterOperation
    {
        public Decrement(ByteVariableCall paramA) : base(paramA)
        {
        }

        public override string ToString() => $"dec {((ByteVariableCall) ParamA).Variable}";
    }
}