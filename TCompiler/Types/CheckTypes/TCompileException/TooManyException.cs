using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     The base class for exception where the quantity of something is too big
    /// </summary>
    [Serializable]
    public abstract class TooManyException : CompileException
    {
        /// <summary>
        ///     Initializes a new TooManyException
        /// </summary>
        /// <param name="lineIndex">The line in which the exception got thrown</param>
        /// <param name="message">The message that is shown to the user</param>
        protected TooManyException(int lineIndex, string message) : base(lineIndex, message)
        {
        }
    }
}