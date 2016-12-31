using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class And : TwoParameterOperation
    {
        public And(Tuple<VariableCall, VariableCall> pars) : base(pars)
        {
        }

        public override string ToString() => ParamA is ByteVariableCall && ParamB is ByteVariableCall
            ? $"mov A, {((ByteVariableCall) ParamA).Variable}\nanl A, {((ByteVariableCall) ParamB).Variable}"
            : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\nmov C, {((BitVariableCall) ParamA).Variable}\nanl C, acc.0";
    }
}