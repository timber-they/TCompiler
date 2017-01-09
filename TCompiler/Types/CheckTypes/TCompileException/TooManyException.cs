namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// The base class for exception where the quantity of something is too big
    /// </summary>
    public abstract class TooManyException : CompileException
    {
        /// <summary>
        /// Initializes a new TooManyException
        /// </summary>
        /// <param name="line">The line in which the exception got thrown</param>
        /// <param name="message">The message that is shown to the user</param>
        protected TooManyException(int line, string message) : base(line, message)
        {
        }
    }
}