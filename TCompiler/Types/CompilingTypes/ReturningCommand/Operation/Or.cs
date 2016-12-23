using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Or : TwoParameterOperation
    {
        public Or(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Or(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }

        public override string ToString()
            =>
            _paramA is ByteVariable
                ? $"mov A, {_paramA}\norl A, {_paramB}"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label1, (BitVariable) _paramB)}\nmov C, {_paramA}\norl C, acc.0";
    }
}