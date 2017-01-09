namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when the value wasn't valid in the context (e.g. too big)
    /// </summary>
    public class InvalidValueException : CompileException
    {
        /// <summary>
        /// Initializes a new InvalidValueException
        /// </summary>
        /// <param name="line">The line in which the exception got thrown</param>
        /// <param name="value">The invalid value</param>
        /// <param name="message">The message that is shown to the user</param>
        public InvalidValueException(int line, string value, string message = "The value entered ({0}) is not valid for this type")
            : base(line, string.Format(message, value))
        {
        }
    }
}