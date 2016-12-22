using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class TwoParameterOperation : Operation
    {
        private Variable.Variable _paramA;
        private Variable.Variable _paramB;

        protected TwoParameterOperation(Variable.Variable paramA, Variable.Variable paramB)
        {
            _paramA = paramA;
            _paramB = paramB;
        }

        protected TwoParameterOperation(Tuple<Variable.Variable, Variable.Variable> pars)
        {
            _paramA = pars.Item1;
            _paramB = pars.Item2;
        }
    }
}