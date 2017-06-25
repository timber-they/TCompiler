#region

using System;

using TCompiler.Types.CompilerTypes;

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
        /// <param name="cLine">The original T code line</param>
        protected TwoParameterOperation (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) : base (true,
                                                                                                                   true,
                                                                                                                   cLine)
        {
            ParamA = paramA;
            ParamB = paramB;
        }

        /// <summary>
        ///     Initializes a new TwoParameterOperation
        /// </summary>
        /// <param name="pars">The two parameters for the operation</param>
        /// <param name="cLine">The original T code line</param>
        protected TwoParameterOperation (Tuple<ReturningCommand, ReturningCommand> pars, CodeLine cLine) : base (true,
                                                                                                                 true,
                                                                                                                 cLine)
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
        protected ReturningCommand ParamB { get; }
    }
}