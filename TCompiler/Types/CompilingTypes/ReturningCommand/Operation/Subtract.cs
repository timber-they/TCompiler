using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Subtract : TwoParameterOperation
    {
        public Subtract(Variable.Variable paramA, Variable.Variable paramB) : base(paramA, paramB)
        {
        }

        public Subtract(Tuple<Variable.Variable, Variable.Variable> pars) : base(pars) { }
    }
}