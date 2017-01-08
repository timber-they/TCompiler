namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class BitOfVariableException : CompileException
    {
        public BitOfVariableException(int line, string message="A bitOf variable is not valid in this context") : base(line, message)
        {
        }
    }
}