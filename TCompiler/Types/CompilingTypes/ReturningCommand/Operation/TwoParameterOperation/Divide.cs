#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Divide : TwoParameterOperation
    {
        public Divide(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public override string ToString() => $"{ParamA}\nmov B, {((ByteVariableCall) ParamB).Variable}\ndiv AB";
    }
}