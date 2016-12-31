namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSyntaxException : CompileException
    {
        public InvalidSyntaxException(int line, string message = "The syntax isn't correct.") : base(line, message)
        {
        }
    }
}