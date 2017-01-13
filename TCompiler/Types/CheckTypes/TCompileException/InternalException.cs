namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when a not-CompileException gets thrown
    /// </summary>
    public class InternalException : CompileException
    {
        /// <summary>
        /// Initializes a new InternalException
        /// </summary>
        /// <param name="exceptionMessage">The message of the exception that got thrown</param>
        /// <param name="message">The message to show the user</param>
        public InternalException(string exceptionMessage,
            string message = "An internal error ({0}) occurred.\n" +
                             "I feel really sorry about that.\n" +
                             "If you're a nice guy, I'd be happy if you sent me this message :)")
            : base(0, string.Format(message, exceptionMessage))
        {
        }
    }
}