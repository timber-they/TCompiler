using TCompiler.Types.CompilerTypes;


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
        /// <param name="tCode">The original T code line</param>
        protected VariableCall (Variable variable, CodeLine tCode) : base (false, false, tCode) => Variable = variable;

        /// <summary>
        ///     The variable that is being called
        /// </summary>
        public Variable Variable { get; }
    }
}