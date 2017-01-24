#region

using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     The base class for operation with two parameters
    /// </summary>
    public abstract class TwoParameterOperation : Operation
    {
        /// <summary>
        ///     Initializes a new TwoParameterOperation
        /// </summary>
        /// <param name="paramA">The first parameter of the operation</param>
        /// <param name="paramB">The second parameter of the operation</param>
        protected TwoParameterOperation(ReturningCommand paramA, VariableCall  paramB) : base(true, true)
        {
            ParamA = paramA;
            ParamB = paramB;
        }

        /// <summary>
        ///     Initializes a new TwoParameterOperation
        /// </summary>
        /// <param name="pars">The two parameters for the operation</param>
        protected TwoParameterOperation(Tuple<ReturningCommand, VariableCall> pars) : base(true, true)
        {
            ParamA = pars.Item1;
            ParamB = pars.Item2;
        }

        /// <summary>
        ///     The first parameter of the operation
        /// </summary>
        protected ReturningCommand ParamA { get; }

        /// <summary>
        ///     The second parameter of the operation
        /// </summary>
        protected VariableCall ParamB { get; }
    }
}