using TCompiler.Enums;

namespace TCompiler.Types.CheckTypes.Error
{
    public class Error
    {
        public Error(CommandType dependsOn, string message, int line, ErrorType type)
        {
            DependsOn = dependsOn;
            Message = message;
            this.line = line;
            Type = type;
        }

        public string Message { get; }
        public CommandType DependsOn { get; }
        public int line { get; }
        public ErrorType Type { get; }
    }
}