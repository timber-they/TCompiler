#region

using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    public class Not : OneParameterOperation
    {
        public Not(VariableCall paramA) : base(paramA)
        {
        }

        public override string ToString()
        {
            var byteVariableCall = ParamA as ByteVariableCall;
            return byteVariableCall != null
                ? $"mov A, {byteVariableCall.ByteVariable}\ncpl A"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamA)}\ncpl acc.0";
        }
    }
}