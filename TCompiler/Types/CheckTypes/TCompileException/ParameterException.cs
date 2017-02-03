#region

using System;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when a parameter isn't valid
    /// </summary>
    [Serializable]
    public class ParameterException : CompileException
    {
        /// <summary>
        ///     Initializes a new ParameterException
        /// </summary>
        /// <param name="lineIndex">The line in which the exception got thrown</param>
        /// <param name="parameter">The invalid parameter</param>
        /// <param name="message">The message that is shown to the user</param>
        public ParameterException(int lineIndex, string parameter, string message = "The parameter ({0}) is not valid!")
            : base(lineIndex, string.Format(message, parameter))
        {
        }
    }
}