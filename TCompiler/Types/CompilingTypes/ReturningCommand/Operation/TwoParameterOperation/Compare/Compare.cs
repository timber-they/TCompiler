using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     The base class for compare operations like bigger
    /// </summary>
    public abstract class Compare : TwoParameterOperation
    {
        /// <summary>
        ///     Initializes a new compare operation
        /// </summary>
        /// <param name="paramA">The parameter that gets compared</param>
        /// <param name="paramB">The parameter to compare with</param>
        /// <param name="cLine">The original T code line</param>
        protected Compare (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) : base (paramA, paramB,
                                                                                                     cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a comparison
        /// </summary>
        /// <returns>The code to execute in assembler as a string</returns>
        public abstract override string ToString ();
    }
}