using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class Or : TwoParameterOperation
    {
        public Or(VariableCall paramA, VariableCall paramB) : base(paramA, paramB)
        {
        }

        public Or(Tuple<VariableCall, VariableCall> pars) : base(pars) { }

        public override string ToString()
            =>
            _paramA is ByteVariableCall
                ? $"mov A, {((ByteVariableCall)_paramA).Variable}\norl A, {((ByteVariableCall)_paramB).Variable}"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label1, (BitVariableCall) _paramB)}\nmov C, {((BitVariableCall) _paramA).Variable}\norl C, acc.0";
    }
}