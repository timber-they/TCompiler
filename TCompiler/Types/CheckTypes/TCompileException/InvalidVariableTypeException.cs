#region

using System;

using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when a type of a variable isn't valid
    /// </summary>
    [Serializable]
    public class InvalidVariableTypeException : CompileException
    {
        /// <summary>
        ///     Initiates a new InvalidVariableTypeException
        /// </summary>
        /// <param name="codeLine">The line in which the error occurred</param>
        /// <param name="variableName">The name of the invalid variable</param>
        /// <param name="message">The message to show the user</param>
        public InvalidVariableTypeException (
            CodeLine codeLine, string variableName,
            string   message = "The variable {0} has an invalid type!")
            : base (codeLine, string.Format (message, variableName)) {}
    }
}