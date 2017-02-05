using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    ///     The command to return (a value) from a method<br />
    ///     Syntax:<br />
    ///     return returningCommand
    /// </summary>
    public class Return : ReturningCommand
    {
        /// <summary>
        ///     The value that is being returned
        /// </summary>
        /// <remarks>Can be null</remarks>
        private readonly ReturningCommand _toReturn;

        /// <summary>
        ///     Initializes a new return command
        /// </summary>
        /// <param name="toReturn"></param>
        /// <param name="cLine">The original T code line</param>
        public Return(ReturningCommand toReturn, CodeLine cLine)
            : base(true, false, cLine)
        {
            _toReturn = toReturn;
        }

        /// <summary>
        ///     Evaluates the assembler code to execute this command
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => _toReturn != null ? $"{_toReturn}\nret" : "ret";
    }
}