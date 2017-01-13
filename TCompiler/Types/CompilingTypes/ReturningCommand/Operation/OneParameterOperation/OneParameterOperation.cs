#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    /// The base class for one parameter operations like increment
    /// </summary>
    public abstract class OneParameterOperation : Operation
    {
        /// <summary>
        /// Initializes a new OneParameterOperation
        /// </summary>
        /// <param name="parameter">The parameter for the operation</param>
        protected OneParameterOperation(VariableCall parameter) : base(true, true, new []{1,2})
        {
            Parameter = parameter;
        }

        /// <summary>
        /// The parameter for the operation
        /// </summary>
        protected VariableCall Parameter { get; }
    }
}