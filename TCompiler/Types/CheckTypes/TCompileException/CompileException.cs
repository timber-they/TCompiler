#region

using System;

using TCompiler.Types.CompilerTypes;

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
        /// <param name="codeLine">The line in which the exception got thrown</param>
        /// <param name="message">The message to show to the user</param>
        protected CompileException (CodeLine codeLine, string message) : base (message) => CodeLine = codeLine;

        /// <summary>
        ///     The line the exception got thrown
        /// </summary>
        public CodeLine CodeLine { get; }
    }
}