#region

using TCompiler.AssembleHelp;
using TCompiler.Compiling;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitVariableCall : VariableCall
    {
        public BitVariableCall(BitVariable bitVariable) : base(bitVariable)
        {
            BitVariable = bitVariable;
        }

        public BitVariable BitVariable { get; }

        public override string ToString()
            =>
            AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, this);
    }
}