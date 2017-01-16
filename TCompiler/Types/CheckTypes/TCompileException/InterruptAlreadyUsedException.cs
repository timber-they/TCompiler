using TCompiler.Enums;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InterruptAlreadyUsedException : CompileException
    {
        public InterruptAlreadyUsedException(int line, InterruptType type, string message="The interrupt type {0} is already used!") : base(line, string.Format(message, type))
        {
        }
    }
}