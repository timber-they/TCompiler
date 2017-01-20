namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The base class for every variable call (bit/byte)
    /// </summary>
    public abstract class VariableCall : ReturningCommand
    {
        /// <summary>
        ///     Initializes a new VariableCall
        /// </summary>
        /// <param name="variable">The variable that is being called</param>
        protected VariableCall(Variable variable) : base(false, false, new[] {1})
        {
            Variable = variable;
        }

        /// <summary>
        ///     The variable that is being called
        /// </summary>
        public Variable Variable { get; }
    }
}