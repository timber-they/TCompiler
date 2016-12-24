using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Increment : OneParameterOperation
    {
        public Increment(ByteVariableCall paramA) : base(paramA)
        {
        }

        public override string ToString() => $"inc {((ByteVariableCall) _paramA).Variable}";
    }
}