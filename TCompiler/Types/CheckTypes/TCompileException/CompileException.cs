#region

using System;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// The base exception for compile errors
    /// </summary>
    public abstract class CompileException : Exception
    {
        /// <summary>
        /// Initializes a new compile exception
        /// </summary>
        /// <param name="line">The line the exception got thrown</param>
        /// <param name="message">The message to show to the user</param>
        protected CompileException(int line, string message) : base(message)
        {
            Line = line;
        }

        /// <summary>
        /// The line the exception got thrown
        /// </summary>
        public int Line { get; }
    }
}