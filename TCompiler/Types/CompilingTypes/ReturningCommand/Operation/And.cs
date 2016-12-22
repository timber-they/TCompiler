using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class And : TwoParameterOperation
    {
        public And(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public And(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
    }
}