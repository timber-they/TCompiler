#region

using TCompiler.Enums;

#endregion

namespace TCompiler.Types.CheckTypes.Error
{
    /// <summary>
    ///     The base class for pre-compile errors
    /// </summary>
    public abstract class Error
    {
        /// <summary>
        ///     The constructor - initiates a new error
        /// </summary>
        /// <returns>Nothing</returns>
        protected Error(CommandType dependsOn, string message, int lineIndex, ErrorType type)
        {
            DependsOn = dependsOn;
            Message = message;
            LineIndex = lineIndex;
            Type = type;
        }

        /// <summary>
        ///     The message of the error the user will see
        /// </summary>
        /// <value>The message in form of a string</value>
        public string Message { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        /// <summary>
        ///     Specifies on which the command depends on
        /// </summary>
        /// <value>the command it depends on</value>
        private CommandType DependsOn { get; }

        /// <summary>
        ///     The line in which the error appeared
        /// </summary>
        /// <value>The line index</value>
        public int LineIndex { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        /// <summary>
        ///     The type of the error
        /// </summary>
        /// <value>As an enumerable</value>
        private ErrorType Type { get; }
    }
}