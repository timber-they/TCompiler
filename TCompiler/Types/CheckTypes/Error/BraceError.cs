using TCompiler.Enums;

namespace TCompiler.Types.CheckTypes.Error
{
    public class BraceError : Error
    {
        public BraceError(CommandType dependsOn, string message, int line, ErrorType type) : base(dependsOn, message, line, type)
        {
        }
    }
}