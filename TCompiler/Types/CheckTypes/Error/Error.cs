using TCompiler.Enums;

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
        private CommandType DependsOn { get; }
        public int Line { get; }
        private ErrorType Type { get; }
    }
}