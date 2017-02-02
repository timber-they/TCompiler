#region

using System;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     The base exception for compile errors
    /// </summary>
    [Serializable]
    public abstract class CompileException : NotSupportedException
    {
        /// <summary>
        ///     Initializes a new compile exception
        /// </summary>
        /// <param name="lineIndex">The line the exception got thrown</param>
        /// <param name="message">The message to show to the user</param>
        protected CompileException(int lineIndex, string message) : base(message)
        {
            LineIndex = lineIndex;
        }

        /// <summary>
        ///     The line the exception got thrown
        /// </summary>
        public int LineIndex { get; }
    }
}