using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class And : TwoParameterOperation
    {
        public And(VariableCall paramA, VariableCall paramB) : base(paramA, paramB)
        {
        }

        public And(Tuple<VariableCall, VariableCall> pars) : base(pars) { }

        public override string ToString() => _paramA is ByteVariableCall && _paramB is ByteVariableCall
            ? $"mov A, {((ByteVariableCall)_paramA).Variable}\nanl A, {((ByteVariableCall)_paramB).Variable}"
            : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label.ToString(), (BitVariableCall)_paramB)}\nmov C, {((BitVariableCall)_paramA).Variable}\nanl C, acc.0";
    }
}