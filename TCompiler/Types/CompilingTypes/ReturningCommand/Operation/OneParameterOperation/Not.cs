using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Not : OneParameterOperation
    {
        public Not(VariableCall paramA) : base(paramA)
        {
        }

        public override string ToString()
        {
            if (_paramA is ByteVariableCall)
                return $"mov A, {((ByteVariableCall) _paramA).Variable}\ncpl A";
            return
                $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label.ToString(), (BitVariableCall) _paramA)}\ncpl acc.0";
        }
    }
}