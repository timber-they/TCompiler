using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Modulo : TwoParameterOperation
    {
        public Modulo(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Modulo(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
        public override string ToString() => $"mov A, {_paramA}\nmov B, {_paramB}\ndiv AB\nxch A, B";
    }
}