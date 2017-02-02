using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when the ByteCounter got too big, so it reached the area of the SFR
    /// </summary>
    [Serializable]
    public class TooManyValuesException : TooManyException
    {
        /// <summary>
        ///     Initializes a new TooManyValuesException
        /// </summary>
        /// <param name="lineIndex">The line in which the exception got thrown</param>
        /// <param name="message">The message that will get shown to the user</param>
        public TooManyValuesException(int lineIndex, string message = "There are too many Values! Try making them local.")
            : base(lineIndex, message)
        {
        }
    }
}