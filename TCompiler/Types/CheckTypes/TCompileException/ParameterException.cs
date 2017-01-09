namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when a parameter isn't valid
    /// </summary>
    public class ParameterException : CompileException
    {
        /// <summary>
        /// Initializes a new ParameterException
        /// </summary>
        /// <param name="line">The line in which the exception got thrown</param>
        /// <param name="parameter">The invalid parameter</param>
        /// <param name="message">The message that is shown to the user</param>
        public ParameterException(int line, string parameter, string message = "The parameter ({0}) is not valid!") : base(line, string.Format(message, parameter))
        {
        }
    }
}