using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Decrement : OneParameterOperation
    {
        public Decrement(ByteVariableCall paramA) : base(paramA)
        {
        }

        public override string ToString() => $"dec {((ByteVariableCall) _paramA).Variable}";
    }
}