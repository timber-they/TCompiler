#region

using System;

using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when the value wasn't valid in the context (e.g. too big)
    /// </summary>
    [Serializable]
    public class InvalidValueException : CompileException
    {
        /// <summary>
        ///     Initializes a new InvalidValueException
        /// </summary>
        /// <param name="codeLine">The line in which the exception got thrown</param>
        /// <param name="value">The invalid value</param>
        /// <param name="message">The message that is shown to the user</param>
        public InvalidValueException (
            CodeLine codeLine, string value,
            string message = "The value entered ({0}) is not valid for this type")
            : base (codeLine, string.Format (message, value)) {}
    }
}