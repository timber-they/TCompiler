using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Or : TwoParameterOperation
    {
        public Or(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Or(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
    }
}