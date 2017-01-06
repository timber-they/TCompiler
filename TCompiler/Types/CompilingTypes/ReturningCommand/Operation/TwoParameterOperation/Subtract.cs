#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Subtract : TwoParameterOperation
    {
        public Subtract(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        public override string ToString() => $"{ParamA}\nclr C\nsubb A, {((ByteVariableCall) ParamB).Variable}";
    }
}