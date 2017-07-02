using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     The base class for one parameter operations like increment
    /// </summary>
    public abstract class OneParameterOperation : Operation
    {
        /// <summary>
        ///     Initializes a new OneParameterOperation
        /// </summary>
        /// <param name="parameter">The parameter for the operation</param>
        /// <param name="cLine">The original T code line</param>
        protected OneParameterOperation (ReturningCommand parameter, CodeLine cLine) : base (true, true, cLine) =>
            Parameter = parameter ?? throw new InvalidParameterException("first", cLine);

        /// <summary>
        ///     The parameter for the operation
        /// </summary>
        protected ReturningCommand Parameter { get; }
    }
}