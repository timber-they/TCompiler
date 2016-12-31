using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public abstract class TwoParameterOperation : Operation
    {
        protected VariableCall ParamA { get; }
        protected VariableCall ParamB { get; }

        protected TwoParameterOperation(VariableCall paramA, VariableCall paramB)
        {
            ParamA = paramA;
            ParamB = paramB;
        }

        protected TwoParameterOperation(Tuple<VariableCall, VariableCall> pars)
        {
            ParamA = pars.Item1;
            ParamB = pars.Item2;
        }
    }
}