using TCompiler.Enums;

namespace TCompiler.Types.CheckTypes.Error
{
    public class BlockError : Error
    {
        public BlockError(CommandType dependsOn, string message, int line, ErrorType type)
            : base(dependsOn, message, line, type)
        {
        }
    }
}