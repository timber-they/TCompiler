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
        protected OneParameterOperation(ReturningCommand parameter) : base(true, true)
        {
            Parameter = parameter;
        }

        /// <summary>
        ///     The parameter for the operation
        /// </summary>
        protected ReturningCommand Parameter { get; }
    }
}