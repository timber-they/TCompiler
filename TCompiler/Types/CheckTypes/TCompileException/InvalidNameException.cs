#region

using System;

using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when the name wasn't valid (didn't exist in the context)
    /// </summary>
    [Serializable]
    public class InvalidNameException : CompileException
    {
        /// <summary>
        ///     Initializes a new InvalidNameException
        /// </summary>
        /// <param name="codeLine">The line in which the exception got thrown</param>
        /// <param name="name">The invalid name</param>
        /// <param name="message">The message to show the user</param>
        public InvalidNameException (
            CodeLine codeLine, string name,
            string message = "The name ({0}) entered is not valid!")
            : base (codeLine, string.Format (message, name)) {}
    }
}