using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Not : OneParameterOperation
    {
        public Not(Variable.Variable paramA) : base(paramA)
        {
        }

        public override string ToString()
            =>
            _paramA is ByteVariable
                ? $"mov A, {_paramA}\ncpl A"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label1, (BitVariable) _paramA)}\ncpl acc.0";
    }
}