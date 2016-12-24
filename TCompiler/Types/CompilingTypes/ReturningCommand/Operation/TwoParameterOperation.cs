using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class TwoParameterOperation : Operation
    {
        protected VariableCall _paramA;
        protected VariableCall _paramB;

        protected TwoParameterOperation(VariableCall paramA, VariableCall paramB)
        {
            _paramA = paramA;
            _paramB = paramB;
        }

        protected TwoParameterOperation(Tuple<VariableCall, VariableCall> pars)
        {
            _paramA = pars.Item1;
            _paramB = pars.Item2;
        }
    }
}