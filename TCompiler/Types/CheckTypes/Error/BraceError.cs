#region

using TCompiler.Enums;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.Error
{
    /// <summary>
    ///     An error for braces (e.g. too many opening/closing braces)
    /// </summary>
    public class BraceError : Error
    {
        /// <summary>
        ///     Initiates a new brace error
        /// </summary>
        /// <param name="dependsOn">The command this error depends on</param>
        /// <param name="message">The message to show the user</param>
        /// <param name="codeLine">The index of the line the error occured in</param>
        /// <param name="type">The type of the error</param>
        public BraceError (CommandType dependsOn, string message, CodeLine codeLine, ErrorType type)
            : base (dependsOn, message, codeLine, type) {}
    }
}