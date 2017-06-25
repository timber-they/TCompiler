#region

using System;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when a not-CompileException gets thrown
    /// </summary>
    [Serializable]
    public class InternalException : CompileException
    {
        /// <summary>
        ///     Initializes a new InternalException
        /// </summary>
        /// <param name="exceptionMessage">The message of the exception that got thrown</param>
        /// <param name="fileName">The name of the file the exception got thrown. Null if no value is available</param>
        /// <param name="message">The message to show the user</param>
        /// <param name="lineIndex">The index of the line the exception got thrown. Null if no value is available</param>
        public InternalException (
            string exceptionMessage, int? lineIndex, string fileName,
            string message = "An internal error ({0}) occurred.\n" +
                             "I feel really sorry about that.\n" +
                             "{1}" +
                             "If you're a nice guy, I'd be happy if you sent me this message :)\n" +
                             "E-Mail: timo@teemze.de")
            : base (
                null,
                string.Format (message, exceptionMessage,
                               lineIndex != null ? $"Line {lineIndex} in {fileName}\n" : "")) {}
    }
}