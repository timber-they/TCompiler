using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class And : TwoParameterOperation
    {
        public And(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public And(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }

        public override string ToString() => _paramA is ByteVariable && _paramB is ByteVariable
            ? $"mov A, {_paramA}\nanl A, {_paramB}"
            : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label1, (BitVariable)_paramB)}\nmov C, {_paramA}\nanl C, acc.0";
    }
}