using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Add : TwoParameterOperation
    {
        public Add(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public override string ToString() => $"{ParamA}\nadd A, {((ByteVariableCall) ParamB).Variable}";
    }
}