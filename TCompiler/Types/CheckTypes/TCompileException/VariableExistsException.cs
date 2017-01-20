using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when the user tried to declare a variable that already exists
    /// </summary>
    [Serializable]
    public class VariableExistsException : CompileException
    {
        /// <summary>
        ///     Initializes a new VariableExistsException
        /// </summary>
        /// <param name="line">The line where the exceptio got thrown</param>
        /// <param name="variable">The variable that already exists</param>
        /// <param name="message">The message that is shown to the user</param>
        public VariableExistsException(int line, string variable,
            string message = "The entered variable name ({0}) already exists!")
            : base(line, string.Format(message, variable))
        {
        }
    }
}