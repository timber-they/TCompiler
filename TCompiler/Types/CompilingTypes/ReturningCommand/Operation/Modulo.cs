using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Modulo : TwoParameterOperation
    {
        public Modulo(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Modulo(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
    }
}