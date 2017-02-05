#region

using System;
using TCompiler.Types.CompilerTypes;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when the user made a syntactical mistake
    /// </summary>
    [Serializable]
    public class InvalidSyntaxException : CompileException
    {
        /// <summary>
        ///     Initializes a new InvalidSyntaxException
        /// </summary>
        /// <param name="codeLine">The line in which the exception got thrown</param>
        /// <param name="message">The message that will be shown to the user</param>
        public InvalidSyntaxException(CodeLine codeLine, string message = "The syntax isn't correct.")
            : base(codeLine, message)
        {
        }
    }
}