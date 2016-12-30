namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSyntaxException : TCompileException
    {
        public InvalidSyntaxException(int line, string message = "The syntax isn't correct.") : base(line, message)
        {
        }
    }
}