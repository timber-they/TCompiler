using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     Complements the parameter<br />
    ///     Syntax:<br />
    ///     !parameter
    /// </summary>
    public class Not : OneParameterOperation
    {
        /// <summary>
        ///     Initializes a new not operation
        /// </summary>
        /// <param name="parameter">The parameter to complement</param>
        /// <param name="cLine">The original T code line</param>
        public Not(ReturningCommand parameter, CodeLine cLine) : base(parameter, cLine)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a not
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"{Parameter}\ncpl A";
    }
}