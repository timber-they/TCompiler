#region

using TCompiler.Enums;

#endregion

namespace TCompiler.Types.CheckTypes.Error
{
    public abstract class Error
    {
        protected Error(CommandType dependsOn, string message, int line, ErrorType type)
        {
            DependsOn = dependsOn;
            Message = message;
            Line = line;
            Type = type;
        }

        public string Message { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private CommandType DependsOn { get; }
        public int Line { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private ErrorType Type { get; }
    }
}