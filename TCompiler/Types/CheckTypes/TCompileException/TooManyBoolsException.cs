using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Occurs when the bitaddressable area in the RAM has overflowed
    /// </summary>
    [Serializable]
    public class TooManyBoolsException : TooManyException
    {
        /// <summary>
        ///     Initializes a new TooManyBoolsException
        /// </summary>
        /// <param name="line">The line in which the exception got thrown</param>
        /// <param name="message">The message that is shown to the user</param>
        public TooManyBoolsException(int line,
            string message = "There are too many Boolean values! Try making them local.") : base(line, message)
        {
        }
    }
}