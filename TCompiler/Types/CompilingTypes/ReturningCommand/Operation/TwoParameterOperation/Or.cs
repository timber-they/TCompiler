using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Or : TwoParameterOperation
    {
        public Or(Tuple<VariableCall, VariableCall> pars) : base(pars)
        {
        }

        public override string ToString()
            =>
            ParamA is ByteVariableCall
                ? $"mov A, {((ByteVariableCall) ParamA).Variable}\norl A, {((ByteVariableCall) ParamB).Variable}"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\nmov C, {((BitVariableCall) ParamA).Variable}\norl C, acc.0";
    }
}