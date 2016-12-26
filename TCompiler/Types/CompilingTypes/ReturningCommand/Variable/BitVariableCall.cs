using TCompiler.Compiling;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitVariableCall : VariableCall
    {
        public BitVariable Variable { get; }

        public BitVariableCall(BitVariable variable)
        {
            Variable = variable;
        }

        public override string ToString()
            =>
            AssembleHelp.AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label.ToString(), this);
    }
}