#region

using System;

using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     This exception gets thrown when the bitOf can't be used in the context it was used
    /// </summary>
    [Serializable]
    public class BitOfVariableException : CompileException
    {
        /// <summary>
        ///     Initializes a new BitOfVariable
        /// </summary>
        /// <param name="codeLine">The line the exception got thrown</param>
        /// <param name="message">The message that is shown to the user</param>
        public BitOfVariableException (
            CodeLine codeLine,
            string   message = "A bitOf variable is not valid in this context")
            : base (codeLine, message) {}
    }
}