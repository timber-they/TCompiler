namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when a pre-compile error occurs
    /// </summary>
    public class PreCompileErrorException : CompileException
    {
        /// <summary>
        /// Initializes a new PreCompileErrorException
        /// </summary>
        /// <param name="error">The PreCompileError</param>
        public PreCompileErrorException(Error.Error error) : base(error.Line, error.Message)
        {
        }
    }
}