#region

using System;

using TCompiler.Types.CheckTypes.TCompileException;
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
            ParamA = paramA ?? throw new InvalidParameterException("first", cLine);
            ParamB = paramB ?? throw new InvalidParameterException ("second", cLine);
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