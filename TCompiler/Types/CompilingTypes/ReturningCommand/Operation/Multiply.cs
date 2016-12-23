using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Multiply : TwoParameterOperation
    {
        public Multiply(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Multiply(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
        public override string ToString() => $"mov A, {_paramA}\nmov B, {_paramB}\nmul AB";
    }
}