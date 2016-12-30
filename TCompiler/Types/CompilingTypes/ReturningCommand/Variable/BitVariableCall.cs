using TCompiler.AssembleHelp;
using TCompiler.Compiling;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitVariableCall : VariableCall
    {
        public BitVariableCall(BitVariable variable)
        {
            Variable = variable;
        }

        public BitVariable Variable { get; }

        public override string ToString()
            =>
            AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, this);
    }
}