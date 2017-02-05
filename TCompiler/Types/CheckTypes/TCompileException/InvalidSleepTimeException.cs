#region

using System;
using TCompiler.Types.CompilerTypes;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when no loop ranges can get evaluated for the given sleep time
    /// </summary>
    [Serializable]
    public class InvalidSleepTimeException : CompileException
    {
        /// <summary>
        ///     Initializes a new InvalidSleepTimeException
        /// </summary>
        /// <param name="codeLine">The line in which the exception got thrown</param>
        /// <param name="value">The value of the sleep time</param>
        /// <param name="message">The message that gets shown to the user</param>
        public InvalidSleepTimeException(CodeLine codeLine, int value,
            string message = "This won't work with that time ({0})")
            : base(codeLine, string.Format(message, value))
        {
            Value = value;
        }

        /// <summary>
        ///     The value of the sleep time
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int Value { get; }
    }
}