using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class MethodExistsException : CompileException
    {
        public MethodExistsException (
            CodeLine codeLine, string methodName,
            string message = "The method {0} already exists!") : base (codeLine, string.Format (message, methodName)) {}
    }
}