#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class And : TwoParameterOperation
    {
        public And(Tuple<VariableCall, VariableCall> pars) : base(pars)
        {
        }

        public override string ToString() => ParamA is ByteVariableCall && ParamB is ByteVariableCall
            ? $"mov A, {((ByteVariableCall) ParamA).Variable}\nanl A, {((ByteVariableCall) ParamB).Variable}"
            : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\n" +
              $"{AssembleCodePreviews.MoveBitTo(new Bool(false, "C", "c"), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamA).Variable)}\n" +
              "anl C, acc.0\nmov acc.0, C";
    }
}