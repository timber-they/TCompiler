#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

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
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\n" +
                  $"{AssembleCodePreviews.MoveBitTo(new Bool(false, "C", "c"), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamA).Variable)}\n" +
                  $"\norl C, acc.0\nmov acc.0, C";
    }
}