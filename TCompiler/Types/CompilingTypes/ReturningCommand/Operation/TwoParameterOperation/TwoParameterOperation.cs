﻿#region

using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public abstract class TwoParameterOperation : Operation
    {
        protected TwoParameterOperation(VariableCall paramA, VariableCall paramB) : base(true, true, new []{1, 2, 3})
        {
            ParamA = paramA;
            ParamB = paramB;
        }

        protected TwoParameterOperation(Tuple<VariableCall, VariableCall> pars) : base(true, true, new []{1,2,3})
        {
            ParamA = pars.Item1;
            ParamB = pars.Item2;
        }

        protected VariableCall ParamA { get; }
        protected VariableCall ParamB { get; }
    }
}