using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when there were over 10 registers in use
    /// </summary>
    [Serializable]
    public class TooManyRegistersException : TooManyException
    {
        /// <summary>
        ///     Initializes a new TooManyRegistersException
        /// </summary>
        /// <param name="lineIndex">The line in which the exception got thrown</param>
        /// <param name="message">The message that is shown to the user</param>
        public TooManyRegistersException(int lineIndex,
            string message = "There are not enough Registers! You'll have to solve this differently.")
            : base(lineIndex, message)
        {
        }
    }
}