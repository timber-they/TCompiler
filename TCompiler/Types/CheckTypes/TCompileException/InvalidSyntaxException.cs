using System;

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
        /// <param name="line">The line in which the exception got thrown</param>
        /// <param name="message">The message that will be shown to the user</param>
        public InvalidSyntaxException(int line, string message = "The syntax isn't correct.") : base(line, message)
        {
        }
    }
}